<template>
  <b-navbar-nav>
    <b-nav-item
      :to="{ name: 'search' }"
    >
      Search
    </b-nav-item>
    <b-nav-item
      :to="{ name: 'new' }"
    >
      New
    </b-nav-item>
  </b-navbar-nav>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  computed: {
    ...mapGetters({
      recipesListRequest: 'recipes/listRequest',
    }),
    searchQuery() {
      return {
        name: this.recipesListRequest.name,
        category: this.recipesListRequest.category,
      };
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
