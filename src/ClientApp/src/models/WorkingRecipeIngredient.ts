import type { SaveRecipeRequestIngredient } from '@/api/data-contracts';

export default class WorkingRecipeIngredient implements SaveRecipeRequestIngredient {
  public id = crypto.randomUUID();

  public name?: string = '';

  /** @format int32 */
  public quantity?: number = 1;

  /** @format int32 */
  public order?: number = Number.MAX_SAFE_INTEGER;

  public isCategory?: boolean = false;
}
