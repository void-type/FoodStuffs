<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useGroceryItemStore from '@/stores/groceryItemStore';
import { storeToRefs } from 'pinia';
import { computed, watch, type PropType, ref } from 'vue';
import { useRouter, useRoute, type LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';
import { toInt, toNumber, toNumberOrNull } from '@/models/FormatHelper';
import Choices from '@/models/Choices';
import GroceryItemsSearchRequest from '@/models/GroceryItemsSearchRequest';
import ApiHelper from '@/models/ApiHelper';
import useMessageStore from '@/stores/messageStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import TagBadge from '@/components/TagBadge.vue';
import type { HttpResponse } from '@/api/http-client';
import GroceryItemInventoryQuantity from '@/components/GroceryItemInventoryQuantity.vue';
import GroceryItemSearchStorageLocationsFilter from '@/components/GroceryItemSearchStorageLocationsFilter.vue';
import GroceryItemSearchGroceryAislesFilter from '@/components/GroceryItemSearchGroceryAislesFilter.vue';
import AppScrollToTop from '@/components/AppScrollToTop.vue';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const groceryItemStore = useGroceryItemStore();
const router = useRouter();
const route = useRoute();
const api = ApiHelper.client;

const { listResponse, listRequest, listFacets } = storeToRefs(groceryItemStore);
const { sortOptions } = Choices;

const storageLocationsFilterModel = ref({
  storageLocations: [] as Array<number>,
  matchAllStorageLocations: false,
});

const groceryAislesFilterModel = ref({
  groceryAisles: [] as Array<number>,
});

const resultCountText = computed(() => {
  const itemSet = listResponse.value;

  const totalCount = itemSet.totalCount || 0;

  // If NaN or less than 0.
  if (!(totalCount > 0)) {
    return 'Found no grocery items.';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} grocery items.`;
});

function navigateSearch(toResults: boolean) {
  const routeParams = {
    query: groceryItemStore.currentQueryParams,
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
  groceryItemStore.setListRequest({
    ...new GroceryItemsSearchRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  });

  // selectedFilters gets their new values from query params.

  navigateSearch(true);
}

function startSearchNoHash() {
  groceryItemStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  navigateSearch(false);
}

function startSearch() {
  groceryItemStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  navigateSearch(true);
}

function changePage(page: number) {
  groceryItemStore.setListRequest({ ...listRequest.value, page });

  navigateSearch(true);
}

function changeTake(take: number) {
  groceryItemStore.setListRequest({
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  });

  navigateSearch(true);
}

function changeSort(event: Event) {
  const { value } = event.target as HTMLSelectElement;

  groceryItemStore.setListRequest({
    ...listRequest.value,
    sortBy: value,
    page: 1,
  });

  navigateSearch(false);
}

function setListRequestFromQuery() {
  const storageLocations =
    props.query.storageLocations
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  const groceryAisles =
    props.query.groceryAisles
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  storageLocationsFilterModel.value.storageLocations = storageLocations;
  storageLocationsFilterModel.value.matchAllStorageLocations =
    props.query.matchAllStorageLocations === 'true';

  groceryAislesFilterModel.value.groceryAisles = groceryAisles;

  groceryItemStore.setListRequest({
    ...new GroceryItemsSearchRequest(),
    ...props.query,
    storageLocations,
    groceryAisles,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  });
}

const storageLocationFacets = computed(() => {
  return listFacets.value.find((x) => x.fieldName === 'StorageLocations')?.values || [];
});

const groceryAisleFacets = computed(() => {
  return listFacets.value.find((x) => x.fieldName === 'GroceryAisle')?.values || [];
});

function getOutOfStockFacetCount(facetValue: boolean | null) {
  if (facetValue == null) {
    return null;
  }

  const count =
    groceryItemStore.listFacets
      .find((x) => x.fieldName === 'IsOutOfStock')
      ?.values?.find((x) => x.fieldValue?.toLowerCase() === facetValue.toString().toLowerCase())
      ?.count || 0;

  return ` (${count})`;
}

function getUnusedFacetCount(facetValue: boolean | null) {
  if (facetValue == null) {
    return null;
  }

  const count =
    groceryItemStore.listFacets
      .find((x) => x.fieldName === 'IsUnused')
      ?.values?.find((x) => x.fieldValue?.toLowerCase() === facetValue.toString().toLowerCase())
      ?.count || 0;

  return ` (${count})`;
}

async function onDeleteGroceryItem(id: number | null | undefined) {
  async function deleteGroceryItem() {
    if (!id) {
      return;
    }

    try {
      const response = await api().groceryItemsDelete(id);
      await groceryItemStore.fetchGroceryItemsList();

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Delete grocery item',
    description: 'Do you really want to delete this grocery item?',
    okAction: () => deleteGroceryItem(),
  };

  appStore.showModal(parameters);
}

watch(
  storageLocationsFilterModel,
  () => {
    const { storageLocations, matchAllStorageLocations } = listRequest.value;

    const initialModel = {
      storageLocations,
      matchAllStorageLocations,
    };

    if (JSON.stringify(initialModel) !== JSON.stringify(storageLocationsFilterModel.value)) {
      groceryItemStore.setListRequest({
        ...listRequest.value,
        ...storageLocationsFilterModel.value,
        page: 1,
      });

      navigateSearch(false);
    }
  },
  { deep: true }
);

watch(
  groceryAislesFilterModel,
  () => {
    const { groceryAisles } = listRequest.value;

    const initialModel = {
      groceryAisles,
    };

    if (JSON.stringify(initialModel) !== JSON.stringify(groceryAislesFilterModel.value)) {
      groceryItemStore.setListRequest({
        ...listRequest.value,
        ...groceryAislesFilterModel.value,
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
    await groceryItemStore.fetchGroceryItemsList();
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
                  data-bs-target="#isUnusedCollapseDesktop"
                  aria-expanded="false"
                  aria-controls="isUnusedCollapseDesktop"
                >
                  Unused
                </button>
              </div>
              <div
                id="isUnusedCollapseDesktop"
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
                        :id="`isUnusedDesktop-${option.value}`"
                        v-model="listRequest.isUnused"
                        class="form-check-input"
                        type="radio"
                        name="isUnusedDesktop"
                        :value="option.value"
                        @change="startSearchNoHash"
                      />
                      <label class="form-check-label" :for="`isUnusedDesktop-${option.value}`">
                        {{ option.text }}{{ getUnusedFacetCount(option.value) }}
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="accordion-item">
              <div class="accordion-header">
                <button
                  class="accordion-button collapsed"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#isOutOfStockCollapseDesktop"
                  aria-expanded="false"
                  aria-controls="isOutOfStockCollapseDesktop"
                >
                  Out of Stock
                </button>
              </div>
              <div
                id="isOutOfStockCollapseDesktop"
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
                        :id="`isOutOfStockDesktop-${option.value}`"
                        v-model="listRequest.isOutOfStock"
                        class="form-check-input"
                        type="radio"
                        name="isOutOfStockDesktop"
                        :value="option.value"
                        @change="startSearchNoHash"
                      />
                      <label class="form-check-label" :for="`isOutOfStockDesktop-${option.value}`">
                        {{ option.text }}{{ getOutOfStockFacetCount(option.value) }}
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <GroceryItemSearchStorageLocationsFilter
              v-model="storageLocationsFilterModel"
              :facet-values="storageLocationFacets"
              parent-accordion-id="filterAccordionDesktop"
              check-class="g-col-12"
            />
            <GroceryItemSearchGroceryAislesFilter
              v-model="groceryAislesFilterModel"
              :facet-values="groceryAisleFacets"
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
            <label for="groceryItemSort" class="form-label">Sort</label>
            <select
              id="groceryItemSort"
              :value="listRequest.sortBy"
              name="groceryItemSort"
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
                    data-bs-target="#isUnusedCollapse"
                    aria-expanded="false"
                    aria-controls="isUnusedCollapse"
                  >
                    Unused
                  </button>
                </div>
                <div
                  id="isUnusedCollapse"
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
                          :id="`isUnused-${option.value}`"
                          v-model="listRequest.isUnused"
                          class="form-check-input"
                          type="radio"
                          name="isUnused"
                          :value="option.value"
                          @change="startSearchNoHash"
                        />
                        <label class="form-check-label" :for="`isUnused-${option.value}`">
                          {{ option.text }}{{ getUnusedFacetCount(option.value) }}
                        </label>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="accordion-item">
                <div class="accordion-header">
                  <button
                    class="accordion-button collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#isOutOfStockCollapse"
                    aria-expanded="false"
                    aria-controls="isOutOfStockCollapse"
                  >
                    Out of Stock
                  </button>
                </div>
                <div
                  id="isOutOfStockCollapse"
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
                          :id="`isOutOfStock-${option.value}`"
                          v-model="listRequest.isOutOfStock"
                          class="form-check-input"
                          type="radio"
                          name="isOutOfStock"
                          :value="option.value"
                          @change="startSearchNoHash"
                        />
                        <label class="form-check-label" :for="`isOutOfStock-${option.value}`">
                          {{ option.text }}{{ getOutOfStockFacetCount(option.value) }}
                        </label>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <GroceryItemSearchStorageLocationsFilter
                v-model="storageLocationsFilterModel"
                :facet-values="storageLocationFacets"
                parent-accordion-id="filterAccordion"
              />
              <GroceryItemSearchGroceryAislesFilter
                v-model="groceryAislesFilterModel"
                :facet-values="groceryAisleFacets"
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
          <router-link :to="{ name: 'groceryItemNew' }" class="btn btn-secondary">New</router-link>
        </div>
        <div id="search-results" class="mt-3">{{ resultCountText }}</div>
        <div class="grid mt-4">
          <div
            v-for="groceryItem in listResponse.items"
            :key="groceryItem.id"
            class="card g-col-12 g-col-md-6"
          >
            <div class="card-header">
              <router-link :to="{ name: 'groceryItemEdit', params: { id: groceryItem.id } }">
                {{ groceryItem.name }}
              </router-link>
            </div>
            <div class="card-body">
              <div class="btn-toolbar d-none">
                <button
                  class="btn btn-sm btn-danger ms-auto"
                  @click="() => onDeleteGroceryItem(groceryItem.id)"
                >
                  Delete
                </button>
              </div>
              <div class="grid">
                <GroceryItemInventoryQuantity
                  :id="`${groceryItem.id}-inventoryQuantity`"
                  v-model="groceryItem.inventoryQuantity"
                  :item-id="groceryItem.id"
                  :inline="true"
                  class="g-col-12 g-col-sm-6 g-col-md-12 g-col-lg-6"
                />
                <div class="g-col-12 g-col-sm-6 g-col-md-12 g-col-lg-6">
                  Used in {{ groceryItem.recipeCount }} recipes.
                </div>
              </div>
              <div v-if="(groceryItem.storageLocations?.length || 0) > 0" class="mt-3">
                <TagBadge
                  v-for="location in groceryItem.storageLocations"
                  :key="location.name || ''"
                  class="me-2 mt-2"
                  :tag="{ name: location.name }"
                />
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
        />
      </div>
    </div>
  </div>
  <AppScrollToTop />
</template>

<style lang="scss" scoped></style>
