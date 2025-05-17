import type {
  IItemSetOfListStorageLocationsResponse,
  StorageLocationsListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import StorageLocationsListRequest from '@/models/StorageLocationsListRequest';
import listRequestToQueryParams from '@/models/StorageLocationStoreHelper';
import useMessageStore from './messageStore';

interface StorageLocationStoreState {
  listResponse: IItemSetOfListStorageLocationsResponse;
  listRequest: StorageLocationsListParams;
}

const api = ApiHelper.client;

export default defineStore('storageLocation', {
  state: (): StorageLocationStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new StorageLocationsListRequest() },
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async fetchStorageLocationsList() {
      try {
        const response = await api().storageLocationsList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
