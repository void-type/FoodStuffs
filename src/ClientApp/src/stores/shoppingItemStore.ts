import type {
  IItemSetOfListShoppingItemsResponse,
  ShoppingItemsListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import ShoppingItemsListRequest from '@/models/ShoppingItemsListRequest';
import listRequestToQueryParams from '@/models/ShoppingItemStoreHelper';
import useMessageStore from './messageStore';

interface ShoppingItemStoreState {
  listResponse: IItemSetOfListShoppingItemsResponse;
  listRequest: ShoppingItemsListParams;
}

const api = ApiHelper.client;

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
    listRequest: { ...new ShoppingItemsListRequest() },
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
