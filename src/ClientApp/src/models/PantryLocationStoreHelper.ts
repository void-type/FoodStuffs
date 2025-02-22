import type { PantryLocationsListParams } from '@/api/data-contracts';
import PantryLocationsListRequest from './PantryLocationsListRequest';

export default function listRequestToQueryParams(listRequest: PantryLocationsListParams) {
  // Query params need to be string or number
  const requestEntries = Object.entries({
    ...listRequest,
  });

  const defaultEntries = Object.entries({
    ...new PantryLocationsListRequest(),
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
