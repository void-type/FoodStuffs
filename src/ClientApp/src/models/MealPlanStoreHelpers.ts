import type { MealPlansListParams } from '@/api/data-contracts';
import SearchMealPlansRequest from './SearchMealPlansRequest';

const settingsKeyCurrentMealPlanId = 'currentMealPlanId';

export default class RecipeStoreHelpers {
  static getCurrentMealPlan() {
    return JSON.parse(localStorage.getItem(settingsKeyCurrentMealPlanId) || 'null') as
      | number
      | null;
  }

  static storeCurrentMealPlan(currentMealPlanId: number | null) {
    localStorage.setItem(settingsKeyCurrentMealPlanId, JSON.stringify(currentMealPlanId));
  }

  static listRequestToQueryParams(listRequest: MealPlansListParams) {
    // Query params need to be string or number
    const requestEntries = Object.entries({
      ...listRequest,
    });

    const defaultEntries = Object.entries({
      ...new SearchMealPlansRequest(),
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
}
