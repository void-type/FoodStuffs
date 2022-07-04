import type { SaveRecipeRequest, SaveRecipeRequestIngredient } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class SaveRecipeRequestClass implements SaveRecipeRequest {
  public id = 0;

  public name = '';

  public directions = '';

  public cookTimeMinutes = null;

  public prepTimeMinutes = null;

  public isForMealPlanning = false;

  public ingredients: SaveRecipeRequestIngredient[] = [];

  public categories: string[] = [];
}
