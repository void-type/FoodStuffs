import type { GetShoppingItemResponse, GetShoppingItemResponseRecipe } from '@/api/data-contracts';

export default class ShoppingItemGetResponse implements GetShoppingItemResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetShoppingItemResponseRecipe[] = [];
}
