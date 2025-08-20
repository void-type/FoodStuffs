import type { RecipesSearchParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';
import RecipeStoreHelper from './RecipeStoreHelper';

export default class RecipesListRequest implements RecipesSearchParams {
  public searchText = '';

  public categories = [] as number[];

  public allCategories = false;

  public isForMealPlanning = null;

  public sortBy = RecipeStoreHelper.sortOptions[0]!.value;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
