import { expect, test } from '@playwright/test';
import { TEST_MARKER } from './constants';

test.describe.serial('Meal Plans CRUD', () => {
  const testName = `Test Plan ${TEST_MARKER}`;
  const updatedName = `${testName} Updated`;

  test('create a new meal plan', async ({ page }) => {
    await page.goto('/meal-plans/new');
    await page.getByTestId('field-name').fill(testName);
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
    await expect(page).toHaveURL(/\/meal-plans\/\d+/);
  });

  test('verify meal plan appears in list', async ({ page }) => {
    await page.goto('/meal-plans');
    await expect(page.getByText(testName)).toBeVisible();
  });

  test('update the meal plan', async ({ page }) => {
    await page.goto('/meal-plans');
    await page.getByText(testName).click();
    await page.getByTestId('field-name').clear();
    await page.getByTestId('field-name').fill(updatedName);
    await page.getByTestId('save-button').click();
    await expect(page.locator('[data-test-id="message-alert"][data-test-message-type="success"]')).toBeVisible();
  });

  test('verify updated meal plan in list', async ({ page }) => {
    await page.goto('/meal-plans');
    await expect(page.getByText(updatedName)).toBeVisible();
  });

  test('delete the meal plan', async ({ page }) => {
    await page.goto('/meal-plans');
    await page.getByText(updatedName).click();
    await page.getByTestId('more-button').click();
    await page.getByTestId('delete-button').click();
    await page.getByTestId('modal-ok').click();
    await expect(page).toHaveURL(/\/meal-plans/);
  });

  test('verify meal plan is deleted', async ({ page }) => {
    await page.goto('/meal-plans');
    await expect(page.getByText(updatedName)).not.toBeVisible();
  });
});
