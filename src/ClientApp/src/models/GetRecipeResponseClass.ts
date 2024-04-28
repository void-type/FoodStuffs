import type { GetRecipeResponse, GetRecipeResponseIngredient } from '@/api/data-contracts';

export default class GetRecipeResponseClass implements GetRecipeResponse {
  public id = 0;

  public name: string = '';

  public directions: string = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public pinnedImage: string | null = null;

  public isForMealPlanning = false;

  public categories: string[] = [];

  public images: string[] = [];

  public ingredients: GetRecipeResponseIngredient[] = [];
}
