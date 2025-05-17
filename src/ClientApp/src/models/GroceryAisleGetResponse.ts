import type {
  GetGroceryAisleResponse,
  GetGroceryAisleResponseGroceryItem,
} from '@/api/data-contracts';

export default class GroceryAisleGetResponse implements GetGroceryAisleResponse {
  public id = 0;

  public name: string = '';

  public order: number = 0;

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetGroceryAisleResponseGroceryItem[] = [];
}
