import type { SaveRecipeRequest } from '@/api/data-contracts';
import type WorkingRecipeIngredient from './WorkingRecipeIngredient';
import type WorkingRecipeShoppingItem from './WorkingRecipeShoppingItem';

export default class WorkingRecipe implements SaveRecipeRequest {
  public id = 0;

  public name: string = '';

  public directions = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public ingredients: WorkingRecipeIngredient[] = [];

  public shoppingItems: WorkingRecipeShoppingItem[] = [];

  public categories: string[] = [];
}
