<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useStorageLocationStore from '@/stores/storageLocationStore';
import { storeToRefs } from 'pinia';
import { computed, watch, type PropType } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';
import { toInt, toNumber } from '@/models/FormatHelper';
import Choices from '@/models/Choices';
import StorageLocationsListRequest from '@/models/StorageLocationsListRequest';
import ApiHelper from '@/models/ApiHelper';
import useMessageStore from '@/stores/messageStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import type { HttpResponse } from '@/api/http-client';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const storageLocationStore = useStorageLocationStore();
const router = useRouter();
const api = ApiHelper.client;

const { useDarkMode } = storeToRefs(appStore);

const { listResponse, listRequest } = storeToRefs(storageLocationStore);

const resultCountText = computed(() => {
  const itemSet = listResponse.value;

  const totalCount = itemSet.totalCount || 0;

  // If NaN or less than 0.
  if (!(totalCount > 0)) {
    return 'Found no storage locations.';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} storage locations.`;
});

function navigateSearch() {
  router.push({
    query: storageLocationStore.currentQueryParams,
  });
}

function clearSearch() {
  storageLocationStore.listRequest = {
    ...new StorageLocationsListRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  };

  navigateSearch();
}

function startSearch() {
  storageLocationStore.listRequest = {
    ...listRequest.value,
    page: 1,
  };

  navigateSearch();
}

function changePage(page: number) {
  storageLocationStore.listRequest = { ...listRequest.value, page };

  navigateSearch();
}

function changeTake(take: number) {
  storageLocationStore.listRequest = {
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  };

  navigateSearch();
}

function setListRequestFromQuery() {
  storageLocationStore.listRequest = {
    ...new StorageLocationsListRequest(),
    ...props.query,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  };
}

async function onDeleteStorageLocation(id: number | null | undefined) {
  async function deleteStorageLocation() {
    if (!id) {
      return;
    }

    try {
      const response = await api().storageLocationsDelete(id);
      await storageLocationStore.fetchStorageLocationsList();

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Delete storage location',
    description: 'Do you really want to delete this storage location?',
    okAction: () => deleteStorageLocation(),
  };

  appStore.showModal(parameters);
}

watch(
  props,
  async () => {
    setListRequestFromQuery();
    await storageLocationStore.fetchStorageLocationsList();
  },
  { immediate: true }
);
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <div class="mt-3">
      <div class="grid mb-3 gap-sm">
        <div class="g-col-12 g-col-md-6">
          <label for="nameSearch" class="form-label">Name</label>
          <input
            id="nameSearch"
            v-model="listRequest.name"
            class="form-control"
            @keydown.stop.prevent.enter="startSearch"
          />
        </div>
        <div class="g-col-6 g-col-md-3">
          <label class="form-label" for="isUnused">Unused</label>
          <select
            id="isUnused"
            v-model="listRequest.isUnused"
            class="form-select"
            @change="startSearch"
          >
            <option
              v-for="option in Choices.boolean"
              :key="option.value?.toString()"
              :value="option.value"
            >
              {{ option.text }}
            </option>
          </select>
        </div>
      </div>
      <div class="btn-toolbar">
        <button class="btn btn-primary me-2" type="button" @click.stop.prevent="startSearch()">
          Search
        </button>
        <button class="btn btn-secondary me-2" type="button" @click.stop.prevent="clearSearch()">
          Clear
        </button>
        <router-link :to="{ name: 'storageLocationNew' }" class="btn btn-secondary"
          >New</router-link
        >
      </div>
    </div>
    <div class="mt-3">{{ resultCountText }}</div>
    <table
      v-if="(listResponse.items?.length || 0) > 0"
      :class="{ 'table mt-3': true, 'table-dark': useDarkMode }"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Grocery Items</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="storageLocation in listResponse.items" :key="storageLocation.id">
          <td>
            <router-link :to="{ name: 'storageLocationEdit', params: { id: storageLocation.id } }">
              {{ storageLocation.name }}
            </router-link>
          </td>
          <td>{{ storageLocation.groceryItemCount }}</td>
          <td>
            <button
              class="btn btn-sm btn-danger"
              @click="() => onDeleteStorageLocation(storageLocation.id)"
            >
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <EntityTablePager
      v-if="(listResponse.items?.length || 0) > 0"
      :list-request="listRequest"
      :total-count="toInt(listResponse.totalCount)"
      :on-change-page="changePage"
      :on-change-take="changeTake"
    />
  </div>
</template>

<style lang="scss" scoped></style>
