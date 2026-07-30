<script lang="ts" setup>
import type { PropType } from 'vue';
import type { SearchFacetValue } from '@/api/data-contracts';
import { computed, onMounted, ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import { toNumberOrNull } from '@/models/FormatHelper';
import useMessageStore from '@/stores/messageStore';

const props = defineProps({
  facetValues: {
    type: Array<SearchFacetValue>,
    required: false,
    default: [],
  },
  parentAccordionId: {
    type: String,
    required: false,
    default: 'filterAccordion',
  },
  checkClass: {
    type: String,
    required: false,
    default: 'g-col-12 g-col-md-6 g-col-lg-4',
  },
});

const model = defineModel({
  type: Object as PropType<{ groceryItemIds: Array<number>; matchAllGroceryItems: boolean }>,
  required: true,
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

interface GroceryItemOption {
  id: number;
  name: string;
}

const DISPLAY_LIMIT = 50;

const allGroceryItems = ref([] as Array<GroceryItemOption>);
const filterText = ref('');

const groceryItemOptions = computed(() => {
  const text = filterText.value.trim().toLowerCase();
  const filtered = text
    ? allGroceryItems.value.filter(x => x.name.toLowerCase().includes(text))
    : allGroceryItems.value;
  return filtered.slice(0, DISPLAY_LIMIT);
});

const groceryItemsOverLimit = computed(() => {
  const text = filterText.value.trim().toLowerCase();
  const total = text
    ? allGroceryItems.value.filter(x => x.name.toLowerCase().includes(text)).length
    : allGroceryItems.value.length;
  return total > DISPLAY_LIMIT ? total : null;
});

function selectAll() {
  model.value.groceryItemIds = allGroceryItems.value.flatMap((x) => {
    const n = toNumberOrNull(x.id);
    return n ? [n] : [];
  });
}

function getFacetCount(facetValue: number | null | undefined) {
  if (facetValue === null || typeof facetValue === 'undefined') {
    return null;
  }

  const count = props.facetValues?.find(x => x.fieldValue === facetValue.toString())?.count || 0;

  return ` (${count})`;
}

onMounted(() => {
  api()
    .groceryItemsSearch({ isPagingEnabled: false })
    .then((response) => {
      allGroceryItems.value = (response.data.results?.items || [])
        .flatMap(x => (x.id && x.name ? [{ id: x.id, name: x.name }] : []))
        .sort((a, b) => a.name.localeCompare(b.name));
    })
    .catch(response => messageStore.setApiFailureMessages(response));
});
</script>

<template>
  <div class="accordion-item">
    <div class="accordion-header">
      <button
        class="accordion-button collapsed px-3 py-2"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#groceryItemsCollapse"
        aria-expanded="false"
        aria-controls="groceryItemsCollapse"
      >
        <label for="groceryItemSearch">Grocery Items
          <span v-if="model.groceryItemIds.length">
            ({{ model.groceryItemIds.length }})
          </span>
        </label>
      </button>
    </div>
    <div
      id="groceryItemsCollapse"
      class="accordion-collapse collapse"
      :data-bs-parent="`#${props.parentAccordionId}`"
    >
      <div class="accordion-body">
        <div class="btn-toolbar mb-3">
          <button
            v-if="model.groceryItemIds.length"
            class="btn btn-sm btn-secondary me-2"
            @click.stop.prevent="model.groceryItemIds = []"
          >
            Select None
          </button>
          <button v-else class="btn btn-sm btn-secondary me-2" @click.stop.prevent="selectAll">
            Select All
          </button>
          <div class="form-check form-switch my-auto">
            <label class="w-100" for="matchAllGroceryItems" aria-label="Match all selected grocery items">Match All</label>
            <input
              id="matchAllGroceryItems"
              v-model="model.matchAllGroceryItems"
              :checked="model.matchAllGroceryItems"
              class="form-check-input"
              type="checkbox"
            >
          </div>
        </div>
        <div class="mb-2">
          <input
            id="groceryItemSearch"
            v-model="filterText"
            type="search"
            class="form-control form-control-sm"
            placeholder="Filter grocery items..."
            aria-label="Filter grocery items"
          >
        </div>
        <div v-if="groceryItemsOverLimit" class="mb-2 text-muted small">
          Showing {{ DISPLAY_LIMIT }} of {{ groceryItemsOverLimit }} — refine filter to see more.
        </div>
        <div class="grid grocery-item-scroll">
          <div
            v-for="item in groceryItemOptions"
            :key="item.id"
            class="form-check m-0"
            :class="checkClass"
          >
            <input
              :id="`grocery-item-${item.id}`"
              v-model.lazy.number="model.groceryItemIds"
              class="form-check-input"
              type="checkbox"
              :value="item.id"
            >
            <label class="form-check-label" :for="`grocery-item-${item.id}`">{{ item.name }}{{ getFacetCount(item.id) }}</label>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.grocery-item-scroll {
  row-gap: 0.1rem;
  column-gap: 0.1rem;
}
</style>
