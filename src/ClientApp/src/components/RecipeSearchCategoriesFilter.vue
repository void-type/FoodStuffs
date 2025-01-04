<script lang="ts" setup>
import ApiHelper from '@/models/ApiHelper';
import { ref, onMounted } from 'vue';
import useMessageStore from '@/stores/messageStore';
import { type ListCategoriesResponse, type SearchFacetValue } from '@/api/data-contracts';
import { toNumberOrNull } from '@/models/FormatHelper';

const props = defineProps({
  facetValues: {
    type: Array<SearchFacetValue>,
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
const api = ApiHelper.client;

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
      <button v-if="model.length" class="btn btn-sm btn-secondary" @click.stop.prevent="model = []">
        Select none
      </button>
      <button v-else class="btn btn-sm btn-secondary" @click.stop.prevent="selectAll">
        Select all
      </button>
    </div>
    <div class="grid slim-scroll category-scroll">
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
.category-scroll {
  max-height: 7rem;
  row-gap: 0.1rem;
  column-gap: 0;
}
</style>
