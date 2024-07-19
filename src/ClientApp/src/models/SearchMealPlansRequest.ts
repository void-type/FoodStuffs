import type { MealPlansListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class SearchMealPlansRequest implements MealPlansListParams {
  public name = '';

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
