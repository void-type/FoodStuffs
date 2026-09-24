<script lang="ts" setup>
import type { PropType } from 'vue';
import type { ListGroceryAislesResponse, SearchFacetValue } from '@/api/data-contracts';
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
  tabPaneId: {
    type: String,
    required: true,
  },
  active: {
    type: Boolean,
    required: false,
    default: false,
  },
});

const model = defineModel({
  type: Object as PropType<{ groceryAisles: Array<number> }>,
  required: true,
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

  const count = props.facetValues?.find(x => x.fieldValue === facetValue.toString())?.count || 0;

  return ` (${count})`;
}

onMounted(() => {
  api()
    .groceryAislesList({ isPagingEnabled: false })
    .then((response) => {
      groceryAisleOptions.value = response.data.items || [];
    })
    .catch(response => messageStore.setApiFailureMessages(response));
});
</script>

<template>
  <div
    :id="tabPaneId"
    class="tab-pane fade"
    :class="{ 'show active': active }"
    role="tabpanel"
    tabindex="0"
  >
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
        class="form-check m-0 g-col-12"
      >
        <input
          :id="`groceryAisle-${groceryAisleOption.id}`"
          v-model.lazy.number="model.groceryAisles"
          class="form-check-input"
          type="checkbox"
          :value="groceryAisleOption.id"
        >
        <label class="form-check-label" :for="`groceryAisle-${groceryAisleOption.id}`">
          {{ groceryAisleOption.name }}{{ getFacetCount(groceryAisleOption.id) }}
        </label>
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
