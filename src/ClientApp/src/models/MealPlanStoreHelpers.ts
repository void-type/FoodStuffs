const settingsKeyCurrentMealPlanId = 'currentMealPlanId';

export default class RecipeStoreHelpers {
  static getCurrentMealPlan() {
    return JSON.parse(localStorage.getItem(settingsKeyCurrentMealPlanId) || 'null') as
      | number
      | null;
  }

  static setCurrentMealPlan(currentMealPlanId: number | null) {
    localStorage.setItem(settingsKeyCurrentMealPlanId, JSON.stringify(currentMealPlanId));
  }
}
