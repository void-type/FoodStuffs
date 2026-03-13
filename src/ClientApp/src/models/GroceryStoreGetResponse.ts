import type {
  GetGroceryStoreResponse,
  GetGroceryStoreResponseGroceryItem,
} from '@/api/data-contracts';

export default class GroceryStoreGetResponse implements GetGroceryStoreResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public groceryItems: GetGroceryStoreResponseGroceryItem[] = [];
}
