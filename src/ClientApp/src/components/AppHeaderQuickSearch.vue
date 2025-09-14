<script setup lang="ts">
import useRecipeStore from '@/stores/recipeStore';
import useMessageStore from '@/stores/messageStore';
import router from '@/router';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { onClickOutside } from '@vueuse/core';
import { onBeforeUnmount, onMounted, ref, computed } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import type { SuggestRecipesResultItem, SuggestGroceryItemsResultItem } from '@/api/data-contracts';
import RouterHelper from '@/models/RouterHelper';
import { debounce } from '@/models/InputHelper';

const searchText = defineModel<string | null | undefined>();

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const api = ApiHelper.client;

const recipeSuggestions = ref<SuggestRecipesResultItem[]>([]);

const grocerySuggestions = ref<SuggestGroceryItemsResultItem[]>([]);
type SearchMode = 'recipe' | 'groceryItem';
const searchMode = ref<SearchMode>('recipe');
// Prevent suggestions from showing if they were requested but now no longer useful.
let cancelInFlightSuggestions = false;

function closeSuggestions() {
  cancelInFlightSuggestions = true;
  recipeSuggestions.value = [];
  grocerySuggestions.value = [];
}

function clearSearch() {
  closeSuggestions();
  searchText.value = '';
}

function cycleSearchMode() {
  searchMode.value = searchMode.value === 'recipe' ? 'groceryItem' : 'recipe';
  clearSearch();
}

const searchModeIcon = computed(() =>
  searchMode.value === 'recipe' ? 'fa-utensils' : 'fa-shopping-basket'
);

function navigateSearchPage() {
  const query = RecipeStoreHelper.listRequestToQueryParams({
    searchText: searchText.value,
    take: recipeStore.listRequest.take,
  });

  clearSearch();

  router.push({
    name: 'recipeList',
    query,
  });
}

function navigateAway() {
  clearSearch();
}

const searchContainerRef = ref<HTMLElement | null>(null);

function suggest(event: Event) {
  searchText.value = (event.target as HTMLInputElement).value;

  debounce(async () => {
    if (!searchText.value || searchText.value.length <= 1) {
      recipeSuggestions.value = [];
      grocerySuggestions.value = [];
      return;
    }

    cancelInFlightSuggestions = false;

    try {
      if (searchMode.value === 'recipe') {
        const response = await api().recipesSuggest({
          searchText: searchText.value,
          take: 8,
        });

        // If we're not focused on the search input, don't show suggestions.
        if (!searchContainerRef.value?.contains(document.activeElement)) {
          return;
        }

        if (cancelInFlightSuggestions) {
          return;
        }

        recipeSuggestions.value = response.data?.items || [];
        grocerySuggestions.value = [];
      } else {
        const response = await api().groceryItemsSuggest({
          searchText: searchText.value,
          take: 8,
        });

        // If we're not focused on the search input, don't show suggestions.
        if (!searchContainerRef.value?.contains(document.activeElement)) {
          return;
        }

        if (cancelInFlightSuggestions) {
          return;
        }

        grocerySuggestions.value = response.data?.items || [];
        recipeSuggestions.value = [];
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }, 200)();
}

function handleFocusOut(event: FocusEvent) {
  const target = event.relatedTarget as HTMLElement;
  const isChildOfForm =
    searchContainerRef.value?.contains(document.activeElement) ||
    (target && searchContainerRef.value?.contains(target));

  if (!isChildOfForm) {
    closeSuggestions();
  }
}

onClickOutside(searchContainerRef, () => {
  closeSuggestions();
});

// Global Ctrl + Shift + F to focus the search input.
const inputRef = ref<HTMLInputElement | null>(null);
const handleKeydown = (event: KeyboardEvent) => {
  if (event.ctrlKey && event.shiftKey && event.key === 'F') {
    event.preventDefault();
    inputRef.value?.focus();
  }
};

onMounted(() => {
  document.addEventListener('keydown', handleKeydown);
});

onBeforeUnmount(() => {
  document.removeEventListener('keydown', handleKeydown);
});
</script>

<template>
  <div ref="searchContainerRef" @focusout="handleFocusOut">
    <div class="input-group" title="Use control + shift + F to focus the search.">
      <button
        class="btn btn-outline-secondary"
        type="button"
        :title="searchMode === 'recipe' ? 'Recipe search' : 'Grocery item search'"
        aria-label="Toggle search mode"
        @click.stop.prevent="cycleSearchMode"
      >
        <font-awesome-icon :icon="searchModeIcon" />
      </button>
      <input
        ref="inputRef"
        :value="searchText"
        type="search"
        inputmode="search"
        enterkeyhint="search"
        class="form-control"
        :placeholder="`Search ${searchMode === 'recipe' ? 'recipes' : 'grocery items'}`"
        :aria-label="`Search ${searchMode === 'recipe' ? 'recipes' : 'grocery items'}`"
        @input="suggest"
        @keydown.enter.stop.prevent="navigateSearchPage"
      />
      <button
        class="btn btn-outline-secondary"
        type="button"
        aria-label="Start search"
        @click.stop.prevent="navigateSearchPage"
      >
        <font-awesome-icon icon="fa-search" />
      </button>
    </div>
    <ul v-if="searchMode === 'recipe' && recipeSuggestions.length" class="dropdown-menu show">
      <li
        v-for="suggestion in recipeSuggestions"
        :key="suggestion.id"
        class="dropdown-item suggestion"
        role="option"
        aria-selected="false"
      >
        <span>
          <router-link :to="RouterHelper.viewRecipe(suggestion)" @click="navigateAway">
            {{ suggestion.name }}
          </router-link>
        </span>
      </li>
    </ul>
    <ul v-if="searchMode === 'groceryItem' && grocerySuggestions.length" class="dropdown-menu show">
      <li
        v-for="suggestion in grocerySuggestions"
        :key="suggestion.id"
        class="dropdown-item suggestion"
        role="option"
        aria-selected="false"
      >
        <span>
          <router-link :to="RouterHelper.editGroceryItem(suggestion)" @click="navigateAway">
            {{ suggestion.name }}
          </router-link>
        </span>
      </li>
    </ul>
  </div>
</template>

<style lang="scss" scoped>
.suggestion {
  display: block;
  max-width: 15rem;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
