import type { SaveRecipeRequest } from '@/api/data-contracts';
import type WorkingRecipeIngredient from './WorkingRecipeIngredient';

export default class WorkingRecipe implements SaveRecipeRequest {
  public id = 0;

  public name: string | null = '';

  public directions = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public ingredients: WorkingRecipeIngredient[] = [];

  public categories: string[] | null = [];
}
