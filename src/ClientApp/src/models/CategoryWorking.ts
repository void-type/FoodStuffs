import type { SaveCategoryRequest } from '@/api/data-contracts';
import { DEFAULT_TAG_COLOR } from '@/models/Tag';

export default class CategoryWorking implements SaveCategoryRequest {
  public id = 0;

  public name: string = '';

  public showOnCards: boolean = false;

  public color: string = DEFAULT_TAG_COLOR;
}
