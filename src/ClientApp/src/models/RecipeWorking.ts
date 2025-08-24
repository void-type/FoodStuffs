import type { GetRecipeResponseCategory } from '@/api/data-contracts';
import type RecipeGroceryItemWorking from './RecipeGroceryItemWorking';

export default class RecipeWorking {
  public id = 0;

  public name: string = '';

  public directions = '';

  public sides = '';

  public cookTimeMinutes: number | null = null;

  public prepTimeMinutes: number | null = null;

  public isForMealPlanning = false;

  public mealPlanningSidesCount = 0;

  public groceryItems: RecipeGroceryItemWorking[] = [];

  public categories: GetRecipeResponseCategory[] = [];
}
