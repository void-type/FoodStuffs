import type { SaveGroceryAisleRequest } from '@/api/data-contracts';

export default class GroceryAisleWorking implements SaveGroceryAisleRequest {
  public id = 0;

  public name: string = '';

  public order?: number = Number.MAX_SAFE_INTEGER;
}
