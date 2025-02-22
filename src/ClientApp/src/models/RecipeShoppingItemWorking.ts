import type { SaveRecipeRequestShoppingItem } from '@/api/data-contracts';

export default class RecipeShoppingItemWorking implements SaveRecipeRequestShoppingItem {
  // Used so Vue can track in editor.
  public uiKey = crypto.randomUUID();

  public id?: number;

  public quantity?: number = 1;

  public inventoryQuantity?: number = 0;

  public order?: number = Number.MAX_SAFE_INTEGER;
}
