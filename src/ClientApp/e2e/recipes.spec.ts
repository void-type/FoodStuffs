import { expect, test } from '@playwright/test';
import { TEST_MARKER } from './constants';

test.describe.serial('Recipes CRUD', () => {
  const testName = `Test Recipe ${TEST_MARKER}`;
  const updatedName = `${testName} Updated`;

  test('create a new recipe', async ({ page }) => {
    await page.goto('/recipes/new');
    await page.getByTestId('field-name').fill(testName);
    await page.getByTestId('field-sides').fill('Test side dish');
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
    await expect(page).toHaveURL(/\/recipes\/\d+\/edit/);
  });

  test('verify recipe appears in list', async ({ page }) => {
    await page.goto('/recipes');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText(testName)).toBeVisible();
  });

  test('update the recipe', async ({ page }) => {
    await page.goto('/recipes');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await page.getByText(testName).click();
    // Navigate from view to edit
    await page.getByRole('link', { name: 'Edit' }).click();
    await page.getByTestId('field-name').clear();
    await page.getByTestId('field-name').fill(updatedName);
    await page.getByTestId('field-sides').fill('Updated side dish');
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
  });

  test('verify updated recipe in list', async ({ page }) => {
    await page.goto('/recipes');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText(updatedName)).toBeVisible();
  });

  test('delete the recipe', async ({ page }) => {
    await page.goto('/recipes');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await page.getByText(updatedName).click();
    await page.getByRole('link', { name: 'Edit' }).click();
    await page.getByTestId('more-button').click();
    await page.getByTestId('delete-button').click();
    await page.getByTestId('modal-ok').click();
    await expect(page).toHaveURL(/\/recipes/);
  });

  test('verify recipe is deleted', async ({ page }) => {
    await page.goto('/recipes');
    await page.getByTestId('search-name').fill(TEST_MARKER);
    await page.getByTestId('search-button').click();
    await expect(page.getByText('Found no recipes.')).toBeVisible();
  });
});
