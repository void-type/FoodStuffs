import type { SaveShoppingItemRequest } from '@/api/data-contracts';

export default class ShoppingItemWorking implements SaveShoppingItemRequest {
  public id = 0;

  public name: string = '';
}
