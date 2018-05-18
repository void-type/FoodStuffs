<template>
    <section>
        <SearchControls :nameSearch="nameSearch"
                        :categorySearch="categorySearch"
                        @updateNameSearch="updateNameSearch"
                        @updateCategorySearch="updateCategorySearch"
                        @search="requestSearch"
                        @clear="clearSearch" />

        <SearchTable :recipes="recipesList"
                     :selectedNameSortType="recipesSearchParametersSortType"
                     @selectRecipe="selectRecipe"
                     @cycleSelectedNameSortType="cycleSelectedNameSortType" />

        <SearchTablePager :page="recipesListPage"
                          :take="recipesListTake"
                          :totalCount="recipesListTotalCount"
                          @requestPage="requestPage" />
    </section>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import SearchControls from '../components/SearchControls.vue';
import SearchTable from '../components/SearchTable.vue';
import SearchTablePager from '../components/SearchTablePager.vue';

export default {
  components: {
    SearchControls,
    SearchTable,
    SearchTablePager,
  },
  computed: {
    ...mapGetters([
      'recipesList',
      'recipesListPage',
      'recipesListTake',
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
  },
  methods: {
    ...mapActions([
      'selectRecipe',
      'fetchRecipes',
      'cycleSelectedNameSortType',
    ]),
    requestPage(pageNumber) {
      this.page = pageNumber;
      this.fetchRecipes();
    },
    requestSearch() {
      this.fetchRecipes();
    },
    clearSearch() {
      this.nameSearch = '';
      this.categorySearch = '';
      this.page = 1;
      this.fetchRecipes();
    },
    updateNameSearch(value) {
      this.nameSearch = value;
    },
    updateCategorySearch(value) {
      this.categorySearch = value;
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
