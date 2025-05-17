import type {
  GetGroceryItemResponse,
  GetGroceryItemResponseGroceryDepartment,
  GetGroceryItemResponseRecipe,
} from '@/api/data-contracts';

export default class GroceryItemGetResponse implements GetGroceryItemResponse {
  public id = 0;

  public name: string = '';

  public inventoryQuantity: number = 0;

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetGroceryItemResponseRecipe[] = [];

  public groceryDepartment: GetGroceryItemResponseGroceryDepartment | null = null;

  public pantryLocations: string[] = [];
}
