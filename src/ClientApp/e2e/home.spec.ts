import { expect, test } from '@playwright/test';

test('main content area is displayed', async ({ page }) => {
  await page.goto('/');
  const main = page.locator('main#main');
  await expect(main).toBeVisible();
});
