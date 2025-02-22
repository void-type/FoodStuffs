import type {
  SaveMealPlanRequest,
  SaveMealPlanRequestExcludedShoppingItem,
  SaveMealPlanRequestRecipe,
} from '@/api/data-contracts';
import DateHelper from '@/models/DateHelper';

export default class MealPlanWorking implements SaveMealPlanRequest {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: SaveMealPlanRequestRecipe[] = [];

  public excludedShoppingItems: SaveMealPlanRequestExcludedShoppingItem[] = [];

  public static createForStore() {
    const newPlan = new MealPlanWorking();
    newPlan.name = DateHelper.dateForView(DateHelper.getThisOrNextDayOfWeek(1));
    return newPlan;
  }
}
