import type { SaveStorageLocationRequest } from '@/api/data-contracts';

export default class StorageLocationWorking implements SaveStorageLocationRequest {
  public id = 0;

  public name: string = '';
}
