import type { CategoriesListParams, IItemSetOfListCategoriesResponse } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import CategoriesListRequest from '@/models/CategoriesListRequest';
import listRequestToQueryParams from '@/models/ShoppingItemStoreHelper';
import useMessageStore from './messageStore';

interface CategoryStoreState {
  listResponse: IItemSetOfListCategoriesResponse;
  listRequest: CategoriesListParams;
}

const api = ApiHelper.client;

export default defineStore('category', {
  state: (): CategoryStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new CategoriesListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchCategoriesList() {
      try {
        const response = await api().categoriesList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
