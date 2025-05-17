import type {
  MealPlansListParams,
  SaveMealPlanRequestExcludedGroceryItem,
} from '@/api/data-contracts';
import MealPlansListRequest from './MealPlansListRequest';
import { isNil } from './FormatHelper';

const settingsKeyCurrentMealPlanId = 'currentMealPlanId';

export function getCurrentMealPlanFromStorage() {
  return JSON.parse(localStorage.getItem(settingsKeyCurrentMealPlanId) || 'null') as number | null;
}

export function storeCurrentMealPlanInStorage(currentMealPlanId: number | null) {
  localStorage.setItem(settingsKeyCurrentMealPlanId, JSON.stringify(currentMealPlanId));
}

export function listRequestToQueryParams(listRequest: MealPlansListParams) {
  // Query params need to be string or number
  const requestEntries = Object.entries({
    ...listRequest,
  });

  const defaultEntries = Object.entries({
    ...new MealPlansListRequest(),
  });

  const cleanedEntries = requestEntries
    .filter(([rKey, rValue]) => {
      // Get the matching default value
      const dValue = defaultEntries.find(([dKey]) => dKey === rKey)?.[1];
      // Compare the values
      return JSON.stringify(rValue) !== JSON.stringify(dValue);
    })
    .map(([qpKey, qpValue]) => [qpKey, String(qpValue)]);

  return Object.fromEntries(cleanedEntries);
}

export function countGroceryItems(
  acc: SaveMealPlanRequestExcludedGroceryItem[],
  curr: SaveMealPlanRequestExcludedGroceryItem
) {
  const { id, quantity } = curr;

  if (isNil(id)) {
    return acc;
  }

  let match = acc.find((x) => x.id === id);

  if (!match) {
    match = { id, quantity: 0 };
    acc.push(match);
  }

  if (match.quantity === undefined) {
    match.quantity = 0;
  }

  match.quantity += quantity || 0;
  return acc;
}

export function addGroceryItem(
  groceryItems: SaveMealPlanRequestExcludedGroceryItem[],
  id: number,
  count = 1
) {
  let groceryItem = groceryItems.find((x) => x.id === id);

  if (!groceryItem) {
    groceryItem = { id, quantity: 0 };
    groceryItems.push(groceryItem);
  }

  if (groceryItem.quantity === undefined) {
    groceryItem.quantity = 0;
  }

  groceryItem.quantity += count;
}

export function subtractGroceryItem(
  groceryItems: SaveMealPlanRequestExcludedGroceryItem[],
  id: number,
  count = 1
) {
  const groceryItem = groceryItems.find((x) => x.id === id);

  if (!groceryItem) {
    return;
  }

  if (groceryItem.quantity === undefined) {
    groceryItem.quantity = 0;
  }

  groceryItem.quantity -= count;

  if (groceryItem.quantity < 1) {
    groceryItems.splice(groceryItems.indexOf(groceryItem), 1);
  }
}
