<script setup lang="ts">
import useRecipeStore from '@/stores/recipeStore';
import useMessageStore from '@/stores/messageStore';
import router from '@/router';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import type { SuggestRecipesResultItem } from '@/api/data-contracts';
import RouterHelper from '@/models/RouterHelper';

const searchText = defineModel<string>();

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const api = ApiHelper.client;

const suggestions = ref<SuggestRecipesResultItem[]>([]);

function initSearch() {
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
  searchText.value = '';
  suggestions.value = [];
}

// eslint-disable-next-line @typescript-eslint/ban-types
const debounce = (fn: Function, ms = 300) => {
  let timeoutId: ReturnType<typeof setTimeout>;
  // eslint-disable-next-line func-names
  return function (this: unknown, ...args: unknown[]) {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => fn.apply(this, args), ms);
  };
};

const suggest = debounce(async () => {
  if (!searchText.value || searchText.value.length <= 1) {
    suggestions.value = [];
    return;
  }

  try {
    const response = await api().recipesSuggest({
      searchText: searchText.value,
    });

    suggestions.value = response.data?.items || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}, 200);
</script>

<template>
  <form role="search" @keydown.enter.stop.prevent>
    <div class="input-group">
      <input
        v-model="searchText"
        class="form-control"
        type="text"
        placeholder="Search"
        aria-label="Search text"
        @input="suggest"
        @keydown.enter.stop.prevent="initSearch"
      />
      <button
        class="btn btn-secondary nav-search-button text-light bg-primary"
        type="button"
        aria-label="Start search"
        @click.stop.prevent="initSearch"
      >
        <font-awesome-icon icon="fa-search" />
      </button>
    </div>
    <ul v-if="suggestions.length" class="dropdown-menu show">
      <li v-for="suggestion in suggestions" :key="suggestion.id" class="dropdown-item">
        <span
          ><router-link :to="RouterHelper.viewRecipe(suggestion)" @click="clearSearch">
            {{ suggestion.name }}
          </router-link>
        </span>
      </li>
    </ul>
  </form>
</template>

<style lang="scss" scoped>
.dropdown-item span {
  display: block;
  max-width: 15rem;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
