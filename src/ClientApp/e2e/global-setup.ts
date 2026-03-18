import type { APIRequestContext } from '@playwright/test';
import { chromium } from '@playwright/test';
import { TEST_MARKER } from './constants';

const BASE_URL = 'https://localhost:5001';

async function deleteAll(
  request: APIRequestContext,
  ids: number[],
  path: string,
  headers: Record<string, string>,
) {
  for (const id of ids) {
    await request.delete(`${BASE_URL}${path}/${id}`, { headers });
  }
}

export default async function globalSetup() {
  const browser = await chromium.launch();

  try {
    const context = await browser.newContext({ ignoreHTTPSErrors: true });
    const page = await context.newPage();

    // eslint-disable-next-line no-console
    console.log('Cleaning up test data...');

    // Navigate to the app so the browser session and CSRF cookie are established.
    await page.goto(BASE_URL);

    const appInfoRes = await page.request.get(`${BASE_URL}/api/app/info`);
    const appInfo = await appInfoRes.json();
    const csrfHeaders: Record<string, string> = {
      [appInfo.antiforgeryTokenHeaderName]: appInfo.antiforgeryToken,
    };

    interface NamedEntity { id: number; name?: string }
    const hasMarker = (x: NamedEntity) => x.name?.toLowerCase().includes(TEST_MARKER.toLowerCase()) ?? false;

    // Categories
    const categoriesRes = await page.request.get(`${BASE_URL}/api/categories`, {
      params: { name: TEST_MARKER, isPagingEnabled: false },
    });
    const categories = await categoriesRes.json();
    await deleteAll(page.request, (categories.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/categories', csrfHeaders);

    // Grocery Aisles
    const aislesRes = await page.request.get(`${BASE_URL}/api/grocery-aisles`, {
      params: { name: TEST_MARKER, isPagingEnabled: false },
    });
    const aisles = await aislesRes.json();
    await deleteAll(page.request, (aisles.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/grocery-aisles', csrfHeaders);

    // Grocery Stores
    const storesRes = await page.request.get(`${BASE_URL}/api/grocery-stores`, {
      params: { name: TEST_MARKER, isPagingEnabled: false },
    });
    const stores = await storesRes.json();
    await deleteAll(page.request, (stores.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/grocery-stores', csrfHeaders);

    // Storage Locations
    const locationsRes = await page.request.get(`${BASE_URL}/api/storage-locations`, {
      params: { name: TEST_MARKER, isPagingEnabled: false },
    });
    const locations = await locationsRes.json();
    await deleteAll(page.request, (locations.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/storage-locations', csrfHeaders);

    // Grocery Items
    const groceryItemsRes = await page.request.get(`${BASE_URL}/api/grocery-items`, {
      params: { searchText: TEST_MARKER, isPagingEnabled: false },
    });
    const groceryItems = await groceryItemsRes.json();
    await deleteAll(page.request, (groceryItems.results?.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/grocery-items', csrfHeaders);

    // Recipes
    const recipesRes = await page.request.get(`${BASE_URL}/api/recipes`, {
      params: { searchText: TEST_MARKER, isPagingEnabled: false },
    });
    const recipes = await recipesRes.json();
    await deleteAll(page.request, (recipes.results?.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/recipes', csrfHeaders);

    // Meal Plans (no name filter — fetch all and filter client-side)
    const mealPlansRes = await page.request.get(`${BASE_URL}/api/meal-plans`, {
      params: { isPagingEnabled: false },
    });
    const mealPlans = await mealPlansRes.json();
    await deleteAll(page.request, (mealPlans.items ?? []).filter(hasMarker).map((x: NamedEntity) => x.id), '/api/meal-plans', csrfHeaders);
  } catch (error) {
    // eslint-disable-next-line no-console
    console.log('Error during global setup:', error);
  } finally {
    await browser.close();
  }
}
