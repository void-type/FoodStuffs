<script lang="ts" setup>
import ApiHelper from '@/models/ApiHelper';
import { ref, onMounted, type PropType } from 'vue';
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
  type: Object as PropType<{ categories: Array<number>; allCategories: boolean }>,
  required: true,
  default: [],
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const categoryOptions = ref([] as Array<ListCategoriesResponse>);

function selectAll() {
  model.value.categories = categoryOptions.value.flatMap((x) => {
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
            <span v-if="model.categories.length">({{ model.categories.length }} selected)</span>
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
              v-if="model.categories.length"
              class="btn btn-sm btn-secondary me-2"
              @click.stop.prevent="model.categories = []"
            >
              Select none
            </button>
            <button v-else class="btn btn-sm btn-secondary me-2" @click.stop.prevent="selectAll">
              Select all
            </button>
            <div class="form-check form-switch my-auto">
              <label class="w-100" for="allCategories" aria-label="Match all selected categories"
                >Match all selected</label
              >
              <input
                id="allCategories"
                v-model="model.allCategories"
                :checked="model.allCategories"
                class="form-check-input"
                type="checkbox"
              />
            </div>
          </div>
          <div class="grid slim-scroll category-scroll">
            <div
              v-for="categoryOption in categoryOptions"
              :key="categoryOption.id"
              class="g-col-6 g-col-md-4 form-check m-0"
            >
              <input
                :id="`category-${categoryOption.id}`"
                v-model.lazy.number="model.categories"
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
