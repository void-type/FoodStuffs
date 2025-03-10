import type { MealPlansListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class MealPlansListRequest implements MealPlansListParams {
  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
