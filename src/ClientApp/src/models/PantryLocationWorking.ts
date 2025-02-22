import type { SavePantryLocationRequest } from '@/api/data-contracts';

export default class PantryLocationWorking implements SavePantryLocationRequest {
  public id = 0;

  public name: string = '';
}
