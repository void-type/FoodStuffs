<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useShoppingItemStore from '@/stores/shoppingItemStore';
import { storeToRefs } from 'pinia';
import { computed, watch, type PropType } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';
import { toInt, toNumber } from '@/models/FormatHelpers';
import Choices from '@/models/Choices';
import SearchShoppingItemsRequest from '@/models/SearchShoppingItemsRequest';
import ApiHelpers from '@/models/ApiHelpers';
import EntityTableControls from '@/components/EntityTableControls.vue';
import useMessageStore from '@/stores/messageStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const shoppingItemStore = useShoppingItemStore();
const router = useRouter();
const api = ApiHelpers.client;

const { useDarkMode } = storeToRefs(appStore);

const { listResponse, listRequest } = storeToRefs(shoppingItemStore);

const resultCountText = computed(() => {
  const itemSet = listResponse.value;

  const totalCount = itemSet.totalCount || 0;

  // If NaN or less than 0.
  if (!(totalCount > 0)) {
    return 'Found no shopping items.';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} shopping items.`;
});

function navigateSearch() {
  router.push({
    query: shoppingItemStore.currentQueryParams,
  });
}

function clearSearch() {
  shoppingItemStore.listRequest = {
    ...new SearchShoppingItemsRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  };

  navigateSearch();
}

function startSearch() {
  shoppingItemStore.listRequest = {
    ...listRequest.value,
    page: 1,
  };

  navigateSearch();
}

function changePage(page: number) {
  shoppingItemStore.listRequest = { ...listRequest.value, page };

  navigateSearch();
}

function changeTake(take: number) {
  shoppingItemStore.listRequest = {
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  };

  navigateSearch();
}

function setListRequestFromQuery() {
  shoppingItemStore.listRequest = {
    ...new SearchShoppingItemsRequest(),
    ...props.query,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  };
}

function newShoppingItem() {
  router.push({ name: 'shoppingItemNew' });
}

async function onDeleteShoppingItem(id: number | null | undefined) {
  function deleteShoppingItem() {
    if (!id) {
      return;
    }

    api()
      .shoppingItemsDelete(id)
      .then(() => {
        navigateSearch();
      })
      .catch((response) => {
        messageStore.setApiFailureMessages(response);
      });
  }

  const parameters: ModalParameters = {
    title: 'Delete shopping item',
    description: 'Do you really want to delete this shopping item?',
    okAction: () => deleteShoppingItem(),
  };

  appStore.showModal(parameters);
}

function fetchList() {
  api()
    .shoppingItemsList(listRequest.value)
    .then((response) => {
      shoppingItemStore.listResponse = response.data;
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
}

watch(
  props,
  () => {
    setListRequestFromQuery();
    fetchList();
  },
  { immediate: true }
);
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <h1 class="mt-3">Shopping Items</h1>
    <div class="btn-toolbar mt-4">
      <button class="btn btn-secondary me-2" @click.stop.prevent="newShoppingItem">New</button>
    </div>
    <EntityTableControls :clear-search="clearSearch" :init-search="startSearch">
      <template #searchForm>
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
        </div>
      </template>
    </EntityTableControls>
    <div class="mt-4">{{ resultCountText }}</div>
    <table
      v-if="(listResponse.items?.length || 0) > 0"
      :class="{ table: true, 'table-dark': useDarkMode, ' mt-3': true }"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="shoppingItem in listResponse.items" :key="shoppingItem.id">
          <td>
            <!-- <router-link :to="{ name: 'shoppingItemEdit', params: { id: shoppingItem.id } }"> -->
            {{ shoppingItem.name }}
            <!-- </router-link> -->
          </td>
          <td>
            <button
              class="btn btn-sm btn-danger"
              @click="() => onDeleteShoppingItem(shoppingItem.id)"
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
      class="mt-4"
    />
  </div>
</template>

<style lang="scss" scoped></style>
