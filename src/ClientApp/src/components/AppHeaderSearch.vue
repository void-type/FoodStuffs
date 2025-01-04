<script setup lang="ts">
import useRecipeStore from '@/stores/recipeStore';
import router from '@/router';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const searchText = defineModel<string>();

const recipeStore = useRecipeStore();

function initSearch() {
  const query = RecipeStoreHelper.listRequestToQueryParams({
    name: searchText.value,
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
  </form>
</template>

<style lang="scss" scoped></style>
