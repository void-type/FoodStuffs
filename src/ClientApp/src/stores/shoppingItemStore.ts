import type {
  IItemSetOfListShoppingItemsResponse,
  ShoppingItemsListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import SearchShoppingItemsRequest from '@/models/SearchShoppingItemsRequest';
import listRequestToQueryParams from '@/models/ShoppingItemStoreHelpers';
import useMessageStore from './messageStore';

interface ShoppingItemStoreState {
  listResponse: IItemSetOfListShoppingItemsResponse;
  listRequest: ShoppingItemsListParams;
}

const api = ApiHelpers.client;

export default defineStore('shoppingItem', {
  state: (): ShoppingItemStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new SearchShoppingItemsRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchShoppingItemsList() {
      try {
        const response = await api().shoppingItemsList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
