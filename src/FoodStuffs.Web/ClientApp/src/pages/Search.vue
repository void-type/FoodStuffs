<script setup lang="ts">
import { Api } from '@/api/Api';
import type { ListRecipesResponse } from '@/api/data-contracts';
import Choices from '@/models/Choices';
import { toInt, toNumber } from '@/models/FormatHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted, type PropType } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '../components/EntityTablePager.vue';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const router = useRouter();

const { listResponse, listRequest } = storeToRefs(recipeStore);

function fetchList() {
  router
    .replace({
      query: {
        ...listRequest.value,
        sortDesc: String(listRequest.value.sortDesc),
        isPagingEnabled: String(listRequest.value.isPagingEnabled),
      },
    })
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    .catch(() => {});

  new Api()
    .recipesList(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}

function clearSearch() {
  recipeStore.setListRequest({
    ...new ListRecipesRequest(),
    take: listRequest.value.take,
  });

  fetchList();
}

function startSearch() {
  recipeStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  fetchList();
}

function changePage(page: number) {
  recipeStore.setListRequest({ ...recipeStore.listRequest, page });
  fetchList();
}

function changeTake(take: number) {
  recipeStore.setListRequest({
    ...recipeStore.listRequest,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  });

  fetchList();
}

function showDetails(recipe: ListRecipesResponse) {
  router.push({ name: 'view', params: { id: recipe.id } });
}

function tableSortChanged(sortBy: string, sortDesc: boolean) {
  recipeStore.setListRequest({
    ...listResponse.value,
    sortBy,
    sortDesc,
  });

  fetchList();
}

onMounted(() => {
  if (Object.keys(props.query).length !== 0) {
    recipeStore.setListRequest({
      ...new ListRecipesRequest(),
      ...props.query,
      sortDesc: JSON.parse(String(props.query.sortDesc?.valueOf())) === true,
      page: toNumber(Number(props.query.take), 1),
      take: toNumber(Number(props.query.page), Choices.paginationTake[0].value),
    });
  }

  fetchList();
});
</script>

<template>
  <div class="container">
    <h1 class="mt-4">Search Recipes</h1>
    <EntityTableControls :clear-search="clearSearch" :init-search="startSearch" class="mt-4">
      <template #searchControls>
        <div class="row">
          <div class="col-12 col-md-6">
            <div class="input-group mb-2">
              <span class="input-group-text">Name contains</span>
              <input
                id="nameSearch"
                v-model="listRequest.name"
                class="form-control"
                name="nameSearch"
              />
            </div>
          </div>
          <div class="col-12 col-md-6">
            <div class="input-group mb-2">
              <span class="input-group-text">Categories contain</span>
              <input
                id="categorySearch"
                v-model="listRequest.category"
                class="form-control"
                name="categorySearch"
              />
            </div>
          </div>
        </div>
      </template>
    </EntityTableControls>
    <!-- <b-table
      :items="listResponse.items"
      :fields="tableFields"
      :sort-by="listRequest.sortBy"
      :sort-desc="listRequest.sortDesc"
      sort-icon-left
      no-local-sorting
      show-empty
      hover
      class="mt-4"
      @row-clicked="showDetails"
      @sort-changed="tableSortChanged"
    >
      <template #cell(name)="data">
        <router-link class="table-link" :to="{ name: 'view', params: { id: data.item.id } }">
          {{ data.value }}
        </router-link>
      </template>
    </b-table> -->
    <EntityTablePager
      :list-request="recipeStore.listRequest"
      :total-count="toInt(recipeStore.listResponse.totalCount)"
      :on-change-page="changePage"
      :on-change-take="changeTake"
      class="mt-4"
    />
  </div>
</template>

<style scoped lang="scss"></style>
