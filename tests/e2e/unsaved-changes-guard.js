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
  // Optional comma-separated list of entity keys to run (set by `run.ps1 -Only`).
  const only = (await page.evaluate(() => localStorage.getItem('e2eOnly')) || '').split(',').filter(Boolean);

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
    { key: 'mealplan', seg: 'meal-plans', existing: await firstOf('/api/meal-plans?take=1', j => j.items[0].id), name: '#mealPlanName' },
  ];

  const loaded = async (sel) => {
    await page.waitForSelector(sel);
    await page.waitForFunction(s => document.querySelector(s).value.length > 0, sel);
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

  for (const e of entities.filter(x => only.length === 0 || only.includes(x.key))) {
    const url = `${base}/${e.seg}/${e.existing}`;
    const k = e.key;
    // The selector of the field holding the record's name.
    const sel = e.name || '#name';

    // Existing record: clean and dirty navigation.
    try {
      await page.goto(url);
      await loaded(sel);
      log(`${k}: clean beforeunload not prevented`, (await unloadPrevented()) === false);
      await page.click(nav);
      await page.waitForURL('**/meal-plans', { timeout: 5000 });
      log(`${k}: clean nav leaves without prompt`, true);

      await page.goto(url);
      await loaded(sel);
      const original = await page.inputValue(sel);
      await page.fill(sel, `${original} x`);
      log(`${k}: dirty beforeunload prevented`, (await unloadPrevented()) === true);
      await page.click(nav);
      log(`${k}: dirty nav shows prompt`, await modalShown());
      log(`${k}: dirty nav stays on page`, page.url() === url);
      await page.click('.modal.show .btn-secondary');
      await page.waitForSelector('.modal.show', { state: 'detached', timeout: 3000 }).catch(() => {});
      log(`${k}: cancel keeps edits`, (await page.inputValue(sel)) === `${original} x`);
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
      await page.waitForSelector(sel);
      await page.fill(sel, stamp);
      await page.click('button:has-text("Save")');
      await page.waitForURL(u => /\/(\d+)(\/|$)/.test(new URL(u).pathname) && !new URL(u).pathname.endsWith('/new'), { timeout: 8000 });
      createdUrl = page.url();
      log(`${k}: create redirects to edit without prompt`, !(await modalShown(500)));
      // The edit page reloads the record and resets its form, so wait for that before editing.
      await page.waitForLoadState('networkidle');
      // Names are title-cased by the server for some entities.
      await page.waitForFunction(([s, v]) => document.querySelector(s).value.toLowerCase() === v, [sel, stamp]);
      createdName = await page.inputValue(sel);
      log(`${k}: clean after create`, (await unloadPrevented()) === false);
      await page.fill(sel, `${stamp} y`);
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
        await page.fill(sel, createdName);
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

  // Meal plans take a different path when editing the current plan, so cover that too.
  if (only.length === 0 || only.includes('mealplan')) {
    const sel = '#mealPlanName';
    let id = null;
    try {
      await page.goto(`${base}/meal-plans/new`);
      await page.waitForSelector(sel);
      await page.fill(sel, stamp);
      await page.click('button:has-text("Save")');
      await page.waitForURL(u => /\/meal-plans\/\d+/.test(u.toString()), { timeout: 8000 });
      id = new URL(page.url()).pathname.match(/\/(\d+)/)[1];
      await page.waitForLoadState('networkidle');
      await page.getByRole('button', { name: 'Make Current', exact: true }).click();
      await page.waitForFunction(() => !Array.from(document.querySelectorAll('button')).some(b => b.textContent.trim() === 'Make Current'));
      await page.waitForLoadState('networkidle');
      await page.fill(sel, `${stamp} cur`);
      log('mealplan current: dirty beforeunload prevented', (await unloadPrevented()) === true);
      // The Meal Plans link can change once a plan is current, so use the brand link, which doesn't.
      await page.click('a.navbar-brand');
      log('mealplan current: dirty nav shows prompt', await modalShown());
      await page.click('.modal.show .btn-secondary');
      await page.waitForSelector('.modal.show', { state: 'detached', timeout: 3000 }).catch(() => {});
      await page.press(sel, 'Enter');
      let saved = false;
      for (let i = 0; i < 12 && !saved; i += 1) {
        saved = await page.evaluate(i2 => fetch(`/api/meal-plans/${i2}`).then(r => r.json()).then(j => /cur$/.test(j.name)), id);
        if (!saved) {
          await page.waitForTimeout(300);
        }
      }
      log('mealplan current: enter saves the name', saved);
      await page.waitForTimeout(600);
      log('mealplan current: clean after saving', (await unloadPrevented()) === false);
      await page.click('#overflowMenuButton');
      await page.click('.dropdown-menu .dropdown-item.text-danger');
      await modalShown();
      await page.click('.modal.show .btn-primary');
      await page.waitForURL(u => !/\/meal-plans\/\d+/.test(u.toString()), { timeout: 8000 });
      log('mealplan current: test record deleted', true);
    } catch (err) {
      log('mealplan current: scenario', false, `${String(err).split('\n')[0]}${id ? ` (delete the meal plan named "${stamp}*" by hand)` : ''}`);
      await disarmUnloadPrompt();
    }
  }

  out.push('DONE');
  await flush();
  return out.join('\n');
}
