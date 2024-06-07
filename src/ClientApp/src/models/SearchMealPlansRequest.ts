import type { MealPlansSearchParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class SearchMealPlansRequest implements MealPlansSearchParams {
  public name = '';

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
