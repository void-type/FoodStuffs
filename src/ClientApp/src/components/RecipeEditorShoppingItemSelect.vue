<script lang="ts" setup>
import { ref, computed, type PropType } from 'vue';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import type WorkingRecipeShoppingItem from '@/models/RecipeShoppingItemWorking';
import { Dropdown } from 'bootstrap';
import { trimAndTitleCase } from '@/models/FormatHelper';

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
  onCreateItem: {
    type: Function,
    required: true,
  },
});

const filterText = ref('');

const filteredSuggestions = computed(() => {
  const { usedIds } = props;

  const filterTextLow = filterText.value.toLowerCase();

  return props.suggestions.filter(
    (x) => !usedIds.includes(x.id) && x.name?.toLowerCase().includes(filterTextLow)
  );
});

function selectSuggestion(id: number | undefined) {
  filterText.value = '';
  model.value = id;
}

const dropdownButton = ref<HTMLButtonElement | null>(null);

async function createShoppingItem(name: string) {
  const newId = await props.onCreateItem(name);

  if (newId) {
    selectSuggestion(newId);
    if (dropdownButton.value) {
      const dropdown = Dropdown.getOrCreateInstance(dropdownButton.value);
      dropdown.hide();
    }
  }
}
</script>

<template>
  <div class="dropdown">
    <button
      ref="dropdownButton"
      class="form-select"
      type="button"
      data-bs-toggle="dropdown"
      aria-expanded="false"
    >
      {{ (item.id || 0) > 0 ? itemName : 'Select one' }}
    </button>
    <ul class="dropdown-menu pt-0 w-100">
      <li>
        <label :for="`item-${item.uiKey}-name-filter`" class="visually-hidden"
          >Type to filter options</label
        >
        <input
          :id="`item-${item.uiKey}-name-filter`"
          v-model="filterText"
          type="text"
          class="form-control"
          placeholder="Type to filter options"
          @click.stop
        />
      </li>
      <li v-if="filteredSuggestions.length === 0" class="p-3">
        No options found
        <div v-if="filterText.length > 0" class="btn-toolbar mt-2">
          <button
            class="btn btn-secondary"
            type="button"
            @click.stop.prevent="(event) => createShoppingItem(trimAndTitleCase(filterText))"
          >
            Create {{ trimAndTitleCase(filterText) }}
          </button>
        </div>
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

.dropdown-menu {
  max-height: 19rem;
  overflow-y: auto;
}
</style>
