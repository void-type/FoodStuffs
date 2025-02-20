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
  <div id="categoriesAccordion" class="accordion">
    <div class="accordion-item">
      <h2 class="accordion-header">
        <button
          class="accordion-button collapsed"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#categoriesCollapse"
          aria-expanded="false"
          aria-controls="categoriesCollapse"
        >
          <label for="categorySearch"
            >Categories
            <span v-if="model.length">({{ model.length }} selected)</span>
          </label>
        </button>
      </h2>
      <div
        id="categoriesCollapse"
        class="accordion-collapse collapse"
        data-bs-parent="#categoriesAccordion"
      >
        <div class="accordion-body">
          <div class="btn-toolbar mb-3">
            <button
              v-if="model.length"
              class="btn btn-sm btn-secondary"
              @click.stop.prevent="model = []"
            >
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
              class="g-col-6 g-col-md-4 form-check m-0"
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
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.category-scroll {
  row-gap: 0.1rem;
  column-gap: 0;
}
</style>
