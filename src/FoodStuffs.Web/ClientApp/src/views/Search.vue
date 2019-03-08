<template>
  <section>
    <SearchControls
      :name-search="listRequest.nameSearch"
      :category-search="listRequest.categorySearch"
      :on-category-search-change="setListCategorySearch"
      :on-name-search-change="setListNameSearch"
      :on-search="fetchRecipesList"
      :on-clear="clearSearch" />
    <SearchTable
      :recipes="listResponse.items"
      :sort="getSortType"
      :on-cycle-sort="cycleSort" />
    <Pager
      :page="listRequest.page"
      :take="listRequest.take"
      :total-count="listResponse.totalCount"
      :on-page-change="updatePage"
      :on-take-change="updateTake" />
  </section>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import recipesApiModels from '../webApi/recipes/models';
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
    ...mapGetters({
      listResponse: 'recipes/listResponse',
      listRequest: 'recipes/listRequest',
    }),
    getSortType() {
      return recipesApiModels.listSortOptions.getTypeByName(this.listRequest.sort);
    },
  },
  methods: {
    ...mapActions({
      fetchRecipesList: 'recipes/fetchList',
      resetListRequest: 'recipes/resetListRequest',
      setListNameSearch: 'recipes/setListNameSearch',
      setListCategorySearch: 'recipes/setListCategorySearch',
      setListSort: 'recipes/setListSort',
      setListPage: 'recipes/setListPage',
      setListTake: 'recipes/setListTake',
    }),
    updatePage(page) {
      this.setListPage(page);
      this.fetchRecipesList();
    },
    updateTake(take) {
      this.setListTake(take);
      this.setListPage(1);
      this.fetchRecipesList();
    },
    cycleSort() {
      this.setListSort(recipesApiModels.listSortOptions.nextSort(this.listRequest.sort).name);
      this.fetchRecipesList();
    },
    clearSearch() {
      this.resetListRequest();
      this.fetchRecipesList();
    },
  },
};
</script>

<style lang="scss" scoped>
main > section {
  width: 100%;
  flex-direction: column;

  & > *:not(:last-child) {
    margin-bottom: 1.5rem;
    margin-right: 0;
  }
}
</style>
