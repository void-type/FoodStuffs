import type {
  IItemSetOfSearchGroceryItemsResultItem,
  GroceryItemsSearchParams,
  SearchGroceryItemsResponse,
  SearchFacet,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import GroceryItemsSearchRequest from '@/models/GroceryItemsSearchRequest';
import listRequestToQueryParams from '@/models/GroceryItemStoreHelper';
import useMessageStore from './messageStore';

interface GroceryItemStoreState {
  listResponse: IItemSetOfSearchGroceryItemsResultItem;
  listFacets: SearchFacet[];
  listRequest: GroceryItemsSearchParams;
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
    listFacets: [],
    listRequest: { ...new GroceryItemsSearchRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    setListResponse(data: SearchGroceryItemsResponse) {
      if (data.results) {
        this.listResponse = data.results;
      }

      if (data.facets) {
        this.listFacets = data.facets;
      }
    },

    setListRequest(data: GroceryItemsSearchParams) {
      this.listRequest = data;
    },

    async fetchGroceryItemsList() {
      try {
        const response = await api().groceryItemsSearch(this.listRequest);

        if (response.data) {
          this.setListResponse(response.data);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
