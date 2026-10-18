<script lang="ts" setup>
import type { PropType } from 'vue';
import type { LocationQuery } from 'vue-router';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { storeToRefs } from 'pinia';
import { computed, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppPageHeading from '@/components/AppPageHeading.vue';
import AppScrollToTop from '@/components/AppScrollToTop.vue';
import AppSearchInput from '@/components/AppSearchInput.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';
import GroceryItemInventoryQuantity from '@/components/GroceryItemInventoryQuantity.vue';
import GroceryItemSearchGroceryAislesFilter from '@/components/GroceryItemSearchGroceryAislesFilter.vue';
import GroceryItemSearchGroceryStoresFilter from '@/components/GroceryItemSearchGroceryStoresFilter.vue';
import GroceryItemSearchStorageLocationsFilter from '@/components/GroceryItemSearchStorageLocationsFilter.vue';
import TagBadge from '@/components/TagBadge.vue';
import Choices from '@/models/Choices';
import { toInt, toNumber, toNumberOrNull } from '@/models/FormatHelper';
import GroceryItemsSearchRequest from '@/models/GroceryItemsSearchRequest';
import RouterHelper from '@/models/RouterHelper';
import useAppStore from '@/stores/appStore';
import useGroceryItemStore from '@/stores/groceryItemStore';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const groceryItemStore = useGroceryItemStore();
const router = useRouter();
const route = useRoute();

const { useDarkMode } = storeToRefs(appStore);
const { listResponse, listRequest, listFacets } = storeToRefs(groceryItemStore);
const { sortOptions } = Choices;

const storageLocationsFilterModel = ref({
  storageLocations: [] as Array<number>,
  matchAllStorageLocations: false,
});

const groceryAislesFilterModel = ref({
  groceryAisles: [] as Array<number>,
});

const groceryStoresFilterModel = ref({
  groceryStores: [] as Array<number>,
  matchAllGroceryStores: false,
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

function resetSearch() {
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
  const storageLocations
    = props.query.storageLocations
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  const groceryStores
    = props.query.groceryStores
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  const groceryAisles
    = props.query.groceryAisles
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  storageLocationsFilterModel.value.storageLocations = storageLocations;
  storageLocationsFilterModel.value.matchAllStorageLocations
    = props.query.matchAllStorageLocations === 'true';

  groceryStoresFilterModel.value.groceryStores = groceryStores;
  groceryStoresFilterModel.value.matchAllGroceryStores
    = props.query.matchAllGroceryStores === 'true';

  groceryAislesFilterModel.value.groceryAisles = groceryAisles;

  groceryItemStore.setListRequest({
    ...new GroceryItemsSearchRequest(),
    ...props.query,
    storageLocations,
    groceryStores,
    groceryAisles,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  });
}

const storageLocationFacets = computed(() => {
  return listFacets.value.find(x => x.fieldName === 'StorageLocations')?.values || [];
});

const groceryStoreFacets = computed(() => {
  return listFacets.value.find(x => x.fieldName === 'GroceryStores')?.values || [];
});

const groceryAisleFacets = computed(() => {
  return listFacets.value.find(x => x.fieldName === 'GroceryAisle')?.values || [];
});

const unusedFilterText = computed(() => {
  const choiceString = Choices.getBooleanChoiceText(listRequest.value.isUnused);
  return choiceString === 'All' ? '' : ` (${choiceString})`;
});

const outOfStockFilterText = computed(() => {
  const choiceString = Choices.getBooleanChoiceText(listRequest.value.isOutOfStock);
  return choiceString === 'All' ? '' : ` (${choiceString})`;
});

const activeFilterCount = computed(() => {
  let count = 0;

  if (listRequest.value.isUnused !== null && typeof listRequest.value.isUnused !== 'undefined') {
    count += 1;
  }

  if (listRequest.value.isOutOfStock !== null && typeof listRequest.value.isOutOfStock !== 'undefined') {
    count += 1;
  }

  if (storageLocationsFilterModel.value.storageLocations.length > 0) {
    count += 1;
  }

  if (groceryStoresFilterModel.value.groceryStores.length > 0) {
    count += 1;
  }

  if (groceryAislesFilterModel.value.groceryAisles.length > 0) {
    count += 1;
  }

  return count;
});

function getOutOfStockFacetCount(facetValue: boolean | null) {
  if (facetValue == null) {
    return null;
  }

  const count
    = groceryItemStore.listFacets
      .find(x => x.fieldName === 'IsOutOfStock')
      ?.values
      ?.find(x => x.fieldValue?.toLowerCase() === facetValue.toString().toLowerCase())
      ?.count || 0;

  return ` (${count})`;
}

function getUnusedFacetCount(facetValue: boolean | null) {
  if (facetValue == null) {
    return null;
  }

  const count
    = groceryItemStore.listFacets
      .find(x => x.fieldName === 'IsUnused')
      ?.values
      ?.find(x => x.fieldValue?.toLowerCase() === facetValue.toString().toLowerCase())
      ?.count || 0;

  return ` (${count})`;
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
  { deep: true },
);

watch(
  groceryStoresFilterModel,
  () => {
    const { groceryStores, matchAllGroceryStores } = listRequest.value;

    const initialModel = {
      groceryStores,
      matchAllGroceryStores,
    };

    if (JSON.stringify(initialModel) !== JSON.stringify(groceryStoresFilterModel.value)) {
      groceryItemStore.setListRequest({
        ...listRequest.value,
        ...groceryStoresFilterModel.value,
        page: 1,
      });

      navigateSearch(false);
    }
  },
  { deep: true },
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
  { deep: true },
);

watch(
  props,
  async () => {
    setListRequestFromQuery();
    await groceryItemStore.fetchGroceryItemsList();
  },
  { immediate: true },
);
</script>

<template>
  <div class="container-xxl">
    <AppPageHeading />
    <div id="skip-filters" class="container-xxl visually-hidden-focusable">
      <router-link
        class="d-inline-flex p-2 m-1"
        :to="{ hash: '#search-results', query: route.query }"
      >
        Skip to search results
      </router-link>
    </div>

    <!-- Two column layout -->
    <div class="search-page-grid mt-3">
      <!-- Left rail filters - desktop only -->
      <div class="d-none d-lg-block">
        <div class="mb-3">
          <span id="isOutOfStockLabelDesktop" class="form-label d-block mb-1">Out of Stock{{ outOfStockFilterText }}</span>
          <div class="btn-group" role="group" aria-labelledby="isOutOfStockLabelDesktop">
            <template v-for="option in Choices.boolean" :key="option.value?.toString()">
              <input
                :id="`isOutOfStockDesktop-${option.value}`"
                v-model="listRequest.isOutOfStock"
                class="btn-check"
                type="radio"
                name="isOutOfStockDesktop"
                autocomplete="off"
                :value="option.value"
                @change="startSearchNoHash"
              >
              <label class="btn btn-outline-secondary btn-sm" :for="`isOutOfStockDesktop-${option.value}`">
                {{ option.text }}{{ getOutOfStockFacetCount(option.value) }}
              </label>
            </template>
          </div>
        </div>
        <div class="mb-3">
          <span id="isUnusedLabelDesktop" class="form-label d-block mb-1">Unused{{ unusedFilterText }}</span>
          <div class="btn-group" role="group" aria-labelledby="isUnusedLabelDesktop">
            <template v-for="option in Choices.boolean" :key="option.value?.toString()">
              <input
                :id="`isUnusedDesktop-${option.value}`"
                v-model="listRequest.isUnused"
                class="btn-check"
                type="radio"
                name="isUnusedDesktop"
                autocomplete="off"
                :value="option.value"
                @change="startSearchNoHash"
              >
              <label class="btn btn-outline-secondary btn-sm" :for="`isUnusedDesktop-${option.value}`">
                {{ option.text }}{{ getUnusedFacetCount(option.value) }}
              </label>
            </template>
          </div>
        </div>
        <div>
          <ul id="filterTabsDesktop" class="nav nav-tabs nav-justified" role="tablist" aria-label="Filters">
            <li class="nav-item" role="presentation">
              <button
                id="storageLocationsTabDesktop"
                class="nav-link active"
                data-bs-toggle="tab"
                data-bs-target="#storageLocationsTabPaneDesktop"
                type="button"
                role="tab"
                aria-controls="storageLocationsTabPaneDesktop"
                aria-selected="true"
              >
                Storage Locations<span v-if="storageLocationsFilterModel.storageLocations.length"> ({{ storageLocationsFilterModel.storageLocations.length }})</span>
              </button>
            </li>
            <li class="nav-item" role="presentation">
              <button
                id="groceryStoresTabDesktop"
                class="nav-link"
                data-bs-toggle="tab"
                data-bs-target="#groceryStoresTabPaneDesktop"
                type="button"
                role="tab"
                aria-controls="groceryStoresTabPaneDesktop"
                aria-selected="false"
              >
                Grocery Stores<span v-if="groceryStoresFilterModel.groceryStores.length"> ({{ groceryStoresFilterModel.groceryStores.length }})</span>
              </button>
            </li>
            <li class="nav-item" role="presentation">
              <button
                id="groceryAislesTabDesktop"
                class="nav-link"
                data-bs-toggle="tab"
                data-bs-target="#groceryAislesTabPaneDesktop"
                type="button"
                role="tab"
                aria-controls="groceryAislesTabPaneDesktop"
                aria-selected="false"
              >
                Grocery Aisles<span v-if="groceryAislesFilterModel.groceryAisles.length"> ({{ groceryAislesFilterModel.groceryAisles.length }})</span>
              </button>
            </li>
          </ul>
          <div class="tab-content border border-top-0 rounded-bottom p-3">
            <GroceryItemSearchStorageLocationsFilter
              v-model="storageLocationsFilterModel"
              :facet-values="storageLocationFacets"
              tab-pane-id="storageLocationsTabPaneDesktop"
              active
            />
            <GroceryItemSearchGroceryStoresFilter
              v-model="groceryStoresFilterModel"
              :facet-values="groceryStoreFacets"
              tab-pane-id="groceryStoresTabPaneDesktop"
            />
            <GroceryItemSearchGroceryAislesFilter
              v-model="groceryAislesFilterModel"
              :facet-values="groceryAisleFacets"
              tab-pane-id="groceryAislesTabPaneDesktop"
            />
          </div>
        </div>
      </div>

      <!-- Main content area -->
      <div>
        <div class="grid mb-3 gap-sm">
          <div class="g-col-12 g-col-lg-9 d-flex gap-2">
            <div class="flex-grow-1">
              <label for="searchText" class="form-label visually-hidden">Search</label>
              <AppSearchInput
                id="searchText"
                v-model="listRequest.searchText"
                type="search"
                inputmode="search"
                enterkeyhint="search"
                placeholder="Search..."
                @keydown.stop.prevent.enter="startSearch"
                @clear="startSearch"
              />
            </div>
            <button
              class="btn btn-outline-secondary flex-shrink-0 d-lg-none position-relative"
              type="button"
              data-bs-toggle="offcanvas"
              data-bs-target="#groceryItemSearchOptions"
              aria-controls="groceryItemSearchOptions"
              aria-label="Sort and Filters"
            >
              <FontAwesomeIcon icon="fa-filter" />
              <span
                v-if="activeFilterCount > 0"
                class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-primary"
              >
                {{ activeFilterCount }}
                <span class="visually-hidden">active filters</span>
              </span>
            </button>
          </div>
          <div class="g-col-12 g-col-lg-3 d-none d-lg-block">
            <label for="groceryItemSort" class="form-label visually-hidden">Sort</label>
            <select
              id="groceryItemSort"
              :value="listRequest.sortBy"
              name="groceryItemSort"
              class="form-select"
              aria-label="Sort options"
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
        </div>

        <div class="btn-toolbar">
          <button class="btn btn-primary me-2" type="button" @click.stop.prevent="startSearch()">
            Search
          </button>
          <button class="btn btn-secondary me-2" type="button" @click.stop.prevent="resetSearch()">
            Reset
          </button>
          <router-link :to="{ name: 'groceryItemNew' }" class="btn btn-secondary">
            New
          </router-link>
        </div>

        <Teleport to="body">
          <div
            id="groceryItemSearchOptions"
            class="offcanvas offcanvas-end"
            tabindex="-1"
            aria-labelledby="groceryItemSearchOptionsLabel"
          >
            <div class="offcanvas-header">
              <h5 id="groceryItemSearchOptionsLabel" class="offcanvas-title">
                Sort &amp; Filters
              </h5>
              <button
                type="button"
                class="btn-close"
                data-bs-dismiss="offcanvas"
                aria-label="Close"
              />
            </div>
            <div class="offcanvas-body">
              <div class="mb-3">
                <label for="groceryItemSortMobile" class="form-label">Sort</label>
                <select
                  id="groceryItemSortMobile"
                  :value="listRequest.sortBy"
                  name="groceryItemSortMobile"
                  class="form-select"
                  aria-label="Sort options"
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

              <!-- Mobile filters - only visible on screens smaller than lg, otherwise shown in the left rail -->
              <div class="d-lg-none">
                <div class="mb-3">
                  <span id="isOutOfStockLabel" class="form-label d-block mb-1">Out of Stock{{ outOfStockFilterText }}</span>
                  <div class="btn-group" role="group" aria-labelledby="isOutOfStockLabel">
                    <template v-for="option in Choices.boolean" :key="option.value?.toString()">
                      <input
                        :id="`isOutOfStock-${option.value}`"
                        v-model="listRequest.isOutOfStock"
                        class="btn-check"
                        type="radio"
                        name="isOutOfStock"
                        autocomplete="off"
                        :value="option.value"
                        @change="startSearchNoHash"
                      >
                      <label class="btn btn-outline-secondary btn-sm" :for="`isOutOfStock-${option.value}`">
                        {{ option.text }}{{ getOutOfStockFacetCount(option.value) }}
                      </label>
                    </template>
                  </div>
                </div>
                <div class="mb-3">
                  <span id="isUnusedLabel" class="form-label d-block mb-1">Unused{{ unusedFilterText }}</span>
                  <div class="btn-group" role="group" aria-labelledby="isUnusedLabel">
                    <template v-for="option in Choices.boolean" :key="option.value?.toString()">
                      <input
                        :id="`isUnused-${option.value}`"
                        v-model="listRequest.isUnused"
                        class="btn-check"
                        type="radio"
                        name="isUnused"
                        autocomplete="off"
                        :value="option.value"
                        @change="startSearchNoHash"
                      >
                      <label class="btn btn-outline-secondary btn-sm" :for="`isUnused-${option.value}`">
                        {{ option.text }}{{ getUnusedFacetCount(option.value) }}
                      </label>
                    </template>
                  </div>
                </div>
                <ul id="filterTabsMobile" class="nav nav-tabs nav-justified" role="tablist" aria-label="Filters">
                  <li class="nav-item" role="presentation">
                    <button
                      id="storageLocationsTabMobile"
                      class="nav-link active"
                      data-bs-toggle="tab"
                      data-bs-target="#storageLocationsTabPaneMobile"
                      type="button"
                      role="tab"
                      aria-controls="storageLocationsTabPaneMobile"
                      aria-selected="true"
                    >
                      Storage Locations<span v-if="storageLocationsFilterModel.storageLocations.length"> ({{ storageLocationsFilterModel.storageLocations.length }})</span>
                    </button>
                  </li>
                  <li class="nav-item" role="presentation">
                    <button
                      id="groceryStoresTabMobile"
                      class="nav-link"
                      data-bs-toggle="tab"
                      data-bs-target="#groceryStoresTabPaneMobile"
                      type="button"
                      role="tab"
                      aria-controls="groceryStoresTabPaneMobile"
                      aria-selected="false"
                    >
                      Grocery Stores<span v-if="groceryStoresFilterModel.groceryStores.length"> ({{ groceryStoresFilterModel.groceryStores.length }})</span>
                    </button>
                  </li>
                  <li class="nav-item" role="presentation">
                    <button
                      id="groceryAislesTabMobile"
                      class="nav-link"
                      data-bs-toggle="tab"
                      data-bs-target="#groceryAislesTabPaneMobile"
                      type="button"
                      role="tab"
                      aria-controls="groceryAislesTabPaneMobile"
                      aria-selected="false"
                    >
                      Grocery Aisles<span v-if="groceryAislesFilterModel.groceryAisles.length"> ({{ groceryAislesFilterModel.groceryAisles.length }})</span>
                    </button>
                  </li>
                </ul>
                <div class="tab-content border border-top-0 rounded-bottom p-3">
                  <GroceryItemSearchStorageLocationsFilter
                    v-model="storageLocationsFilterModel"
                    :facet-values="storageLocationFacets"
                    tab-pane-id="storageLocationsTabPaneMobile"
                    active
                  />
                  <GroceryItemSearchGroceryStoresFilter
                    v-model="groceryStoresFilterModel"
                    :facet-values="groceryStoreFacets"
                    tab-pane-id="groceryStoresTabPaneMobile"
                  />
                  <GroceryItemSearchGroceryAislesFilter
                    v-model="groceryAislesFilterModel"
                    :facet-values="groceryAisleFacets"
                    tab-pane-id="groceryAislesTabPaneMobile"
                  />
                </div>
              </div>
            </div>
          </div>
        </Teleport>

        <div id="search-results" class="mt-3">
          <small class="text-muted">{{ resultCountText }}</small>
        </div>
        <table
          v-if="(listResponse.items?.length || 0) > 0"
          class="table table-grocery-items align-middle mt-4" :class="{ 'table-dark': useDarkMode }"
        >
          <thead>
            <tr>
              <th scope="col">
                Name
              </th>
              <th scope="col">
                Inventory
              </th>
              <th scope="col">
                Recipes
              </th>
              <th scope="col">
                Location, store &amp; aisle
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="groceryItem in listResponse.items" :key="groceryItem.id">
              <td class="cell-name">
                <router-link :to="RouterHelper.editGroceryItem(groceryItem)">
                  {{ groceryItem.name }}
                </router-link>
              </td>
              <td class="cell-inventory">
                <GroceryItemInventoryQuantity
                  :id="`${groceryItem.id}-inventoryQuantity`"
                  v-model="groceryItem.inventoryQuantity"
                  :item-id="groceryItem.id"
                  compact
                />
              </td>
              <td class="cell-recipes">
                {{ groceryItem.recipeCount }}<span class="d-md-none"> recipe{{ groceryItem.recipeCount === 1 ? '' : 's' }}</span>
              </td>
              <td class="cell-tags">
                <div class="d-flex flex-wrap gap-1">
                  <TagBadge
                    v-for="location in groceryItem.storageLocations"
                    :key="`location-${location.id}`"
                    :tag="{ name: location.name }"
                    :to="RouterHelper.searchGroceryItemsByStorageLocation(location)"
                  />
                  <TagBadge
                    v-for="store in groceryItem.groceryStores"
                    :key="`store-${store.id}`"
                    :tag="{ name: store.name }"
                    :to="RouterHelper.searchGroceryItemsByGroceryStore(store)"
                  />
                  <TagBadge
                    v-if="groceryItem.groceryAisle"
                    :tag="{ name: groceryItem.groceryAisle.name }"
                    :to="RouterHelper.searchGroceryItemsByGroceryAisle(groceryItem.groceryAisle)"
                  />
                </div>
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
        />
      </div>
    </div>
  </div>
  <AppScrollToTop />
</template>

<style lang="scss" scoped>
// Below md each row becomes a stacked block: name and inventory on top, recipe count, then the
// location/store/aisle badges across the full width.
@media (max-width: 767.98px) {
  .table-grocery-items {
    thead {
      display: none;
    }

    tbody tr {
      display: grid;
      grid-template-columns: 1fr auto;
      grid-template-areas:
        'name inventory'
        'recipes inventory'
        'tags tags';
      column-gap: 0.75rem;
      align-items: center;
      padding: 0.5rem 0;
      border-bottom: var(--bs-border-width) solid var(--bs-table-border-color);
    }

    td {
      padding: 0.125rem 0;
      border-bottom-width: 0;
      box-shadow: none;
    }

    .cell-name {
      grid-area: name;
      font-weight: 500;
    }

    .cell-inventory {
      grid-area: inventory;
    }

    .cell-recipes {
      grid-area: recipes;
      color: var(--bs-secondary-color);
      font-size: 0.875rem;
    }

    .cell-tags {
      grid-area: tags;
      padding-top: 0.5rem;

      &:has(> div:empty) {
        display: none;
      }
    }
  }
}
</style>
