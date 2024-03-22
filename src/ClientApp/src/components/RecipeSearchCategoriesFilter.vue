<script lang="ts" setup>
import ApiHelpers from '@/models/ApiHelpers';
import { ref, onMounted } from 'vue';
import useMessageStore from '@/stores/messageStore';
import { type ListCategoriesResponse, type RecipeSearchFacetValue } from '@/api/data-contracts';
import { toNumberOrNull } from '@/models/FormatHelpers';

const props = defineProps({
  facetValues: {
    type: Array<RecipeSearchFacetValue>,
    required: false,
    default: [],
  },
});

const model = defineModel({
  type: Array<number>,
  required: true,
  default: [],
});

const messageStore = useMessageStore();
const api = ApiHelpers.client;

const categoryOptions = ref([] as Array<ListCategoriesResponse>);

function selectAll() {
  model.value = categoryOptions.value.flatMap((x) => {
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
    .categoriesList({ isPagingEnabled: false })
    .then((response) => {
      categoryOptions.value = response.data.items || [];
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
});
</script>

<template>
  <div>
    <label for="categorySearch" class="form-label"
      >Categories
      <span v-if="model.length">({{ model.length }} selected)</span>
    </label>
    <div class="mb-2">
      <button
        v-if="model.length"
        class="btn btn-sm btn-outline-light btn-cling"
        @click.stop.prevent="model = []"
      >
        Select none
      </button>
      <button
        v-else
        class="btn btn-sm btn-outline-light btn-cling2"
        @click.stop.prevent="selectAll"
      >
        Select all
      </button>
    </div>
    <div class="grid category-facets-scroll">
      <div
        v-for="categoryOption in categoryOptions"
        :key="categoryOption.id"
        class="g-col-6 form-check m-0"
      >
        <input
          :id="`category-${categoryOption.id}`"
          v-model.lazy.number="model"
          class="form-check-input"
          type="checkbox"
          :value="categoryOption.id"
        />
        <label class="form-check-label" :for="`category-${categoryOption.id}`"
          >{{ categoryOption.name }}{{ getFacetCount(categoryOption.id) }}</label
        >
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.category-facets-scroll {
  max-height: 12rem;
  overflow-y: auto;
  row-gap: 0.1rem;
  column-gap: 0;

  &::-webkit-scrollbar {
    width: 1px;
    height: 36px;
  }

  &::-webkit-scrollbar-thumb {
    background-color: var(--bs-body-color);
    outline: 1px solid var(--bs-body-color);
    border-radius: 30px;
    height: 10px;
  }
}
</style>
