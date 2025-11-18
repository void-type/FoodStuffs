<script lang="ts" setup>
import ApiHelper from '@/models/ApiHelper';
import { ref, onMounted, type PropType } from 'vue';
import useMessageStore from '@/stores/messageStore';
import { type ListGroceryAislesResponse, type SearchFacetValue } from '@/api/data-contracts';
import { toNumberOrNull } from '@/models/FormatHelper';

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
  type: Object as PropType<{ groceryAisles: Array<number> }>,
  required: true,
  default: [],
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const groceryAisleOptions = ref([] as Array<ListGroceryAislesResponse>);

function selectAll() {
  model.value.groceryAisles = groceryAisleOptions.value.flatMap((x) => {
    const n = toNumberOrNull(x.id);
    return n ? [n] : [];
  });
}

function getFacetCount(facetValue: number | null | undefined) {
  if (facetValue === null || typeof facetValue === 'undefined') {
    return null;
  }

  const count = props.facetValues?.find((x) => x.fieldValue === facetValue.toString())?.count || 0;

  return ` (${count})`;
}

onMounted(() => {
  api()
    .groceryAislesList({ isPagingEnabled: false })
    .then((response) => {
      groceryAisleOptions.value = response.data.items || [];
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
});
</script>

<template>
  <div class="accordion-item">
    <div class="accordion-header">
      <button
        class="accordion-button collapsed"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#groceryAislesCollapse"
        aria-expanded="false"
        aria-controls="groceryAislesCollapse"
      >
        <label for="groceryAisleSearch"
          >Grocery Aisles
          <span v-if="model.groceryAisles.length">
            ({{ model.groceryAisles.length }} selected)
          </span>
        </label>
      </button>
    </div>
    <div
      id="groceryAislesCollapse"
      class="accordion-collapse collapse"
      :data-bs-parent="`#${props.parentAccordionId}`"
    >
      <div class="accordion-body">
        <div class="btn-toolbar mb-3">
          <button
            v-if="model.groceryAisles.length"
            class="btn btn-sm btn-secondary me-2"
            @click.stop.prevent="model.groceryAisles = []"
          >
            Select None
          </button>
          <button v-else class="btn btn-sm btn-secondary me-2" @click.stop.prevent="selectAll">
            Select All
          </button>
        </div>
        <div class="grid slim-scroll grocery-aisle-scroll">
          <div
            v-for="groceryAisleOption in groceryAisleOptions"
            :key="groceryAisleOption.id"
            :class="`${checkClass} form-check m-0`"
          >
            <input
              :id="`groceryAisle-${groceryAisleOption.id}`"
              v-model.lazy.number="model.groceryAisles"
              class="form-check-input"
              type="checkbox"
              :value="groceryAisleOption.id"
            />
            <label class="form-check-label" :for="`groceryAisle-${groceryAisleOption.id}`">
              {{ groceryAisleOption.name }}{{ getFacetCount(groceryAisleOption.id) }}
            </label>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.grocery-aisle-scroll {
  row-gap: 0.1rem;
  column-gap: 0;
}
</style>
