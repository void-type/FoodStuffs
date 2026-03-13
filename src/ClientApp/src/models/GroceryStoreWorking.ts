import type { SaveGroceryStoreRequest } from '@/api/data-contracts';

export default class GroceryStoreWorking implements SaveGroceryStoreRequest {
  public id = 0;

  public name: string = '';
}
