import type {
  GetMealPlanResponse,
  GetMealPlanResponsePantryIngredient,
  GetRecipeResponse,
} from '@/api/data-contracts';
import DateHelpers from '@/models/DateHelpers';

export default class GetMealPlanResponseClass implements GetMealPlanResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetRecipeResponse[] = [];

  public pantryIngredients: GetMealPlanResponsePantryIngredient[] = [];

  public static createForStore() {
    const newPlan = new GetMealPlanResponseClass();
    newPlan.name = DateHelpers.dateForView(DateHelpers.getThisOrNextDayOfWeek(1));
    return newPlan;
  }
}
