import type { ShoppingItemsListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class ShoppingItemsListRequest implements ShoppingItemsListParams {
  public name = '';

  public isUnused = null;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
