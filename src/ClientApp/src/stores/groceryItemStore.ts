import type {
  IItemSetOfListGroceryItemsResponse,
  GroceryItemsListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import GroceryItemsListRequest from '@/models/GroceryItemsListRequest';
import listRequestToQueryParams from '@/models/GroceryItemStoreHelper';
import useMessageStore from './messageStore';

interface GroceryItemStoreState {
  listResponse: IItemSetOfListGroceryItemsResponse;
  listRequest: GroceryItemsListParams;
}

const api = ApiHelper.client;

export default defineStore('groceryItem', {
  state: (): GroceryItemStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new GroceryItemsListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchGroceryItemsList() {
      try {
        const response = await api().groceryItemsList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
