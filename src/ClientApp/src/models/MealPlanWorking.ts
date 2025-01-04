import type {
  SaveMealPlanRequest,
  SaveMealPlanRequestPantryShoppingItem,
  SaveMealPlanRequestRecipe,
} from '@/api/data-contracts';
import DateHelper from '@/models/DateHelper';

export default class WorkingMealPlan implements SaveMealPlanRequest {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: SaveMealPlanRequestRecipe[] = [];

  public pantryShoppingItems: SaveMealPlanRequestPantryShoppingItem[] = [];

  public static createForStore() {
    const newPlan = new WorkingMealPlan();
    newPlan.name = DateHelper.dateForView(DateHelper.getThisOrNextDayOfWeek(1));
    return newPlan;
  }
}
