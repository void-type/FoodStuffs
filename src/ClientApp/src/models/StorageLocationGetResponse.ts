import type {
  GetStorageLocationResponse,
  GetStorageLocationResponseGroceryItem,
} from '@/api/data-contracts';

export default class StorageLocationGetResponse implements GetStorageLocationResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetStorageLocationResponseGroceryItem[] = [];
}
