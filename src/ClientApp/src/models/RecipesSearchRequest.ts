import type { RecipesSearchParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class RecipesSearchRequest implements RecipesSearchParams {
  public searchText = '';

  public categories = [] as number[];

  public matchAllCategories = false;

  public isForMealPlanning = null;

  public sortBy = Choices.sortOptions[0]!.value;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
