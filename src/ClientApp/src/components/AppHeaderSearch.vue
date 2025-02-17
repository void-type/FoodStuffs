<script setup lang="ts">
import useRecipeStore from '@/stores/recipeStore';
import useMessageStore from '@/stores/messageStore';
import router from '@/router';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { onBeforeUnmount, onMounted, ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import type { SuggestRecipesResultItem } from '@/api/data-contracts';
import RouterHelper from '@/models/RouterHelper';

const searchText = defineModel<string>();

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const api = ApiHelper.client;

const suggestions = ref<SuggestRecipesResultItem[]>([]);

// Prevent suggestions from showing if they were requested but now no longer useful.
let cancelInFlightSuggestions = false;

function initSearch() {
  cancelInFlightSuggestions = true;

  const query = RecipeStoreHelper.listRequestToQueryParams({
    searchText: searchText.value,
    take: recipeStore.listRequest.take,
  });

  router
    .push({
      name: 'recipeList',
      query,
    })
    .then(() => {
      searchText.value = '';
    });
}

function clearSearch() {
  cancelInFlightSuggestions = true;
  searchText.value = '';
  suggestions.value = [];
}

// eslint-disable-next-line @typescript-eslint/ban-types
function debounce(fn: Function, ms = 300) {
  let debounceTimeoutId: ReturnType<typeof setTimeout>;
  // eslint-disable-next-line func-names
  return function (this: unknown, ...args: unknown[]) {
    clearTimeout(debounceTimeoutId);
    debounceTimeoutId = setTimeout(() => fn.apply(this, args), ms);
  };
}

const suggest = debounce(async () => {
  if (!searchText.value || searchText.value.length <= 1) {
    suggestions.value = [];
    return;
  }

  // Reset if cancelled.
  cancelInFlightSuggestions = false;

  try {
    const response = await api().recipesSuggest({
      searchText: searchText.value,
      take: 8,
    });

    if (cancelInFlightSuggestions) {
      return;
    }

    suggestions.value = response.data?.items || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}, 200);

function handleFocusOut(event: FocusEvent) {
  const relatedTarget = event.relatedTarget as HTMLElement;
  const isChildOfForm = relatedTarget?.closest('#headerSearch') !== null;

  if (!isChildOfForm) {
    suggestions.value = [];
  }
}

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
  <div id="headerSearch" @focusout="handleFocusOut">
    <div class="input-group" title="Use control + shift + F to focus the search.">
      <input
        ref="inputRef"
        v-model="searchText"
        class="form-control"
        type="text"
        placeholder="Search"
        aria-label="Search text"
        @input="suggest"
        @keydown.enter.stop.prevent="initSearch"
      />
      <button
        class="btn btn-outline-secondary text-body bg-body border"
        type="button"
        aria-label="Start search"
        @click.stop.prevent="initSearch"
      >
        <font-awesome-icon icon="fa-search" />
      </button>
    </div>
    <ul v-if="suggestions.length" class="dropdown-menu show">
      <li v-for="suggestion in suggestions" :key="suggestion.id" class="dropdown-item suggestion">
        <span
          ><router-link :to="RouterHelper.viewRecipe(suggestion)" @click="clearSearch">
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
