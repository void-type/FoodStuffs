import type { CategoriesListParams } from '@/api/data-contracts';
import Choices from '@/models/Choices';

export default class CategoriesListRequest implements CategoriesListParams {
  public name = '';

  public isUnused = null;

  public isPagingEnabled = true;

  public page = 1;

  public take = Choices.defaultPaginationTake.value;
}
