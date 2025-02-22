import type { SaveGroceryDepartmentRequest } from '@/api/data-contracts';

export default class GroceryDepartmentWorking implements SaveGroceryDepartmentRequest {
  public id = 0;

  public name: string = '';

  public order?: number = Number.MAX_SAFE_INTEGER;
}
