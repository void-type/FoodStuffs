import type { SaveRecipeRequest } from '@/api/data-contracts';
import type RecipeShoppingItemWorking from './RecipeShoppingItemWorking';

export default class RecipeWorking implements SaveRecipeRequest {
  public id = 0;

  public name: string = '';

  public directions = '';

  public sides = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public shoppingItems: RecipeShoppingItemWorking[] = [];

  public categories: string[] = [];
}
