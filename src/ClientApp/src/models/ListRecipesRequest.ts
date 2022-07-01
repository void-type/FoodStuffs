import type { RecipesListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class ListRecipesRequest implements RecipesListParams {
  public name = '';

  public category = '';

  public isForMealPlanning = false;

  public sortBy = '';

  public sortDesc = false;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.paginationTake[0].value;
}
