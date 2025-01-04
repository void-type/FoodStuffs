import type { SaveRecipeRequest } from '@/api/data-contracts';
import type WorkingRecipeIngredient from './RecipeIngredientWorking';
import type WorkingRecipeShoppingItem from './RecipeShoppingItemWorking';

export default class WorkingRecipe implements SaveRecipeRequest {
  public id = 0;

  public name: string = '';

  public directions = '';

  public sides = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public ingredients: WorkingRecipeIngredient[] = [];

  public shoppingItems: WorkingRecipeShoppingItem[] = [];

  public categories: string[] = [];
}
