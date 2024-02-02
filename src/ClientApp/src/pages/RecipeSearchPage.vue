<script lang="ts" setup>
import Choices from '@/models/Choices';
import ApiHelpers from '@/models/ApiHelpers';
import { toInt, toNumber, toNumberOrNull } from '@/models/FormatHelpers';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { watch, type PropType, ref } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';
import RecipeSearchCard from '@/components/RecipeSearchCard.vue';
import SidebarRecipeRecent from '@/components/SidebarRecipeRecent.vue';
import useMessageStore from '@/stores/messageStore';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';
import RecipeSearchCategoriesFilter from '@/components/RecipeSearchCategoriesFilter.vue';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const router = useRouter();
const api = ApiHelpers.client;

const { listResponse, listRequest } = storeToRefs(recipeStore);
const { sortOptions } = RecipeStoreHelpers;

const selectedCategories = ref([] as Array<number>);

function navigateSearch() {
  router.push({
    query: recipeStore.currentQueryParams,
  });
}

function clearSearch() {
  recipeStore.setListRequest({
    ...new SearchRecipesRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  });

  // selectedCategories gets it's new value from query params.

  navigateSearch();
}

function startSearch() {
  recipeStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  navigateSearch();
}

function changePage(page: number) {
  recipeStore.setListRequest({ ...listRequest.value, page });

  navigateSearch();
}

function changeTake(take: number) {
  recipeStore.setListRequest({
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  });

  navigateSearch();
}

function changeSort(event: Event) {
  const { value } = event.target as HTMLSelectElement;
  listRequest.value.sortBy = value;
  navigateSearch();
}

function setListRequestFromQuery() {
  const categories =
    props.query.categories
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  selectedCategories.value = categories;

  recipeStore.setListRequest({
    ...new SearchRecipesRequest(),
    ...props.query,
    categories,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  });
}

function fetchList() {
  api()
    .recipesList(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => messageStore.setApiFailureMessages(response));
}

watch(selectedCategories, () => {
  listRequest.value.categories = selectedCategories.value;
  navigateSearch();
});

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
    <h1 class="mt-4 mb-4">Search recipes</h1>
    <div class="grid">
      <div class="g-col-12 g-col-lg-9">
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
              <div class="g-col-6 g-col-md-3">
                <label class="form-label" for="isForMealPlanning">Meals</label>
                <select
                  id="isForMealPlanning"
                  v-model="listRequest.isForMealPlanning"
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
              <div class="g-col-6 g-col-md-3">
                <label for="recipeSort" class="form-label">Sort</label>
                <select
                  id="recipeSort"
                  :value="listRequest.sortBy"
                  name="recipeSort"
                  class="form-select"
                  aria-label="Page size"
                  @change="changeSort"
                >
                  <option
                    v-for="sortOption in sortOptions"
                    :key="sortOption.value"
                    :value="sortOption.value"
                  >
                    {{ sortOption.text }}
                  </option>
                </select>
              </div>
              <RecipeSearchCategoriesFilter v-model="selectedCategories" class="g-col-12" />
            </div>
          </template>
        </EntityTableControls>
        <div class="grid mt-4">
          <div v-if="(listResponse.items?.length || 0) < 1" class="g-col-12 p-4 text-center">
            No results
          </div>
          <RecipeSearchCard
            v-for="recipe in listResponse.items"
            :key="recipe.id"
            :recipe="recipe"
            class="g-col-12"
          />
        </div>
        <EntityTablePager
          :list-request="listRequest"
          :total-count="toInt(listResponse.totalCount)"
          :on-change-page="changePage"
          :on-change-take="changeTake"
          class="mt-4"
        />
      </div>
      <div class="g-col-12 g-col-lg-3 d-print-none">
        <SidebarRecipeRecent :route-name="'view'" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
