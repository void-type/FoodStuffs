<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useGroceryDepartmentStore from '@/stores/groceryDepartmentStore';
import { storeToRefs } from 'pinia';
import { computed, watch, type PropType } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';
import { toInt, toNumber } from '@/models/FormatHelper';
import Choices from '@/models/Choices';
import GroceryDepartmentsListRequest from '@/models/GroceryDepartmentsListRequest';
import ApiHelper from '@/models/ApiHelper';
import EntityTableControls from '@/components/EntityTableControls.vue';
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
const groceryDepartmentStore = useGroceryDepartmentStore();
const router = useRouter();
const api = ApiHelper.client;

const { useDarkMode } = storeToRefs(appStore);

const { listResponse, listRequest } = storeToRefs(groceryDepartmentStore);

const resultCountText = computed(() => {
  const itemSet = listResponse.value;

  const totalCount = itemSet.totalCount || 0;

  // If NaN or less than 0.
  if (!(totalCount > 0)) {
    return 'Found no grocery departments.';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} grocery departments.`;
});

function navigateSearch() {
  router.push({
    query: groceryDepartmentStore.currentQueryParams,
  });
}

function clearSearch() {
  groceryDepartmentStore.listRequest = {
    ...new GroceryDepartmentsListRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  };

  navigateSearch();
}

function startSearch() {
  groceryDepartmentStore.listRequest = {
    ...listRequest.value,
    page: 1,
  };

  navigateSearch();
}

function changePage(page: number) {
  groceryDepartmentStore.listRequest = { ...listRequest.value, page };

  navigateSearch();
}

function changeTake(take: number) {
  groceryDepartmentStore.listRequest = {
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  };

  navigateSearch();
}

function setListRequestFromQuery() {
  groceryDepartmentStore.listRequest = {
    ...new GroceryDepartmentsListRequest(),
    ...props.query,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  };
}

async function onDeleteGroceryDepartment(id: number | null | undefined) {
  async function deleteGroceryDepartment() {
    if (!id) {
      return;
    }

    try {
      const response = await api().groceryDepartmentsDelete(id);
      await groceryDepartmentStore.fetchGroceryDepartmentsList();

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Delete grocery department',
    description: 'Do you really want to delete this grocery department?',
    okAction: () => deleteGroceryDepartment(),
  };

  appStore.showModal(parameters);
}

watch(
  props,
  async () => {
    setListRequestFromQuery();
    await groceryDepartmentStore.fetchGroceryDepartmentsList();
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
    <table
      v-if="(listResponse.items?.length || 0) > 0"
      :class="{ 'table mt-3': true, 'table-dark': useDarkMode }"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Order</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="groceryDepartment in listResponse.items" :key="groceryDepartment.id">
          <td>
            <router-link
              :to="{ name: 'groceryDepartmentEdit', params: { id: groceryDepartment.id } }"
            >
              {{ groceryDepartment.name }}
            </router-link>
          </td>
          <td>{{ groceryDepartment.order }}</td>
          <td>
            <button
              class="btn btn-sm btn-danger"
              @click="() => onDeleteGroceryDepartment(groceryDepartment.id)"
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
      class="mt-3"
    />
  </div>
</template>

<style lang="scss" scoped></style>
