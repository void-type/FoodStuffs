<script lang="ts" setup>
import ApiHelpers from '@/models/ApiHelpers';
import { ref, onMounted } from 'vue';
import useMessageStore from '@/stores/messageStore';
import type { ListCategoriesResponse } from '@/api/data-contracts';

const model = defineModel({
  type: Array<number>,
  required: true,
  default: [],
});

const messageStore = useMessageStore();
const api = ApiHelpers.client;

const categoryOptions = ref([] as Array<ListCategoriesResponse>);

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
    <button class="btn btn-sm btn-outline-light btn-cling ms-3" @click.stop.prevent="model = []">
      Clear
    </button>
    <div class="grid py-2 category-facets-scroll">
      <div
        v-for="categoryOption in categoryOptions"
        :key="categoryOption.id"
        class="g-col-6 form-check"
      >
        <input
          :id="`category-${categoryOption.id}`"
          v-model.lazy.number="model"
          class="form-check-input"
          type="checkbox"
          :value="categoryOption.id"
        />
        <label class="form-check-label" :for="`category-${categoryOption.id}`">{{
          categoryOption.name
        }}</label>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
