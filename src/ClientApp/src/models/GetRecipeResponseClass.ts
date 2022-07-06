import type { GetRecipeResponse, GetRecipeResponseIngredient } from '@/api/data-contracts';

export default class GetRecipeResponseClass implements GetRecipeResponse {
  public id = 0;

  public name: string | null = '';

  public directions: string | null = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public createdBy: string | null = '';

  public createdOn = '';

  public modifiedBy: string | null = '';

  public modifiedOn = '';

  public pinnedImageId: number | null = null;

  public isForMealPlanning = false;

  public categories: string[] | null = [];

  public images: number[] | null = [];

  public ingredients: GetRecipeResponseIngredient[] | null = [];
}
