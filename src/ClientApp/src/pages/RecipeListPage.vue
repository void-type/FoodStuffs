<script lang="ts" setup>
import Choices from '@/models/Choices';
import { toInt, toNumber, toNumberOrNull } from '@/models/FormatHelper';
import RecipesSearchRequest from '@/models/RecipesSearchRequest';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { watch, type PropType, ref, computed } from 'vue';
import { useRoute, useRouter, type LocationQuery } from 'vue-router';
import EntityTablePager from '@/components/EntityTablePager.vue';
import RecipeSearchCategoriesFilter from '@/components/RecipeSearchCategoriesFilter.vue';
import RecipeCard from '@/components/RecipeCard.vue';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const recipeStore = useRecipeStore();
const router = useRouter();
const route = useRoute();

const { listResponse, listRequest, listFacets } = storeToRefs(recipeStore);
const { sortOptions } = Choices;

const categoriesFilterModel = ref({
  categories: [] as Array<number>,
  matchAllCategories: false,
});

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

function navigateSearch(toResults: boolean) {
  const routeParams = {
    query: recipeStore.currentQueryParams,
    hash: undefined as string | undefined,
  };

  if (toResults) {
    routeParams.hash = '#search-results';
  } else {
    // Don't scroll when editing filters.
    routeParams.hash = '#';
  }
  router.push(routeParams);
}

function clearSearch() {
  recipeStore.setListRequest({
    ...new RecipesSearchRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  });

  // selectedCategories gets its new value from query params.

  navigateSearch(true);
}

function startSearchNoHash() {
  recipeStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  navigateSearch(false);
}

function startSearch() {
  recipeStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  navigateSearch(true);
}

function changePage(page: number) {
  recipeStore.setListRequest({ ...listRequest.value, page });

  navigateSearch(true);
}

function changeTake(take: number) {
  recipeStore.setListRequest({
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  });

  navigateSearch(true);
}

function changeSort(event: Event) {
  const { value } = event.target as HTMLSelectElement;

  recipeStore.setListRequest({
    ...listRequest.value,
    sortBy: value,
    page: 1,
  });

  navigateSearch(false);
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

  categoriesFilterModel.value.categories = categories;
  categoriesFilterModel.value.matchAllCategories = props.query.matchAllCategories === 'true';

  recipeStore.setListRequest({
    ...new RecipesSearchRequest(),
    ...props.query,
    categories,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  });
}

