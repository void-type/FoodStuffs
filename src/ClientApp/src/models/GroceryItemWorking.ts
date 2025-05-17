import type { SaveGroceryItemRequest } from '@/api/data-contracts';

export default class GroceryItemWorking implements SaveGroceryItemRequest {
  public id = 0;

  public name: string = '';

  public inventoryQuantity: number = 0;

  public groceryAisleId: number | undefined | null = null;

  public pantryLocations: string[] = [];
}
