import type { CategoriesListParams } from '@/api/data-contracts';
import CategoriesListRequest from './CategoriesListRequest';

export default class CategoryStoreHelper {
  static listRequestToQueryParams(listRequest: CategoriesListParams) {
    // Query params need to be string or number
    const requestEntries = Object.entries({
      ...listRequest,
    });

    const defaultEntries = Object.entries({
      ...new CategoriesListRequest(),
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