const categoryFacets = computed(() => {
  return listFacets.value.find((x) => x.fieldName === 'Categories')?.values || [];
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

watch(
  categoriesFilterModel,
  () => {
    const { categories, matchAllCategories } = listRequest.value;

    const initialModel = {
      categories,
      matchAllCategories,
    };

    if (JSON.stringify(initialModel) !== JSON.stringify(categoriesFilterModel.value)) {
      recipeStore.setListRequest({
        ...listRequest.value,
        ...categoriesFilterModel.value,
        page: 1,
      });

      navigateSearch(false);
    }
  },
  { deep: true }
);

watch(
  props,
  async () => {
    setListRequestFromQuery();
    await recipeStore.fetchRecipesList();
  },
  { immediate: true }
);
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <div id="skip-filters" class="container-xxl visually-hidden-focusable">
      <router-link
        class="d-inline-flex p-2 m-1"
        :to="{ hash: '#search-results', query: route.query }"
        >Skip to search results</router-link
      >
    </div>

    <!-- Two column layout -->
    <div class="grid mt-3 gap-lg">
      <!-- Left rail filters - desktop only -->
      <div class="g-col-12 g-col-lg-3 d-none d-lg-block">
        <div>
          <label class="form-label" for="filterAccordionDesktop">Filters</label>
          <div id="filterAccordionDesktop" class="accordion">
            <div class="accordion-item">
              <div class="accordion-header">
                <button
                  class="accordion-button collapsed"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#isForMealPlanningCollapseDesktop"
                  aria-expanded="false"
                  aria-controls="isForMealPlanningCollapseDesktop"
                >
                  For Meal Planning
                </button>
              </div>
              <div
                id="isForMealPlanningCollapseDesktop"
                class="accordion-collapse collapse"
                data-bs-parent="#filterAccordionDesktop"
              >
                <div class="accordion-body">
                  <div class="form-group">
                    <div
                      v-for="option in Choices.boolean"
                      :key="option.value?.toString()"
                      class="form-check"
                    >
                      <input
                        :id="`isForMealPlanning-desktop-${option.value}`"
                        v-model="listRequest.isForMealPlanning"
                        class="form-check-input"
                        type="radio"
                        name="isForMealPlanning"
                        :value="option.value"
                        @change="startSearchNoHash"
                      />
                      <label
                        class="form-check-label"
                        :for="`isForMealPlanning-desktop-${option.value}`"
                      >
                        {{ option.text }}{{ getMealFacetCount(option.value) }}
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <RecipeSearchCategoriesFilter
              v-model="categoriesFilterModel"
              :facet-values="categoryFacets"
              parent-accordion-id="filterAccordionDesktop"
              check-class="g-col-12"
            />
          </div>
        </div>
      </div>

      <!-- Main content area -->
      <div class="g-col-12 g-col-lg-9">
        <div class="grid mb-3 gap-sm">
          <div class="g-col-12 g-col-md-9">
            <label for="searchText" class="form-label">Search</label>
            <input
              id="searchText"
              v-model="listRequest.searchText"
              type="search"
              inputmode="search"
              enterkeyhint="search"
              class="form-control"
              @keydown.stop.prevent.enter="startSearch"
            />
          </div>
          <div class="g-col-12 g-col-md-3">
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

          <!-- Mobile filters - only visible on screens smaller than lg -->
          <div class="g-col-12 d-lg-none">
            <label class="form-label" for="filterAccordion">Filters</label>
            <div id="filterAccordion" class="accordion">
              <div class="accordion-item">
                <div class="accordion-header">
                  <button
                    class="accordion-button collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#isForMealPlanningCollapse"
                    aria-expanded="false"
                    aria-controls="isForMealPlanningCollapse"
                  >
                    For Meal Planning
                  </button>
                </div>
                <div
                  id="isForMealPlanningCollapse"
                  class="accordion-collapse collapse"
                  data-bs-parent="#filterAccordion"
                >
                  <div class="accordion-body">
                    <div class="form-group">
                      <div
                        v-for="option in Choices.boolean"
                        :key="option.value?.toString()"
                        class="form-check"
                      >
                        <input
                          :id="`isForMealPlanning-${option.value}`"
                          v-model="listRequest.isForMealPlanning"
                          class="form-check-input"
                          type="radio"
                          name="isForMealPlanning"
                          :value="option.value"
                          @change="startSearchNoHash"
                        />
                        <label class="form-check-label" :for="`isForMealPlanning-${option.value}`">
                          {{ option.text }}{{ getMealFacetCount(option.value) }}
                        </label>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <RecipeSearchCategoriesFilter
                v-model="categoriesFilterModel"
                :facet-values="categoryFacets"
                parent-accordion-id="filterAccordion"
              />
            </div>
          </div>
        </div>

        <div class="btn-toolbar">
          <button class="btn btn-primary me-2" type="button" @click.stop.prevent="startSearch()">
            Search
          </button>
          <button class="btn btn-secondary me-2" type="button" @click.stop.prevent="clearSearch()">
            Clear
          </button>
          <router-link :to="{ name: 'recipeNew' }" class="btn btn-secondary">New</router-link>
        </div>

        <div id="search-results" class="mt-3">{{ resultCountText }}</div>
        <div class="form-check form-switch mt-3">
          <label class="form-check-label" for="useCompactView">Compact</label>
          <input
            id="useCompactView"
            v-model="useCompactView"
            :checked="useCompactView"
            class="form-check-input"
            type="checkbox"
          />
        </div>
        <div class="grid mt-3">
          <RecipeCard
            v-for="(recipe, i) in listResponse.items"
            :key="recipe.id"
            :recipe="recipe"
            :lazy="i > 6"
            :show-compact-view="useCompactView"
            class="g-col-12 g-col-sm-6"
          />
        </div>
        <EntityTablePager
          v-if="(listResponse.items?.length || 0) > 0"
          :list-request="listRequest"
          :total-count="toInt(listResponse.totalCount)"
          :on-change-page="changePage"
          :on-change-take="changeTake"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
