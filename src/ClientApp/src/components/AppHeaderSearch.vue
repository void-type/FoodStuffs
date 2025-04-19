<script setup lang="ts">
import useRecipeStore from '@/stores/recipeStore';
import useMessageStore from '@/stores/messageStore';
import router from '@/router';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { onClickOutside } from '@vueuse/core';
import { onBeforeUnmount, onMounted, ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import type { SuggestRecipesResultItem } from '@/api/data-contracts';
import RouterHelper from '@/models/RouterHelper';
import { debounce } from '@/models/InputHelper';

const searchText = defineModel<string | null | undefined>();

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const api = ApiHelper.client;

const suggestions = ref<SuggestRecipesResultItem[]>([]);

// Prevent suggestions from showing if they were requested but now no longer useful.
let cancelInFlightSuggestions = false;

function closeSearch() {
  cancelInFlightSuggestions = true;
  suggestions.value = [];
}

function clearSearch() {
  closeSearch();
  searchText.value = '';
}

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

function navigateRecipe() {
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

      suggestions.value = response.data?.items || [];
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
    closeSearch();
  }
}

// Replace your focusout handler with this
onClickOutside(searchContainerRef, () => {
  closeSearch();
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
      <input
        ref="inputRef"
        :value="searchText"
        type="search"
        inputmode="search"
        enterkeyhint="search"
        class="form-control"
        placeholder="Search"
        aria-label="Search text"
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
    <ul v-if="suggestions.length" class="dropdown-menu show">
      <li v-for="suggestion in suggestions" :key="suggestion.id" class="dropdown-item suggestion">
        <span
          ><router-link :to="RouterHelper.viewRecipe(suggestion)" @click="navigateRecipe">
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
