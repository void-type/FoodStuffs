import type { SaveRecipeRequestIngredient } from '@/api/data-contracts';

export default class WorkingRecipeIngredient implements SaveRecipeRequestIngredient {
  // Used so Vue can track in editor.
  public uiKey = crypto.randomUUID();

  public name?: string = '';

  public quantity?: number = 1;

  public order?: number = Number.MAX_SAFE_INTEGER;

  public isCategory?: boolean = false;
}
