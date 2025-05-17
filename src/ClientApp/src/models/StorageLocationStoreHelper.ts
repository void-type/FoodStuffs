import type { StorageLocationsListParams } from '@/api/data-contracts';
import StorageLocationsListRequest from './StorageLocationsListRequest';

export default function listRequestToQueryParams(listRequest: StorageLocationsListParams) {
  // Query params need to be string or number
  const requestEntries = Object.entries({
    ...listRequest,
  });

  const defaultEntries = Object.entries({
    ...new StorageLocationsListRequest(),
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
