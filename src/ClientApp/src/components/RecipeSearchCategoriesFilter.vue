<script lang="ts" setup>
import type { PropType } from 'vue';
import type { ListCategoriesResponse, SearchFacetValue } from '@/api/data-contracts';
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
  type: Object as PropType<{ categories: Array<number>; matchAllCategories: boolean }>,
  required: true,
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const DISPLAY_LIMIT = 50;

const allCategories = ref([] as Array<ListCategoriesResponse>);
const filterText = ref('');

const categoryOptions = computed(() => {
  const text = filterText.value.trim().toLowerCase();
  const filtered = text
    ? allCategories.value.filter(x => x.name?.toLowerCase().includes(text))
    : allCategories.value;
  return filtered.slice(0, DISPLAY_LIMIT);
});

const categoriesOverLimit = computed(() => {
  const text = filterText.value.trim().toLowerCase();
  const total = text
    ? allCategories.value.filter(x => x.name?.toLowerCase().includes(text)).length
    : allCategories.value.length;
  return total > DISPLAY_LIMIT ? total : null;
});

function selectAll() {
  model.value.categories = allCategories.value.flatMap((x) => {
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
    .categoriesList({ isPagingEnabled: false })
    .then((response) => {
      allCategories.value = response.data.items || [];
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
        v-if="model.categories.length"
        class="btn btn-sm btn-secondary me-2"
        @click.stop.prevent="model.categories = []"
      >
        Select None
      </button>
      <button v-else class="btn btn-sm btn-secondary me-2" @click.stop.prevent="selectAll">
        Select All
      </button>
      <div class="form-check form-switch my-auto">
        <label class="w-100" for="matchAllCategories" aria-label="Match all selected categories">Match All</label>
        <input
          id="matchAllCategories"
          v-model="model.matchAllCategories"
          :checked="model.matchAllCategories"
          class="form-check-input"
          type="checkbox"
        >
      </div>
    </div>
    <div class="mb-2">
      <input
        id="categorySearch"
        v-model="filterText"
        type="search"
        class="form-control form-control-sm"
        placeholder="Filter categories..."
        aria-label="Filter categories"
      >
    </div>
    <div v-if="categoriesOverLimit" class="mb-2 text-muted small">
      Showing {{ DISPLAY_LIMIT }} of {{ categoriesOverLimit }} — refine filter to see more.
    </div>
    <div class="grid category-scroll">
      <div
        v-for="categoryOption in categoryOptions"
        :key="categoryOption.id"
        class="form-check m-0 g-col-12"
      >
        <input
          :id="`category-${categoryOption.id}`"
          v-model.lazy.number="model.categories"
          class="form-check-input"
          type="checkbox"
          :value="categoryOption.id"
        >
        <label class="form-check-label" :for="`category-${categoryOption.id}`">{{ categoryOption.name }}{{ getFacetCount(categoryOption.id) }}</label>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.category-scroll {
  row-gap: 0.1rem;
  column-gap: 0.1rem;
}
</style>
