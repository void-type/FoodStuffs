import type { GroceryItemsSearchParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class GroceryItemsSearchRequest implements GroceryItemsSearchParams {
  public searchText = '';

  public storageLocations = [] as number[];

  public matchAllStorageLocations = false;

  public groceryAisles = [] as number[];

  public isOutOfStock = null;

  public isUnused = null;

  public sortBy = Choices.sortOptions[0]!.value;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
