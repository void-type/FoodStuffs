import type {
  GetShoppingItemResponse,
  GetShoppingItemResponseGroceryDepartment,
  GetShoppingItemResponseRecipe,
} from '@/api/data-contracts';

export default class ShoppingItemGetResponse implements GetShoppingItemResponse {
  public id = 0;

  public name: string = '';

  public inventoryQuantity: number = 0;

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetShoppingItemResponseRecipe[] = [];

  public groceryDepartment: GetShoppingItemResponseGroceryDepartment | null = null;

  public pantryLocations: string[] = [];
}
