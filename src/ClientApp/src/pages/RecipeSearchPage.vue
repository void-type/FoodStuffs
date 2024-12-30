<script lang="ts" setup>
import Choices from '@/models/Choices';
import ApiHelpers from '@/models/ApiHelpers';
import { toInt, toNumber, toNumberOrNull } from '@/models/FormatHelpers';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { watch, type PropType, ref, computed } from 'vue';
import { useRouter, type LocationQuery } from 'vue-router';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';
import useMessageStore from '@/stores/messageStore';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';
import RecipeSearchCategoriesFilter from '@/components/RecipeSearchCategoriesFilter.vue';
import RecipeCard from '@/components/RecipeCard.vue';
import RecipeListItem from '@/components/RecipeListItem.vue';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';

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

const useCompactView = ref(false);

const resultCountText = computed(() => {
  const itemSet = listResponse.value;

  const totalCount = itemSet.totalCount || 0;

  // If NaN or less than 0.
  if (!(totalCount > 0)) {
    return 'Found no recipes.';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} recipes.`;
});

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

  // selectedCategories gets its new value from query params.

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

  recipeStore.setListRequest({
    ...listRequest.value,
    sortBy: value,
    page: 1,
  });

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
    .recipesSearch(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => messageStore.setApiFailureMessages(response));
}

const { listFacets } = storeToRefs(recipeStore);

const categoryFacets = computed(() => {
  return listFacets.value.find((x) => x.fieldName === 'Category_Ids')?.values || [];
});

function getMealFacetCount(facetValue: boolean | null) {
  if (facetValue == null) {
    return null;
  }

  const count =
    recipeStore.listFacets
      .find((x) => x.fieldName === 'IsForMealPlanning')
      ?.values?.find((x) => x.fieldValue?.toLowerCase() === facetValue.toString().toLowerCase())
      ?.count || 0;

  return ` (${count})`;
}

watch(selectedCategories, () => {
  if (JSON.stringify(listRequest.value.categories) !== JSON.stringify(selectedCategories.value)) {
    recipeStore.setListRequest({
      ...listRequest.value,
      categories: selectedCategories.value,
      page: 1,
    });

    navigateSearch();
  }
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
    <AppBreadcrumbs />
    <h1 class="mt-3">Recipes</h1>
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
                {{ option.text }}{{ getMealFacetCount(option.value) }}
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
          <RecipeSearchCategoriesFilter
            v-model="selectedCategories"
            :facet-values="categoryFacets"
            class="g-col-12"
          />
        </div>
      </template>
    </EntityTableControls>
    <div class="mt-3">{{ resultCountText }}</div>
    <div class="form-check form-switch mt-3">
      <label class="form-check-label" for="useCompactView" aria-label="Use compact view"
        ><font-awesome-icon class="me-2" icon="fa-moon" />Compact view</label
      >
      <input
        id="useCompactView"
        v-model="useCompactView"
        :checked="useCompactView"
        class="form-check-input"
        type="checkbox"
      />
    </div>
    <div v-if="useCompactView" class="grid mt-3">
      <RecipeListItem
        v-for="(recipe, i) in listResponse.items"
        :key="recipe.id"
        :recipe="recipe"
        :lazy="i > 6"
        class="g-col-12 g-col-sm-6 g-col-lg-4"
      />
    </div>
    <div v-else class="grid mt-3">
      <RecipeCard
        v-for="(recipe, i) in listResponse.items"
        :key="recipe.id"
        :recipe="recipe"
        :lazy="i > 6"
        class="g-col-12 g-col-sm-6 g-col-lg-4"
      />
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
