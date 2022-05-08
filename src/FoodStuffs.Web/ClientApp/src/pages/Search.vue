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
