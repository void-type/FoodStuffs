<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useShoppingItemStore from '@/stores/shoppingItemStore';
import { storeToRefs } from 'pinia';
import { computed, watch, type PropType } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';
import { toInt, toNumber } from '@/models/FormatHelper';
import Choices from '@/models/Choices';
import ShoppingItemsListRequest from '@/models/ShoppingItemsListRequest';
import ApiHelper from '@/models/ApiHelper';
import EntityTableControls from '@/components/EntityTableControls.vue';
import useMessageStore from '@/stores/messageStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import type { HttpResponse } from '@/api/http-client';
import ShoppingItemInventoryQuantity from '@/components/ShoppingItemInventoryQuantity.vue';

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
const api = ApiHelper.client;

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
    ...new ShoppingItemsListRequest(),
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
    ...new ShoppingItemsListRequest(),
    ...props.query,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  };
}

async function onDeleteShoppingItem(id: number | null | undefined) {
  async function deleteShoppingItem() {
    if (!id) {
      return;
    }

    try {
      const response = await api().shoppingItemsDelete(id);
      await shoppingItemStore.fetchShoppingItemsList();

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Delete shopping item',
    description: 'Do you really want to delete this shopping item?',
    okAction: () => deleteShoppingItem(),
  };

  appStore.showModal(parameters);
}

watch(
  props,
  async () => {
    setListRequestFromQuery();
    await shoppingItemStore.fetchShoppingItemsList();
  },
  { immediate: true }
);
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <EntityTableControls class="mt-3" :clear-search="clearSearch" :init-search="startSearch">
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
      </template>
    </EntityTableControls>
    <div class="mt-3">{{ resultCountText }}</div>
    <div class="grid mt-3">
      <div
        v-for="shoppingItem in listResponse.items"
        :key="shoppingItem.id"
        class="card g-col-12 g-col-md-6"
      >
        <div class="card-header">
          <router-link :to="{ name: 'shoppingItemEdit', params: { id: shoppingItem.id } }">
            {{ shoppingItem.name }}
          </router-link>
        </div>
        <div class="card-body">
          <div class="btn-toolbar d-none">
            <button
              class="btn btn-sm btn-danger ms-auto"
              @click="() => onDeleteShoppingItem(shoppingItem.id)"
            >
              Delete
            </button>
          </div>
          <div class="grid">
            <ShoppingItemInventoryQuantity
              :id="`${shoppingItem.id}-inventoryQuantity`"
              v-model="shoppingItem.inventoryQuantity"
              :item="shoppingItem"
              :inline="true"
              class="g-col-12 g-col-sm-6 g-col-md-12 g-col-lg-6"
            />
          </div>
          <div v-if="(shoppingItem.pantryLocations?.length || 0) > 0" class="mt-3">
            <span
              v-for="location in shoppingItem.pantryLocations"
              :key="location || ''"
              class="badge rounded-pill text-bg-secondary me-2 mt-2"
            >
              {{ location }}</span
            >
          </div>
        </div>
      </div>
    </div>
    <EntityTablePager
      v-if="(listResponse.items?.length || 0) > 0"
      :list-request="listRequest"
      :total-count="toInt(listResponse.totalCount)"
      :on-change-page="changePage"
      :on-change-take="changeTake"
      class="mt-3"
    />
  </div>
</template>

<style lang="scss" scoped></style>
