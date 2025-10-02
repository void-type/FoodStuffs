import type { SaveRecipeRequestGroceryItem } from '@/api/data-contracts';

export default class RecipeGroceryItemWorking implements SaveRecipeRequestGroceryItem {
  // Used so Vue can track in editor.
  public uiKey = crypto.randomUUID();

  public id?: number = 0;

  public quantity?: number = 1;

  public order?: number = Number.MAX_SAFE_INTEGER;
}
