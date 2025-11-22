<script setup lang="ts">
import type { HttpResponse } from '@/api/http-client';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { onClickOutside } from '@vueuse/core';
import { computed, onBeforeUnmount, onMounted, ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import GroceryItemStoreHelper from '@/models/GroceryItemStoreHelper';
import { debounce } from '@/models/InputHelper';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import RouterHelper from '@/models/RouterHelper';
import router from '@/router';
import useGroceryItemStore from '@/stores/groceryItemStore';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';

interface UnifiedSuggestion {
  id: string;
  name: string;
  icon: string;
  route: object;
}

const searchText = defineModel<string | null | undefined>();

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const groceryItemStore = useGroceryItemStore();
const api = ApiHelper.client;

const suggestions = ref<UnifiedSuggestion[]>([]);

type SearchMode = 'recipe' | 'groceryItem';
const searchMode = ref<SearchMode>('recipe');
// Prevent suggestions from showing if they were requested but now no longer useful.
let cancelInFlightSuggestions = false;

function closeSuggestions() {
  cancelInFlightSuggestions = true;
  suggestions.value = [];
}

function clearSearch() {
  closeSuggestions();
  searchText.value = '';
}

function cycleSearchMode() {
  searchMode.value = searchMode.value === 'recipe' ? 'groceryItem' : 'recipe';
  closeSuggestions();
}

const searchModeIcon = computed(() =>
  searchMode.value === 'recipe' ? 'fa-utensils' : 'fa-shopping-basket',
);

function navigateSearchPage() {
  const searchTextValue = searchText.value;

  clearSearch();

  if (searchMode.value === 'recipe') {
    const query = RecipeStoreHelper.listRequestToQueryParams({
      searchText: searchTextValue,
      take: recipeStore.listRequest.take,
    });

    router.push({
      name: 'recipeList',
      query,
    });
  } else if (searchMode.value === 'groceryItem') {
    const query = GroceryItemStoreHelper.listRequestToQueryParams({
      searchText: searchTextValue,
      take: groceryItemStore.listRequest.take,
    });

    router.push({
      name: 'groceryItemList',
      query,
    });
  }
}

function navigateAway() {
  clearSearch();
}

const searchContainerRef = ref<HTMLElement | null>(null);

function suggest(event: Event) {
  searchText.value = (event.target as HTMLInputElement).value;

  debounce(async () => {
    if (!searchText.value || searchText.value.length <= 1) {
      suggestions.value = [];
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

        suggestions.value = (response.data?.items || []).map(recipe => ({
          id: `recipe-${recipe.id!}`,
          name: recipe.name!,
          icon: 'fa-utensils',
          route: RouterHelper.viewRecipe(recipe),
        }));
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

        suggestions.value = (response.data?.items || []).map(item => ({
          id: `groceryItem-${item.id!}`,
          name: item.name!,
          icon: 'fa-shopping-basket',
          route: RouterHelper.editGroceryItem(item),
        }));
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }, 200)();
}

function handleFocusOut(event: FocusEvent) {
  const target = event.relatedTarget as HTMLElement;
  const isChildOfForm
    = searchContainerRef.value?.contains(document.activeElement)
      || (target && searchContainerRef.value?.contains(target));

  if (!isChildOfForm) {
    closeSuggestions();
  }
}

onClickOutside(searchContainerRef, () => {
  closeSuggestions();
});

// Global Ctrl + Shift + F to focus the search input.
const inputRef = ref<HTMLInputElement | null>(null);
function handleKeydown(event: KeyboardEvent) {
  if (event.ctrlKey && event.shiftKey && event.key === 'F') {
    event.preventDefault();
    inputRef.value?.focus();
  }
}

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
        <FontAwesomeIcon :icon="searchModeIcon" />
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
      >
      <button
        class="btn btn-outline-secondary"
        type="button"
        aria-label="Start search"
        @click.stop.prevent="navigateSearchPage"
      >
        <FontAwesomeIcon icon="fa-search" />
      </button>
    </div>
    <ul v-if="suggestions.length" class="dropdown-menu show">
      <li
        v-for="suggestion in suggestions"
        :key="suggestion.id"
        class="dropdown-item suggestion"
        role="option"
        aria-selected="false"
      >
        <span>
          <router-link :to="suggestion.route" @click="navigateAway">
            <FontAwesomeIcon class="me-2" :icon="suggestion.icon" />{{
              suggestion.name
            }}</router-link>
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
