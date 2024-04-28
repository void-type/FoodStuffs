import type {
  GetMealSetResponse,
  GetMealSetResponsePantryIngredient,
  GetRecipeResponse,
} from '@/api/data-contracts';

export default class GetMealSetResponseClass implements GetMealSetResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetRecipeResponse[] = [];

  public pantryIngredients: GetMealSetResponsePantryIngredient[] = [];
}
