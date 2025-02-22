import type {
  GetGroceryDepartmentResponse,
  GetGroceryDepartmentResponseShoppingItem,
} from '@/api/data-contracts';

export default class GroceryDepartmentGetResponse implements GetGroceryDepartmentResponse {
  public id = 0;

  public name: string = '';

  public order: number = 0;

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetGroceryDepartmentResponseShoppingItem[] = [];
}
