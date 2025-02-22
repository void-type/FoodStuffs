import type {
  MealPlansListParams,
  SaveMealPlanRequestExcludedShoppingItem,
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

export function countShoppingItems(
  acc: SaveMealPlanRequestExcludedShoppingItem[],
  curr: SaveMealPlanRequestExcludedShoppingItem
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

export function addShoppingItem(
  shoppingItems: SaveMealPlanRequestExcludedShoppingItem[],
  id: number,
  count = 1
) {
  let shoppingItem = shoppingItems.find((x) => x.id === id);

  if (!shoppingItem) {
    shoppingItem = { id, quantity: 0 };
    shoppingItems.push(shoppingItem);
  }

  if (shoppingItem.quantity === undefined) {
    shoppingItem.quantity = 0;
  }

  shoppingItem.quantity += count;
}

export function subtractShoppingItem(
  shoppingItems: SaveMealPlanRequestExcludedShoppingItem[],
  id: number,
  count = 1
) {
  const shoppingItem = shoppingItems.find((x) => x.id === id);

  if (!shoppingItem) {
    return;
  }

  if (shoppingItem.quantity === undefined) {
    shoppingItem.quantity = 0;
  }

  shoppingItem.quantity -= count;

  if (shoppingItem.quantity < 1) {
    shoppingItems.splice(shoppingItems.indexOf(shoppingItem), 1);
  }
}
