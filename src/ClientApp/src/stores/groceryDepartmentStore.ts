import type {
  IItemSetOfListGroceryDepartmentsResponse,
  GroceryDepartmentsListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import GroceryDepartmentsListRequest from '@/models/GroceryDepartmentsListRequest';
import listRequestToQueryParams from '@/models/GroceryDepartmentStoreHelper';
import useMessageStore from './messageStore';

interface GroceryDepartmentStoreState {
  listResponse: IItemSetOfListGroceryDepartmentsResponse;
  listRequest: GroceryDepartmentsListParams;
}

const api = ApiHelper.client;

export default defineStore('groceryDepartment', {
  state: (): GroceryDepartmentStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new GroceryDepartmentsListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchGroceryDepartmentsList() {
      try {
        const response = await api().groceryDepartmentsList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
