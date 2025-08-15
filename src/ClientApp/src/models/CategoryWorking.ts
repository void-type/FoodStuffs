import type { SaveCategoryRequest } from '@/api/data-contracts';

export default class CategoryWorking implements SaveCategoryRequest {
  public id = 0;

  public name: string = '';

  public showInMealPlan: boolean = false;

  public color: string = '#000000';
}
