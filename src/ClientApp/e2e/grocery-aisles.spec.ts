import { expect, test } from '@playwright/test';
import { TEST_MARKER } from './constants';

test.describe.serial('Grocery Aisles CRUD', () => {
  const testName = `Test Aisle ${TEST_MARKER}`;
  const updatedName = `${testName} Updated`;

  test('create a new grocery aisle', async ({ page }) => {
    await page.goto('/grocery-aisles/new');
    await page.getByTestId('field-name').fill(testName);
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
    await expect(page).toHaveURL(/\/grocery-aisles\/\d+/);
  });

  test('verify grocery aisle appears in list', async ({ page }) => {
    await page.goto('/grocery-aisles');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText(testName)).toBeVisible();
  });

  test('update the grocery aisle', async ({ page }) => {
    await page.goto('/grocery-aisles');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await page.getByText(testName).click();
    await page.getByTestId('field-name').clear();
    await page.getByTestId('field-name').fill(updatedName);
    await page.getByTestId('number-increment-order').click();
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
  });

  test('verify updated grocery aisle in list', async ({ page }) => {
    await page.goto('/grocery-aisles');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText(updatedName)).toBeVisible();
  });

  test('delete the grocery aisle', async ({ page }) => {
    await page.goto('/grocery-aisles');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await page.getByText(updatedName).click();
    await page.getByTestId('more-button').click();
    await page.getByTestId('delete-button').click();
    await page.getByTestId('modal-ok').click();
    await expect(page).toHaveURL(/\/grocery-aisles/);
  });

  test('verify grocery aisle is deleted', async ({ page }) => {
    await page.goto('/grocery-aisles');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText('Found no grocery aisles.')).toBeVisible();
  });
});
