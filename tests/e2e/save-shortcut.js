// Regression suite: Ctrl+S saves from inside a field on the edit pages.
// Run with tests/e2e/run.ps1 (see README.md). Executed by `playwright-cli run-code`, so `page` is a Playwright Page.
async (page) => {
  page.on('dialog', d => d.accept());
  page.setDefaultTimeout(8000);

  const base = new URL(page.url()).origin;
  const stamp = `zz-e2e-${Date.now()}`;
  const out = [];
  const flush = () => page.evaluate(o => localStorage.setItem('e2eResults', o), out.join('\n')).catch(() => {});
  const log = (name, ok, detail = '') => {
    out.push(`${ok ? 'PASS' : 'FAIL'} ${name}${detail ? ` -- ${detail}` : ''}`);
    flush();
  };

  const entities = [
    { key: 'recipe', seg: 'recipes', api: 'recipes', name: '#name' },
    { key: 'category', seg: 'categories', api: 'categories', name: '#name' },
    { key: 'aisle', seg: 'grocery-aisles', api: 'grocery-aisles', name: '#name' },
    { key: 'store', seg: 'grocery-stores', api: 'grocery-stores', name: '#name' },
    { key: 'location', seg: 'storage-locations', api: 'storage-locations', name: '#name' },
    { key: 'item', seg: 'grocery-items', api: 'grocery-items', name: '#name' },
    { key: 'mealplan', seg: 'meal-plans', api: 'meal-plans', name: '#mealPlanName' },
  ];

  const getField = async (api, id, field) =>
    page.evaluate(([u, f]) => fetch(u).then(r => r.json()).then(j => j[f]), [`/api/${api}/${id}`, field]);
  // Poll until the saved field has the expected value (or give up).
  const savedAs = async (api, id, field, predicate) => {
    for (let i = 0; i < 12; i += 1) {
      const value = await getField(api, id, field);
      if (predicate(value)) {
        return true;
      }
      await page.waitForTimeout(300);
    }
    return false;
  };
  const modalShown = () => page.waitForSelector('.modal.show').then(() => true).catch(() => false);
  // After a failure the page may be dirty; stop its beforeunload prompt so the next navigation can't hang on a native dialog.
  const disarmUnloadPrompt = () =>
    page.evaluate(() => window.addEventListener('beforeunload', ev => ev.stopImmediatePropagation(), true)).catch(() => {});
  // Registered after the app's own listener, so it sees whether the app cancelled the event.
  const watchDefaultPrevented = () => page.evaluate(() => {
    window.e2eDefaultPrevented = null;
    if (!window.e2eWatching) {
      window.e2eWatching = true;
      window.addEventListener('keydown', (e) => {
        if (e.key.toLowerCase() === 's') {
          window.e2eDefaultPrevented = e.defaultPrevented;
        }
      });
    }
  });
  const defaultPrevented = () => page.evaluate(() => window.e2eDefaultPrevented);

  for (const e of entities) {
    const k = e.key;
    let id = null;

    try {
      await page.goto(`${base}/${e.seg}/new`);
      await page.waitForSelector(e.name);
      await page.fill(e.name, stamp);
      await page.click('button:has-text("Save")');
      await page.waitForURL(u => /\/(\d+)(\/|$)/.test(new URL(u).pathname) && !new URL(u).pathname.endsWith('/new'));
      id = new URL(page.url()).pathname.match(/\/(\d+)(\/|$)/)[1];
      // The edit page reloads the record and resets its form, so wait for that before editing.
      await page.waitForLoadState('networkidle');
      await page.waitForFunction(([sel, s]) => document.querySelector(sel).value.toLowerCase() === s, [e.name, stamp]);

      // Ctrl+S from inside a text field saves and suppresses the browser's own dialog.
      await page.fill(e.name, `${stamp}-shortcut`);
      await page.focus(e.name);
      await watchDefaultPrevented();
      await page.keyboard.press('Control+s');
      log(`${k}: ctrl+s saves from a field`, await savedAs(e.api, id, 'name', v => String(v).toLowerCase() === `${stamp}-shortcut`));
      log(`${k}: ctrl+s suppresses the browser dialog`, (await defaultPrevented()) === true);
      const savedName = await getField(e.api, id, 'name');

      if (k === 'recipe') {
        // The rich text editor is a contenteditable, not an input.
        await page.click('#directions .ProseMirror');
        await page.keyboard.type('shortcut directions');
        await page.keyboard.press('Control+s');
        log('recipe: ctrl+s saves from the rich text editor', await savedAs(e.api, id, 'directions', v => String(v).includes('shortcut directions')));
      }

      // Other chords are left alone.
      await page.fill(e.name, `${stamp}-shift`);
      await page.focus(e.name);
      await watchDefaultPrevented();
      await page.keyboard.press('Control+Shift+S');
      await page.waitForTimeout(900);
      log(`${k}: ctrl+shift+s does not save`, (await getField(e.api, id, 'name')) === savedName && (await defaultPrevented()) === false);

      // With a dialog open the shortcut must not save from underneath it (but still blocks the browser dialog).
      await page.fill(e.name, savedName);
      await page.click('#overflowMenuButton');
      await page.click('.dropdown-menu .dropdown-item.text-danger');
      await modalShown();
      await watchDefaultPrevented();
      await page.keyboard.press('Control+s');
      await page.waitForTimeout(900);
      log(`${k}: ctrl+s is ignored while a dialog is open`, (await getField(e.api, id, 'name')) === savedName && (await defaultPrevented()) === true);

      // Confirming the open delete dialog also cleans up the record.
      await page.click('.modal.show .btn-primary');
      await page.waitForURL(u => !/\/\d+(\/|$)/.test(new URL(u).pathname));
      log(`${k}: test record deleted`, true);
    } catch (err) {
      log(`${k}: scenario`, false, `${String(err).split('\n')[0]}${id ? ` (delete the ${k} named "${stamp}*" by hand)` : ''}`);
      await disarmUnloadPrompt();
    }
  }

  out.push('DONE');
  await flush();
  return out.join('\n');
}
