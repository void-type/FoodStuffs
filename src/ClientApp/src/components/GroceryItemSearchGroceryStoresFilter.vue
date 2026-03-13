<script lang="ts" setup>
import type { PropType } from 'vue';
import type { ListGroceryStoresResponse, SearchFacetValue } from '@/api/data-contracts';
import { onMounted, ref } from 'vue';
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
    default: 'g-col-6 g-col-md-4',
  },
});

const model = defineModel({
  type: Object as PropType<{ groceryStores: Array<number>; matchAllGroceryStores: boolean }>,
  required: true,
  default: [],
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const groceryStoreOptions = ref([] as Array<ListGroceryStoresResponse>);

function selectAll() {
  model.value.groceryStores = groceryStoreOptions.value.flatMap((x) => {
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
    .groceryStoresList({ isPagingEnabled: false })
    .then((response) => {
      groceryStoreOptions.value = response.data.items || [];
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
        data-bs-target="#groceryStoresCollapse"
        aria-expanded="false"
        aria-controls="groceryStoresCollapse"
      >
        <label for="groceryStoreSearch">Grocery Stores
          <span v-if="model.groceryStores.length">
            ({{ model.groceryStores.length }})
          </span>
        </label>
      </button>
    </div>
    <div
      id="groceryStoresCollapse"
      class="accordion-collapse collapse"
      :data-bs-parent="`#${props.parentAccordionId}`"
    >
      <div class="accordion-body">
        <div class="btn-toolbar mb-3">
          <button
            v-if="model.groceryStores.length"
            class="btn btn-sm btn-secondary me-2"
            @click.stop.prevent="model.groceryStores = []"
          >
            Select None
          </button>
          <button v-else class="btn btn-sm btn-secondary me-2" @click.stop.prevent="selectAll">
            Select All
          </button>
          <div class="form-check form-switch my-auto">
            <label
              class="w-100"
              for="matchAllGroceryStores"
              aria-label="Match all selected grocery stores"
            >Match All</label>
            <input
              id="matchAllGroceryStores"
              v-model="model.matchAllGroceryStores"
              :checked="model.matchAllGroceryStores"
              class="form-check-input"
              type="checkbox"
            >
          </div>
        </div>
        <div class="grid slim-scroll grocery-store-scroll">
          <div
            v-for="groceryStoreOption in groceryStoreOptions"
            :key="groceryStoreOption.id"
            :class="`${checkClass} form-check m-0`"
          >
            <input
              :id="`groceryStore-${groceryStoreOption.id}`"
              v-model.lazy.number="model.groceryStores"
              class="form-check-input"
              type="checkbox"
              :value="groceryStoreOption.id"
            >
            <label class="form-check-label" :for="`groceryStore-${groceryStoreOption.id}`">
              {{ groceryStoreOption.name }}{{ getFacetCount(groceryStoreOption.id) }}
            </label>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.grocery-store-scroll {
  row-gap: 0.1rem;
  column-gap: 0;
}
</style>
