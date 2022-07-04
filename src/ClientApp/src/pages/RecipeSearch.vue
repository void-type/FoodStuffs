<script lang="ts" setup>
import { Api } from '@/api/Api';
import type { ListRecipesResponse } from '@/api/data-contracts';
import Choices from '@/models/Choices';
import { toInt, toNumber } from '@/models/FormatHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted, watch, type PropType } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';

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
  router.push({
    query: recipeStore.currentQueryParams,
  });
}

function clearSearch() {
  recipeStore.setListRequest({
    ...new ListRecipesRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
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

function changeSort(columnName: string) {
  // 1st click asc
  let sortBy = columnName;
  let sortDesc = false;

  if (columnName === listRequest.value.sortBy) {
    if (listRequest.value.sortDesc !== true) {
      // 2nd click desc
      sortDesc = true;
    } else {
      // 3rd click turn off sorting
      sortBy = '';
    }
  }

  recipeStore.setListRequest({
    ...listRequest.value,
    sortBy,
    sortDesc,
  });

  fetchList();
}

function showDetails(recipe: ListRecipesResponse) {
  router.push({ name: 'view', params: { id: recipe.id } });
}

onMounted(() => {
  if (Object.keys(props.query).length !== 0) {
    recipeStore.setListRequest({
      ...new ListRecipesRequest(),
      ...props.query,
      sortDesc: JSON.parse(String(props.query.sortDesc?.valueOf())) === true,
      page: toNumber(Number(props.query.page), 1),
      take: toNumber(Number(props.query.take), Choices.paginationTake[0].value),
    });
  }

  fetchList();
});

watch(listRequest, () => {
  new Api()
    .recipesList(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4 mb-0">Search recipes</h1>
    <EntityTableControls :clear-search="clearSearch" :init-search="startSearch" class="mt-4">
      <template #searchForm>
        <div class="row">
          <div class="col-12 col-md-6">
            <div class="input-group mb-3">
              <label for="nameSearch" class="input-group-text">Name contains</label>
              <input id="nameSearch" v-model="listRequest.name" class="form-control" />
            </div>
          </div>
          <div class="col-12 col-md-6">
            <div class="input-group mb-3">
              <label for="categorySearch" class="input-group-text">Categories contain</label>
              <input id="categorySearch" v-model="listRequest.category" class="form-control" />
            </div>
          </div>
          <div class="col-12 col-md-6 col-lg-4">
            <div class="input-group mb-3">
              <label class="input-group-text" for="isForMealPlanning">For meal planning</label>
              <select
                id="isForMealPlanning"
                v-model="listRequest.isForMealPlanning"
                class="form-select"
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
        </div>
      </template>
    </EntityTableControls>
    <table class="table table-hover text-start mt-4">
      <thead>
        <tr>
          <th
            class="sortable"
            scope="col"
            tabindex="0"
            @click="changeSort('name')"
            @keyup.enter.stop.prevent="changeSort('name')"
          >
            Name
            <i v-if="listRequest.sortBy === 'name'">{{ listRequest.sortDesc ? '▼' : '▲' }}</i>
            <span v-if="listRequest.sortBy === 'name'" class="visually-hidden"
              >({{ listRequest.sortDesc ? 'Descending' : 'Ascending' }} sort)</span
            >
            <span v-else class="visually-hidden">(Not sorted)</span>
          </th>
          <th scope="col">Categories</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="recipe in listResponse.items" :key="recipe.id" @click="showDetails(recipe)">
          <td>
            <router-link class="table-link" :to="{ name: 'view', params: { id: recipe.id } }">{{
              recipe.name
            }}</router-link>
          </td>
          <td>{{ recipe?.categories?.join(', ') }}</td>
        </tr>
      </tbody>
    </table>
    <EntityTablePager
      :list-request="recipeStore.listRequest"
      :total-count="toInt(recipeStore.listResponse.totalCount)"
      :on-change-page="changePage"
      :on-change-take="changeTake"
      class="mt-3"
    />
  </div>
</template>

<style lang="scss" scoped>
@import '@/styles/theme';

// Hover table cursor
table.table-hover tbody {
  cursor: pointer;
}

table th.sortable:hover {
  cursor: pointer;
  --bs-table-accent-bg: var(--bs-table-hover-bg);
  color: var(--bs-table-hover-color);
}

.table-link {
  color: unset;
  text-decoration: none;
}
</style>
