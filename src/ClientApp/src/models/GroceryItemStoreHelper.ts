import type { GroceryItemsSearchParams } from '@/api/data-contracts';
import GroceryItemsSearchRequest from './GroceryItemsSearchRequest';

export default class GroceryItemStoreHelper {
  static listRequestToQueryParams(listRequest: GroceryItemsSearchParams) {
    // Query params need to be string or number
    const requestEntries = Object.entries({
      ...listRequest,
      storageLocations: listRequest.storageLocations?.join() || '',
      groceryAisles: listRequest.groceryAisles?.join() || '',
    });

    const defaultEntries = Object.entries({
      ...new GroceryItemsSearchRequest(),
      storageLocations: new GroceryItemsSearchRequest().storageLocations?.join() || '',
      groceryAisles: new GroceryItemsSearchRequest().groceryAisles?.join() || '',
    });

    const cleanedEntries = requestEntries
      .filter(([rKey, rValue]) => {
        // Get the matching default value
        const dValue = defaultEntries.find(([dKey]) => dKey === rKey)?.[1];
        // Compare the values
        return JSON.stringify(rValue) !== JSON.stringify(dValue);
      })
      .map(([qpKey, qpValue]) => [qpKey, String(qpValue)]);

    return Object.fromEntries(cleanedEntries);
  }
}
