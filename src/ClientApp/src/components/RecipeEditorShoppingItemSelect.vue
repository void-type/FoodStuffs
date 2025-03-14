<script lang="ts" setup>
import { ref, computed, onMounted, onBeforeUnmount, type PropType } from 'vue';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import type RecipeShoppingItemWorking from '@/models/RecipeShoppingItemWorking';
import { Dropdown } from 'bootstrap';
import { trimAndTitleCase } from '@/models/FormatHelper';
import type { HTMLInputEvent } from '@/models/HTMLInputEvent';

const model = defineModel({
  type: Number as PropType<number | undefined>,
  required: true,
});

const props = defineProps({
  isFieldInError: {
    type: Function,
    required: true,
  },
  item: {
    type: Object as PropType<RecipeShoppingItemWorking>,
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

const dropdownParent = ref<HTMLElement | null>(null);
const dropdownButton = ref<HTMLButtonElement | null>(null);
const filterInput = ref<HTMLInputElement | null>(null);

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

function onDropdownKeydown() {
  if (dropdownButton.value) {
    const dropdown = Dropdown.getOrCreateInstance(dropdownButton.value);
    dropdown.toggle();
  }
}

function onShow() {
  if (filterInput.value) {
    filterInput.value.value = '';
    filterInput.value.focus();
  }
}

onMounted(() => {
  if (dropdownParent.value) {
    dropdownParent.value.addEventListener('shown.bs.dropdown', onShow);
  }
});

onBeforeUnmount(() => {
  if (dropdownParent.value) {
    dropdownParent.value.removeEventListener('shown.bs.dropdown', onShow);
  }
});
</script>

<template>
  <div ref="dropdownParent" class="dropdown">
    <button
      ref="dropdownButton"
      class="form-select first-form-item"
      type="button"
      data-bs-toggle="dropdown"
      aria-expanded="false"
      @keydown="onDropdownKeydown"
    >
      {{ (item.id || 0) > 0 ? itemName : 'Select one' }}
    </button>
    <ul class="dropdown-menu pt-0 w-100">
      <li class="mb-2">
        <label :for="`item-${item.uiKey}-name-filter`" class="visually-hidden"
          >Type to filter options</label
        >
        <input
          :id="`item-${item.uiKey}-name-filter`"
          ref="filterInput"
          :value="filterText"
          type="text"
          class="form-control"
          placeholder="Type to filter options"
          @input="(e) => (filterText = (e as HTMLInputEvent).target.value)"
        />
      </li>
      <li v-for="suggestion in filteredSuggestions" :key="suggestion.id">
        <a class="dropdown-item" href="#" @click.prevent="selectSuggestion(suggestion.id)">
          {{ suggestion.name }}
        </a>
      </li>
      <li v-if="filteredSuggestions.length === 0" class="p-3">No options found</li>
      <li
        v-if="
          filterText.length > 0 &&
          filteredSuggestions.find((x) => x.name === filterText) === undefined
        "
        class="mt-2"
      >
        <div class="dropdown-item-text">
          <div class="btn-toolbar">
            <button
              class="btn btn-sm btn-secondary"
              type="button"
              @click.stop.prevent="(event) => createShoppingItem(trimAndTitleCase(filterText))"
            >
              Create {{ trimAndTitleCase(filterText) }}
            </button>
          </div>
        </div>
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
