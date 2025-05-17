import type { GroceryAislesListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class GroceryAislesListRequest implements GroceryAislesListParams {
  public name = '';

  public isUnused = null;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
