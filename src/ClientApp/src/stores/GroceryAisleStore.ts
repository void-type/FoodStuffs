import type {
  IItemSetOfListGroceryAislesResponse,
  GroceryAislesListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import GroceryAislesListRequest from '@/models/GroceryAislesListRequest';
import listRequestToQueryParams from '@/models/GroceryAisleStoreHelper';
import useMessageStore from './messageStore';

interface GroceryAisleStoreState {
  listResponse: IItemSetOfListGroceryAislesResponse;
  listRequest: GroceryAislesListParams;
}

const api = ApiHelper.client;

export default defineStore('groceryAisle', {
  state: (): GroceryAisleStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new GroceryAislesListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchGroceryAislesList() {
      try {
        const response = await api().groceryAislesList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
