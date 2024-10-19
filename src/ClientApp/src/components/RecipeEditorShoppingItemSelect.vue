<script lang="ts" setup>
import { ref, computed, defineProps, type PropType } from 'vue';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import type WorkingRecipeShoppingItem from '@/models/WorkingRecipeShoppingItem';

const model = defineModel({
  type: Number as PropType<number | undefined>,
  required: true,
});

const props = defineProps({
  item: {
    type: Object as PropType<WorkingRecipeShoppingItem>,
    required: true,
  },
  itemName: {
    type: String,
    required: false,
    default: '',
  },
  suggestions: {
    type: Array as PropType<Array<ListShoppingItemsResponse>>,
    required: true,
  },
  usedIds: {
    type: Array as PropType<Array<number | undefined>>,
    required: true,
  },
});

const filterText = ref('');

const filteredSuggestions = computed(() => {
  const { usedIds } = props;

  const filterTextLow = filterText.value.toLowerCase();

  return props.suggestions.filter(
    (x) =>
      // it is the current item or (id isn't used already, and the name contains the filter text).
      x.id === props.item.id ||
      (!usedIds.includes(x.id) && x.name?.toLowerCase().includes(filterTextLow))
  );
});

function selectSuggestion(id: number | undefined) {
  filterText.value = '';
  model.value = id;
}
</script>

<template>
  <div class="dropdown">
    <button class="form-select" type="button" data-bs-toggle="dropdown" aria-expanded="false">
      {{ (item.id || 0) > 0 ? itemName : 'Select one' }}
    </button>
    <ul class="dropdown-menu w-100 d-relative">
      <li>
        <label :for="`item-${item.uiKey}-name-filter`" class="visually-hidden"
          >Type to filter options</label
        >
        <input
          :id="`item-${item.uiKey}-name-filter`"
          v-model="filterText"
          type="text"
          class="form-control mb-2"
          placeholder="Type to filter options"
          @click.stop
        />
      </li>
      <li v-if="filteredSuggestions.length === 0" class="dropdown-item disabled">
        No options found
      </li>
      <li v-for="suggestion in filteredSuggestions" :key="suggestion.id">
        <a class="dropdown-item" href="#" @click.prevent="selectSuggestion(suggestion.id)">
          {{ suggestion.name }}
        </a>
      </li>
    </ul>
  </div>
</template>

<style scoped>
button.form-select {
  text-align: left;
}
</style>
