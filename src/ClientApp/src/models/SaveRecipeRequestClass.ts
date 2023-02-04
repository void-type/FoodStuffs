import type { SaveRecipeRequest, SaveRecipeRequestIngredient } from '@/api/data-contracts';

export default class SaveRecipeRequestClass implements SaveRecipeRequest {
  public id = 0;

  public name: string | null = '';

  public directions = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public ingredients: SaveRecipeRequestIngredient[] = [];

  public categories: string[] | null = [];
}
