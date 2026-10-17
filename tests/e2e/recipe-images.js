// Regression suite: recipe image upload, pin, delete and carousel behavior on the recipe edit page.
// Run with tests/e2e/run.ps1 (see README.md). Executed by `playwright-cli run-code`, so `page` is a Playwright Page.
async (page) => {
  page.on('dialog', d => d.accept());
  page.setDefaultTimeout(12000);

  const base = new URL(page.url()).origin;
  const stamp = `zz-e2e-${Date.now()}`;
  const out = [];
  const check = (name, ok, detail = '') => {
    out.push(`${ok ? 'PASS' : 'FAIL'} ${name}${detail ? ` -- ${detail}` : ''}`);
    flush();
  };
  // The CLI can return before the script finishes, so results are persisted where the runner can poll them.
  const flush = () => page.evaluate(o => localStorage.setItem('e2eResults', o), out.join('\n')).catch(() => {});
  const nav = 'a.nav-link[href="/meal-plans"]';

  const state = () => page.evaluate(() => {
    const items = [...document.querySelectorAll('.carousel-item')];
    const active = items.findIndex(e => e.classList.contains('active'));
    return {
      count: items.length,
      active,
      pinBtnOnActive: !!items[active]?.querySelector('button[title="Pin image"]'),
      pinBtns: document.querySelectorAll('.carousel-item button[title="Pin image"]').length,
      uploading: /Uploading \d+ image/.test(document.body.textContent || ''),
    };
  });
  // Compare only the keys in `expected`.
  const expectState = async (name, expected) => {
    const actual = await state();
    const bad = Object.keys(expected).filter(key => actual[key] !== expected[key]);
    check(name, bad.length === 0, bad.length ? `expected ${JSON.stringify(expected)} got ${JSON.stringify(actual)}` : '');
  };

  const png = color => page.evaluate(c => {
    const cv = document.createElement('canvas');
    cv.width = 64;
    cv.height = 48;
    const ctx = cv.getContext('2d');
    ctx.fillStyle = c;
    ctx.fillRect(0, 0, 64, 48);
    return cv.toDataURL('image/png').split(',')[1];
  }, color);
  const file = async (name, color) => ({ name, mimeType: 'image/png', buffer: Buffer.from(await png(color), 'base64') });
  // Wait for the expected image count and for the uploading indicator to clear.
  const settle = n => page.waitForFunction(
    c => document.querySelectorAll('.carousel-item').length === c && !/Uploading \d+ image/.test(document.body.textContent || ''),
    n,
    { timeout: 10000 },
  ).catch(() => {});
  const modalShown = () => page.waitForSelector('.modal.show').then(() => true).catch(() => false);

  let created = false;
  try {
    await page.goto(`${base}/recipes/new`);
    await page.waitForSelector('#name');
    await page.fill('#name', stamp);
    await page.click('button:has-text("Save")');
    await page.waitForURL(u => /\/recipes\/\d+\//.test(u.toString()) && u.toString().endsWith('/edit'));
    await page.waitForSelector('#upload-file', { state: 'attached' });
    created = true;
    await expectState('no images initially', { count: 0, active: -1 });

    await page.setInputFiles('#upload-file', await file('a.png', '#ff0000'));
    await settle(1);
    await expectState('upload one', { count: 1, active: 0, pinBtnOnActive: true, uploading: false });

    await page.setInputFiles('#upload-file', await file('b.png', '#00ff00'));
    await settle(2);
    await expectState('second upload becomes active', { count: 2, active: 1, uploading: false });

    // Which of two parallel uploads finishes last is a race, so the active slide isn't asserted.
    await page.setInputFiles('#upload-file', [await file('c.png', '#0000ff'), await file('d.png', '#ffff00')]);
    await settle(4);
    await expectState('multi upload', { count: 4, uploading: false });

    await page.setInputFiles('#upload-file', { name: 'x.txt', mimeType: 'text/plain', buffer: Buffer.from('nope') });
    await page.waitForTimeout(800);
    check('non-image shows an error', await page.evaluate(() => /not an image and was skipped/.test(document.body.textContent || '')));
    await expectState('non-image is not added', { count: 4, uploading: false });

    await page.click('.carousel-item.active button[title="Pin image"]');
    await page.waitForTimeout(1200);
    await expectState('pin moves the pinned image first', { count: 4, active: 0, pinBtnOnActive: false, pinBtns: 3 });

    await page.click('.carousel-control-prev');
    await page.waitForTimeout(900);
    await expectState('carousel prev wraps to last', { count: 4, active: 3 });

    await page.click('.carousel-item.active button[title="Delete image"]');
    check('delete asks for confirmation', await modalShown());
    await page.click('.modal.show .btn-primary');
    await settle(3);
    await expectState('delete shows the previous image', { count: 3, active: 2 });

    await page.click('.carousel-control-next');
    await page.waitForTimeout(900);
    await page.click('.carousel-item.active button[title="Delete image"]');
    await modalShown();
    await page.click('.modal.show .btn-primary');
    await settle(2);
    await expectState('deleting the first image stays on the first', { count: 2, active: 0 });

    // The carousel must remember the slide the user moved to, so refreshing the recipe doesn't jump back.
    await page.click('.carousel-control-next');
    await page.waitForTimeout(900);
    await expectState('carousel next moves to the second image', { count: 2, active: 1 });
    await page.fill('#name', `${stamp} saved`);
    await page.click('button:has-text("Save")');
    await page.waitForFunction(() => document.querySelector('#name').value.toLowerCase().endsWith('saved'));
    await page.waitForTimeout(1200);
    await expectState('saving keeps images and the current slide', { count: 2, active: 1 });

    // A failed upload must clear the uploading indicator.
    await page.route('**/api/images**', r => (r.request().method() === 'POST'
      ? r.fulfill({ status: 400, contentType: 'application/json', body: '{"items":[{"message":"stub failure","uiHandle":"upload"}],"count":1,"isPagingEnabled":false,"page":1,"take":1,"totalCount":1}' })
      : r.continue()));
    await page.setInputFiles('#upload-file', await file('e.png', '#00ffff'));
    await page.waitForTimeout(1500);
    await page.unroute('**/api/images**');
    await expectState('failed upload clears the indicator and adds nothing', { count: 2, uploading: false });

    await page.click(nav);
    await page.waitForURL('**/meal-plans');
    await page.goBack();
    await page.waitForSelector('#upload-file', { state: 'attached' });
    await page.waitForTimeout(1200);
    await expectState('returning to the recipe starts on the first image', { count: 2, active: 0 });
  } catch (err) {
    check('script completed', false, String(err).split('\n')[0]);
    await page.evaluate(() => window.addEventListener('beforeunload', ev => ev.stopImmediatePropagation(), true)).catch(() => {});
  }

  // Delete the test recipe.
  try {
    if (created && /\/recipes\/\d+\//.test(page.url())) {
      await page.click('#overflowMenuButton');
      await page.click('.dropdown-menu .dropdown-item.text-danger');
      await modalShown();
      await page.click('.modal.show .btn-primary');
      await page.waitForURL(u => !/\/recipes\/\d+\//.test(u.toString()));
      check('test recipe deleted', true);
    } else if (created) {
      check('test recipe deleted', false, `not on the recipe page; delete the recipe named "${stamp}" by hand`);
    }
  } catch (err) {
    check('test recipe deleted', false, `${String(err).split('\n')[0]} (delete the recipe named "${stamp}" by hand)`);
  }

  out.push('DONE');
  await flush();
  return out.join('\n');
}
