<template>
  <section>
    <SearchControls
      :name-search="nameSearch"
      :update-name="updateNameSearch"
      :category-search="categorySearch"
      :update-category="updateCategorySearch"
      :init-search="requestSearch"
      :clear-search="clearSearch" />
    <SearchTable
      :recipes="recipesList"
      :selected-name-sort-type="recipesSearchParametersSortType"
      :select-recipe="selectRecipe"
      :cycle-selected-name-sort-type="cycleSelectedNameSortType" />
    <Pager
      :total-count="recipesListTotalCount"
      :page="page"
      :take="take"
      :change-take="updateTake"
      :change-page="requestPage" />
  </section>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import SearchControls from '../viewComponents/SearchControls.vue';
import SearchTable from '../viewComponents/SearchTable.vue';
import Pager from '../viewComponents/Pager.vue';

export default {
  components: {
    SearchControls,
    SearchTable,
    Pager,
  },
  computed: {
    ...mapGetters([
      'recipesList',
      'recipesListTotalCount',
      'recipesSearchParameters',
      'recipesSearchParametersSortType',
    ]),
    nameSearch: {
      get() {
        return this.recipesSearchParameters.nameSearch;
      },
      set(value) {
        this.$store.dispatch('setRecipesSearchParametersNameSearch', value);
      },
    },
    categorySearch: {
      get() {
        return this.recipesSearchParameters.categorySearch;
      },
      set(value) {
        this.$store.dispatch('setRecipesSearchParametersCategorySearch', value);
      },
    },
    page: {
      get() {
        return this.recipesSearchParameters.page;
      },
      set(value) {
        this.$store.dispatch('setRecipesSearchParametersPage', value);
      },
    },
    take: {
      get() {
        return this.recipesSearchParameters.take;
      },
      set(value) {
        this.$store.dispatch('setRecipesSearchParametersTake', value);
      },
    },
  },
  methods: {
    ...mapActions([
      'selectRecipe',
      'fetchRecipesList',
      'cycleSelectedNameSortType',
    ]),
    requestPage(pageNumber) {
      this.page = pageNumber;
      this.fetchRecipesList();
    },
    requestSearch() {
      this.fetchRecipesList();
    },
    clearSearch() {
      this.nameSearch = '';
      this.categorySearch = '';
      this.page = 1;
      this.fetchRecipesList();
    },
    updateNameSearch(value) {
      this.nameSearch = value;
    },
    updateCategorySearch(value) {
      this.categorySearch = value;
    },
    updateTake(value) {
      this.take = value;
    },
  },
};
</script>

<style lang="scss" scoped>
main > section {
  width: 100%;
  flex-direction: column;

  & > *:not(:last-child) {
    margin-bottom: 1.5em;
    margin-right: 0;
  }
}
</style>
