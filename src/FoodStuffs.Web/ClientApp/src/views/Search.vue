<template>
  <section>
    <SearchControls
      :category-search="listRequest.categorySearch"
      :name-search="listRequest.nameSearch"
      :change-category-search="setListCategorySearch"
      :change-name-search="setListNameSearch"
      :init-search="fetchRecipesList"
      :clear-search="clearSearch" />
    <SearchTable
      :recipes="listResponse.items"
      :name-sort="getNameSortType"
      :cycle-name-sort="cycleNameSort" />
    <Pager
      :total-count="listResponse.totalCount"
      :page="listResponse.page"
      :take="listResponse.take"
      :change-page="updatePage"
      :change-take="updateTake" />
  </section>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import sort from '../util/sort';
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
    getNameSortType() {
      return sort.getTypeByName(this.listResponse.nameSort);
    },
  },
  methods: {
    ...mapActions({
      fetchRecipesList: 'recipes/fetchList',
      resetListRequest: 'recipes/resetListRequest',
      setListPage: 'recipes/setListPage',
      setListTake: 'recipes/setListTake',
      setListCategorySearch: 'recipes/setListCategorySearch',
      setListNameSearch: 'recipes/setListNameSearch',
      setListNameSort: 'recipes/setListNameSort',
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
    cycleNameSort() {
      this.setListNameSort(sort.nextType(this.listResponse.nameSort).name);
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
