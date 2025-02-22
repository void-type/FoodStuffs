import type {
  IItemSetOfListPantryLocationsResponse,
  PantryLocationsListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import PantryLocationsListRequest from '@/models/PantryLocationsListRequest';
import listRequestToQueryParams from '@/models/PantryLocationStoreHelper';
import useMessageStore from './messageStore';

interface PantryLocationStoreState {
  listResponse: IItemSetOfListPantryLocationsResponse;
  listRequest: PantryLocationsListParams;
}

const api = ApiHelper.client;

export default defineStore('pantryLocation', {
  state: (): PantryLocationStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new PantryLocationsListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchPantryLocationsList() {
      try {
        const response = await api().pantryLocationsList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
