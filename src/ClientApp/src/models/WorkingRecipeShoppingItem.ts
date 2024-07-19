import type { SaveRecipeRequestShoppingItem } from '@/api/data-contracts';

export default class WorkingRecipeShoppingItem implements SaveRecipeRequestShoppingItem {
  // Used so Vue can track in editor.
  public uiKey = crypto.randomUUID();

  public id = 0;

  public quantity?: number = 1;

  public order?: number = Number.MAX_SAFE_INTEGER;

  public isCategory?: boolean = false;
}
