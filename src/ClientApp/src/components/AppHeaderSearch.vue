<script setup lang="ts">
import { defineModel } from 'vue';
import useRecipeStore from '@/stores/recipeStore';
import router from '@/router';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const searchText = defineModel<string>();

const recipeStore = useRecipeStore();

function initSearch() {
  const query = RecipeStoreHelpers.listRequestToQueryParams({
    name: searchText.value,
    take: recipeStore.listRequest.take,
  });

  router
    .push({
      name: 'search',
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
        aria-label="Search"
        @keydown.enter.stop.prevent="initSearch"
      />
      <button
        class="btn btn-outline-light nav-search-button text-light bg-primary"
        type="button"
        aria-label="Search"
        @click.stop.prevent="initSearch"
      >
        <font-awesome-icon icon="fa-search" aria-label="view recipe" />
      </button>
    </div>
  </form>
</template>

<style lang="scss" scoped>
.nav-search-button {
  min-width: unset;
}
</style>
