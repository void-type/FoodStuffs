import type {
  GetMealPlanResponse,
  GetMealPlanResponsePantryIngredient,
  GetRecipeResponse,
} from '@/api/data-contracts';

export default class GetMealPlanResponseClass implements GetMealPlanResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetRecipeResponse[] = [];

  public pantryIngredients: GetMealPlanResponsePantryIngredient[] = [];
}
