import type {
  GroceryStoresListParams,
  IItemSetOfListGroceryStoresResponse,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import { defineStore } from 'pinia';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import GroceryStoresListRequest from '@/models/GroceryStoresListRequest';
import listRequestToQueryParams from '@/models/GroceryStoreStoreHelper';
import useMessageStore from './messageStore';

interface GroceryStoreStoreState {
  listResponse: IItemSetOfListGroceryStoresResponse;
  listRequest: GroceryStoresListParams;
}

const api = ApiHelper.client;

export default defineStore('groceryStore', {
  state: (): GroceryStoreStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new GroceryStoresListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchGroceryStoresList() {
      try {
        const response = await api().groceryStoresList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
