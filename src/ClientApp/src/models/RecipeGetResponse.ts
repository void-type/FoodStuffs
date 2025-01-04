import type {
  GetRecipeResponse,
  GetRecipeResponseIngredient,
  GetRecipeResponseShoppingItem,
} from '@/api/data-contracts';

export default class RecipeGetResponse implements GetRecipeResponse {
  public id = 0;

  public name: string = '';

  public directions: string = '';

  public prepTimeMinutes: number | null = null;

  public cookTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public pinnedImage: string | null = null;

  public images: string[] = [];

  public categories: string[] = [];

  public ingredients: GetRecipeResponseIngredient[] = [];

  public shoppingItems: GetRecipeResponseShoppingItem[] = [];
}
