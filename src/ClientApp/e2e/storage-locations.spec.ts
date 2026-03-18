import { expect, test } from '@playwright/test';
import { TEST_MARKER } from './constants';

test.describe.serial('Storage Locations CRUD', () => {
  const testName = `Test Location ${TEST_MARKER}`;
  const updatedName = `${testName} Updated`;

  test('create a new storage location', async ({ page }) => {
    await page.goto('/storage-locations/new');
    await page.getByTestId('field-name').fill(testName);
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
    await expect(page).toHaveURL(/\/storage-locations\/\d+/);
  });

  test('verify storage location appears in list', async ({ page }) => {
    await page.goto('/storage-locations');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText(testName)).toBeVisible();
  });

  test('update the storage location', async ({ page }) => {
    await page.goto('/storage-locations');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await page.getByText(testName).click();
    await page.getByTestId('field-name').clear();
    await page.getByTestId('field-name').fill(updatedName);
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
  });

  test('verify updated storage location in list', async ({ page }) => {
    await page.goto('/storage-locations');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText(updatedName)).toBeVisible();
  });

  test('delete the storage location', async ({ page }) => {
    await page.goto('/storage-locations');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await page.getByText(updatedName).click();
    await page.getByTestId('more-button').click();
    await page.getByTestId('delete-button').click();
    await page.getByTestId('modal-ok').click();
    await expect(page).toHaveURL(/\/storage-locations/);
  });

  test('verify storage location is deleted', async ({ page }) => {
    await page.goto('/storage-locations');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText('Found no storage locations.')).toBeVisible();
  });
});
