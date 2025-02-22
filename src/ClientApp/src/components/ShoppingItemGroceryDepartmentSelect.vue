<script lang="ts" setup>
import { ref, computed, onMounted, onBeforeUnmount, type PropType } from 'vue';
import type { ListGroceryDepartmentsResponse } from '@/api/data-contracts';
import { Dropdown } from 'bootstrap';
import { trimAndTitleCase } from '@/models/FormatHelper';
import ApiHelper from '@/models/ApiHelper';
import useMessageStore from '@/stores/messageStore';
import type { HttpResponse } from '@/api/http-client';

const model = defineModel({
  type: Number as PropType<number | null | undefined>,
  required: true,
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const filterText = ref('');

const suggestions = ref([] as ListGroceryDepartmentsResponse[]);

const selectedSuggestion = computed(() => suggestions.value.find((x) => x.id === model.value));

async function fetchSuggestions() {
  try {
    const response = await api().groceryDepartmentsList({ isPagingEnabled: false });
    suggestions.value = (response.data.items || []) as Array<ListGroceryDepartmentsResponse>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

const filteredSuggestions = computed(() => {
  const filterTextLow = filterText.value.toLowerCase();

  return suggestions.value.filter((x) => x.name?.toLowerCase().includes(filterTextLow));
});

function selectSuggestion(id: number | undefined) {
  filterText.value = '';
  model.value = id;
}

const dropdownParent = ref<HTMLElement | null>(null);
const dropdownButton = ref<HTMLButtonElement | null>(null);
const filterInput = ref<HTMLInputElement | null>(null);

async function createItem(name: string) {
  try {
    const response = await api().groceryDepartmentsSave({ name, order: 0 });

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }

    const newId = response.data.id;

    await fetchSuggestions();

    if (newId) {
      selectSuggestion(newId);
      if (dropdownButton.value) {
        const dropdown = Dropdown.getOrCreateInstance(dropdownButton.value);
        dropdown.hide();
      }
    }
  } catch (error) {
    const response = error as HttpResponse<unknown, unknown>;
    messageStore.setApiFailureMessages(response);
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
  fetchSuggestions();

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
      {{ (selectedSuggestion?.id || 0) > 0 ? selectedSuggestion?.name : 'Select one' }}
    </button>
    <ul class="dropdown-menu pt-0 w-100">
      <li>
        <label :for="`item-${selectedSuggestion?.id}-name-filter`" class="visually-hidden"
          >Type to filter options</label
        >
        <input
          :id="`item-${selectedSuggestion?.id}-name-filter`"
          ref="filterInput"
          v-model="filterText"
          type="text"
          class="form-control"
          placeholder="Type to filter options"
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
        class="px-3 py-2"
      >
        <div class="btn-toolbar">
          <button
            class="btn btn-secondary"
            type="button"
            @click.stop.prevent="(event) => createItem(trimAndTitleCase(filterText))"
          >
            Create {{ trimAndTitleCase(filterText) }}
          </button>
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
