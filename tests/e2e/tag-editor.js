// Regression suite: keyboard flow of the tag editor (categories on recipes, storage locations on grocery items).
// Run with tests/e2e/run.ps1 (see README.md). Executed by `playwright-cli run-code`, so `page` is a Playwright Page.
// Nothing is saved: the pages are left unsaved, so no test data is created.
async (page) => {
  page.on('dialog', d => d.accept());
  page.setDefaultTimeout(8000);

  const base = new URL(page.url()).origin;
  const out = [];
  const flush = () => page.evaluate(o => localStorage.setItem('e2eResults', o), out.join('\n')).catch(() => {});
  const log = (name, ok, detail = '') => {
    out.push(`${ok ? 'PASS' : 'FAIL'} ${name}${detail ? ` -- ${detail}` : ''}`);
    flush();
  };
  const getJson = path => page.evaluate(p => fetch(p).then(r => r.json()), path);
  const disarmUnloadPrompt = () =>
    page.evaluate(() => window.addEventListener('beforeunload', ev => ev.stopImmediatePropagation(), true)).catch(() => {});

  // What the editor currently shows, read from the DOM.
  const snapshot = fieldId => page.evaluate((id) => {
    const input = document.getElementById(id);
    const root = input.closest('.input-group').parentElement;
    const options = [...root.querySelectorAll('.dropdown-menu.show .dropdown-item')];
    const active = options.find(o => o.classList.contains('active'));
    return {
      badges: [...root.querySelectorAll('button.badge')].map(b => b.textContent.trim()),
      options: options.map(o => o.textContent.trim()),
      active: active ? active.textContent.trim() : null,
      activeDescendant: input.getAttribute('aria-activedescendant') || '',
      inputValue: input.value,
      focusedId: document.activeElement?.id || '',
      focusedText: document.activeElement?.textContent?.trim() || '',
    };
  }, fieldId);
  const eq = (a, b) => JSON.stringify(a) === JSON.stringify(b);
  // Pages title-case new tags, so compare names ignoring case.
  const has = (badges, name) => name !== null && badges.some(b => b.toLowerCase() === name.toLowerCase());

  const runEditor = async (label, url, fieldId, names) => {
    try {
      await disarmUnloadPrompt();
      await page.goto(url);
      await page.waitForSelector(`#${fieldId}`);
      await page.waitForLoadState('networkidle');

      // Choose a prefix that matches several existing names so up/down navigation can be exercised.
      const prefixes = names.map(n => n.slice(0, 2).toLowerCase());
      const prefix = prefixes
        .map(p => ({ p, n: names.filter(n => n.toLowerCase().includes(p)).length }))
        .sort((a, b) => b.n - a.n)[0].p;

      await page.focus(`#${fieldId}`);
      await page.keyboard.type(prefix);
      await page.waitForSelector('.dropdown-menu.show .dropdown-item');
      const first = await snapshot(fieldId);
      log(`${label}: suggestions appear while typing`, first.options.length > 0, `typed "${prefix}"`);
      log(`${label}: nothing is highlighted until arrow down`, first.active === null && first.activeDescendant === '');

      await page.keyboard.press('ArrowDown');
      const down1 = await snapshot(fieldId);
      log(`${label}: arrow down highlights the first suggestion`, down1.active === first.options[0] && down1.activeDescendant !== '');
      log(`${label}: arrow down keeps the typed text and focus in the field`, down1.inputValue === prefix && down1.focusedId === fieldId);

      if (first.options.length > 1) {
        await page.keyboard.press('ArrowDown');
        log(`${label}: arrow down moves to the next suggestion`, (await snapshot(fieldId)).active === first.options[1]);
        await page.keyboard.press('ArrowUp');
        log(`${label}: arrow up moves back`, (await snapshot(fieldId)).active === first.options[0]);
      }
      await page.keyboard.press('ArrowUp');
      log(`${label}: arrow up from the first returns to the typed text`, (await snapshot(fieldId)).active === null);
      await page.keyboard.press('ArrowDown');

      // Enter in the field adds the highlighted suggestion and leaves focus in the field.
      const chosen = (await snapshot(fieldId)).active;
      await page.keyboard.press('Enter');
      const afterEnter = await snapshot(fieldId);
      log(`${label}: enter adds the highlighted suggestion`, has(afterEnter.badges, chosen), `chose "${chosen}"`);
      log(`${label}: field is cleared and focused after adding`, afterEnter.inputValue === '' && afterEnter.focusedId === fieldId);
      log(`${label}: suggestions close after adding`, afterEnter.options.length === 0);

      // Enter with nothing highlighted adds the typed text.
      await page.keyboard.type('zz e2e tag');
      await page.keyboard.press('Enter');
      const typed = await snapshot(fieldId);
      log(`${label}: enter adds typed text when nothing is highlighted`, typed.badges.some(b => b.toLowerCase() === 'zz e2e tag'), JSON.stringify(typed.badges));
      log(`${label}: focus stays in the field after adding typed text`, typed.focusedId === fieldId && typed.inputValue === '');

      // Already added tags aren't offered again.
      await page.keyboard.type(prefix);
      const again = await snapshot(fieldId);
      log(`${label}: added tags are not suggested again`, again.options.every(o => !has(again.badges, o)), JSON.stringify(again.options));

      // Tab moves to Add and Enter there adds the highlighted suggestion, then focus returns to the field.
      if (again.options.length > 0) {
        await page.keyboard.press('ArrowDown');
        const target = (await snapshot(fieldId)).active;
        await page.keyboard.press('Tab');
        const onAdd = await snapshot(fieldId);
        log(`${label}: tab moves focus to the Add button`, onAdd.focusedText === 'Add');
        log(`${label}: highlight survives tabbing to Add`, onAdd.active === target);
        await page.keyboard.press('Enter');
        const added = await snapshot(fieldId);
        log(`${label}: enter on Add adds the highlighted suggestion`, has(added.badges, target), `chose "${target}"`);
        log(`${label}: focus returns to the field after using Add`, added.focusedId === fieldId && added.inputValue === '');
      }

      // Escape closes the list without adding anything.
      await page.keyboard.type(prefix);
      await page.keyboard.press('ArrowDown');
      const badgesBefore = (await snapshot(fieldId)).badges;
      await page.keyboard.press('Escape');
      const esc = await snapshot(fieldId);
      log(`${label}: escape closes the suggestions`, esc.options.length === 0 && esc.active === null);
      log(`${label}: escape keeps the typed text and adds nothing`, esc.inputValue === prefix && eq(esc.badges, badgesBefore));

      // Clicking a suggestion still works and returns focus to the field.
      await page.fill(`#${fieldId}`, '');
      await page.keyboard.type(prefix);
      const beforeClick = await snapshot(fieldId);
      if (beforeClick.options.length > 0) {
        const clicked = beforeClick.options[0];
        await page.click('.dropdown-menu.show .dropdown-item');
        const afterClick = await snapshot(fieldId);
        log(`${label}: clicking a suggestion adds it`, has(afterClick.badges, clicked));
        log(`${label}: focus returns to the field after a click`, afterClick.focusedId === fieldId);
      }
    } catch (err) {
      log(`${label}: scenario`, false, String(err).split('\n')[0]);
      await disarmUnloadPrompt();
    }
  };

  const categories = (await getJson('/api/categories?isPagingEnabled=false')).items.map(c => c.name);
  const locations = (await getJson('/api/storage-locations?isPagingEnabled=false')).items.map(c => c.name);

  await runEditor('recipe categories', `${base}/recipes/new`, 'categories', categories);
  if (locations.length > 0) {
    await runEditor('grocery item storage locations', `${base}/grocery-items/new`, 'storageLocations', locations);
  }

  await disarmUnloadPrompt();
  out.push('DONE');
  await flush();
  return out.join('\n');
}
