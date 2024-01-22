import type { RecipesListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';
import RecipeStoreHelpers from './RecipeStoreHelpers';

export default class SearchRecipesRequest implements RecipesListParams {
  public name = '';

  public categories = [];

  public isForMealPlanning = null;

  public sortBy = RecipeStoreHelpers.sortOptions[0].value;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
