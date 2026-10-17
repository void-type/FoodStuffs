// Regression suite: unsaved-changes guard on the edit pages.
// Run with tests/e2e/run.ps1 (see README.md). Executed by `playwright-cli run-code`, so `page` is a Playwright Page.
async (page) => {
  page.on('dialog', d => d.accept());
  page.setDefaultTimeout(8000);

  const base = new URL(page.url()).origin;
  const stamp = `zz-e2e-${Date.now()}`;
  const out = [];
  const log = (name, ok, detail = '') => out.push(`${ok ? 'PASS' : 'FAIL'} ${name}${detail ? ` -- ${detail}` : ''}`);
  const nav = 'a.nav-link[href="/meal-plans"]';
  // The CLI can return before the script finishes, so results are persisted where the runner can poll them.
  const flush = () => page.evaluate(o => localStorage.setItem('e2eResults', o), out.join('\n')).catch(() => {});

  const getJson = path => page.evaluate(p => fetch(p).then(r => r.json()), path);

  // Pick existing records to edit instead of depending on specific ids.
  const firstOf = async (path, pick) => pick(await getJson(path));
  const recipe = await firstOf('/api/recipes?take=1&isForMealPlanning=', j => j.results.items[0]);
  const entities = [
    { key: 'category', seg: 'categories', existing: await firstOf('/api/categories?take=1', j => j.items[0].id) },
    { key: 'aisle', seg: 'grocery-aisles', existing: await firstOf('/api/grocery-aisles?take=1', j => j.items[0].id) },
    { key: 'store', seg: 'grocery-stores', existing: await firstOf('/api/grocery-stores?take=1', j => j.items[0].id) },
    { key: 'location', seg: 'storage-locations', existing: await firstOf('/api/storage-locations?take=1', j => j.items[0].id) },
    { key: 'item', seg: 'grocery-items', existing: await firstOf('/api/grocery-items?take=1', j => j.results.items[0].id) },
    { key: 'recipe', seg: 'recipes', existing: `${recipe.id}/${recipe.slug}/edit` },
  ];

  const loaded = async () => {
    await page.waitForSelector('#name');
    await page.waitForFunction(() => document.querySelector('#name').value.length > 0);
  };
  const modalShown = async (ms = 2500) =>
    page.waitForSelector('.modal.show', { timeout: ms }).then(() => true).catch(() => false);
  const unloadPrevented = () =>
    page.evaluate(() => {
      const ev = new Event('beforeunload', { cancelable: true });
      window.dispatchEvent(ev);
      return ev.defaultPrevented;
    });
  // After a failure the page may be dirty; stop its beforeunload prompt so the next navigation can't hang on a native dialog.
  const disarmUnloadPrompt = () =>
    page.evaluate(() => window.addEventListener('beforeunload', ev => ev.stopImmediatePropagation(), true)).catch(() => {});

  for (const e of entities) {
    const url = `${base}/${e.seg}/${e.existing}`;
    const k = e.key;

    // Existing record: clean and dirty navigation.
    try {
      await page.goto(url);
      await loaded();
      log(`${k}: clean beforeunload not prevented`, (await unloadPrevented()) === false);
      await page.click(nav);
      await page.waitForURL('**/meal-plans', { timeout: 5000 });
      log(`${k}: clean nav leaves without prompt`, true);

      await page.goto(url);
      await loaded();
      const original = await page.inputValue('#name');
      await page.fill('#name', `${original} x`);
      log(`${k}: dirty beforeunload prevented`, (await unloadPrevented()) === true);
      await page.click(nav);
      log(`${k}: dirty nav shows prompt`, await modalShown());
      log(`${k}: dirty nav stays on page`, page.url() === url);
      await page.click('.modal.show .btn-secondary');
      await page.waitForSelector('.modal.show', { state: 'detached', timeout: 3000 }).catch(() => {});
      log(`${k}: cancel keeps edits`, (await page.inputValue('#name')) === `${original} x`);
      await page.click(nav);
      await modalShown();
      await page.click('.modal.show .btn-primary');
      await page.waitForURL('**/meal-plans', { timeout: 5000 });
      log(`${k}: confirm leaves`, true);
      log(`${k}: listener removed after leaving`, (await unloadPrevented()) === false);
      if (k === 'recipe') {
        const recents = await page.evaluate(() => localStorage.getItem('recentRecipes') || '');
        log('recipe: left recipe added to recents', recents.includes(`"id":${recipe.id},`));
      }
    } catch (err) {
      log(`${k}: existing-entity scenarios`, false, String(err).split('\n')[0]);
      await disarmUnloadPrompt();
    }

    // New record: create, redirect without a prompt, then the guard must still work.
    let createdUrl = null;
    let createdName = null;
    try {
      await page.goto(`${base}/${e.seg}/new`);
      await page.waitForSelector('#name');
      await page.fill('#name', stamp);
      await page.click('button:has-text("Save")');
      await page.waitForURL(u => /\/(\d+)(\/|$)/.test(new URL(u).pathname) && !new URL(u).pathname.endsWith('/new'), { timeout: 8000 });
      createdUrl = page.url();
      log(`${k}: create redirects to edit without prompt`, !(await modalShown(500)));
      // Names are title-cased by the server for some entities.
      await page.waitForFunction(s => document.querySelector('#name').value.toLowerCase() === s, stamp);
      createdName = await page.inputValue('#name');
      log(`${k}: clean after create`, (await unloadPrevented()) === false);
      await page.fill('#name', `${stamp} y`);
      await page.click(nav);
      log(`${k}: dirty after create prompts`, await modalShown());
      await page.click('.modal.show .btn-secondary');
      await page.waitForSelector('.modal.show', { state: 'detached', timeout: 3000 }).catch(() => {});
    } catch (err) {
      log(`${k}: create flow`, false, String(err).split('\n')[0]);
      await disarmUnloadPrompt();
    }

    // Delete the test record in place; navigating away while dirty would raise a native dialog.
    try {
      if (!createdUrl) {
        continue;
      }
      // Undo the edit so the redirect after deleting isn't held up by the unsaved-changes prompt.
      if (createdName !== null) {
        await page.fill('#name', createdName);
      }
      await page.click('#overflowMenuButton');
      await page.click('.dropdown-menu .dropdown-item.text-danger');
      await modalShown();
      await page.click('.modal.show .btn-primary');
      await page.waitForURL(u => !/\/\d+(\/|$)/.test(new URL(u).pathname), { timeout: 8000 });
      log(`${k}: test record deleted`, true);
    } catch (err) {
      log(`${k}: cleanup`, false, `${String(err).split('\n')[0]} (delete records named "${stamp}" by hand)`);
    }
    await flush();
  }

  out.push('DONE');
  await flush();
  return out.join('\n');
}
