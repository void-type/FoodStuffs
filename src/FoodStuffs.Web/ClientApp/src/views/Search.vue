<template>
  <div>
    <EntityTableControls
      :clear-search="clearSearch"
      :init-search="fetchRecipesList"
      class="mt-4"
    >
      <b-input-group
        slot="searchControls"
        prepend="Name contains"
        class="mr-1"
      >
        <b-form-input
          id="nameSearch"
          :value="listRequest.nameSearch"
          @input="setListRequestNameSearch"
        />
      </b-input-group>
      <b-input-group
        slot="searchControls"
        prepend="Categories contain"
        class="mr-1"
      >
        <b-form-input
          id="categorySearch"
          :value="listRequest.categorySearch"
          @input="setListRequestCategorySearch"
        />
      </b-input-group>
    </EntityTableControls>
    <b-table
      :items="listResponse.items"
      :fields="tableFields"
      show-empty
      hover
      class="mt-3"
      @row-clicked="onTableRowClick"
    />
    <EntityTablePager
      :list-response="listResponse"
      :list-request="listRequest"
      :change-page="updatePage"
      :change-take="updateTake"
    />
  </div>
</template>

<script>

// :sort="getSortType"
// :on-cycle-sort="cycleSort"

import { mapActions, mapGetters } from 'vuex';
import recipesApiModels from '../models/recipesApiModels';
import webApi from '../webApi';
import router from '../router';
import EntityTableControls from '../viewComponents/EntityTableControls.vue';
import EntityTablePager from '../viewComponents/EntityTablePager.vue';

export default {
  components: {
    EntityTableControls,
    EntityTablePager,
  },
  data() {
    return {
      tableFields: [
        'name',
        {
          key: 'categories',
          formatter: value => value.join(', '),
        },
      ],
    };
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
  created() {
    if (this.listResponse.count === 0) {
      this.fetchRecipesList();
    }
  },
  methods: {
    ...mapActions({
      setApiFailureMessages: 'app/setApiFailureMessages',
      resetListRequest: 'recipes/resetListRequest',
      setListResponse: 'recipes/setListResponse',
      setListRequestNameSearch: 'recipes/setListRequestNameSearch',
      setListRequestCategorySearch: 'recipes/setListRequestCategorySearch',
      setListRequestSort: 'recipes/setListRequestSort',
      setListRequestPage: 'recipes/setListRequestPage',
      setListRequestTake: 'recipes/setListRequestTake',
    }),
    fetchRecipesList() {
      webApi.recipes.list(
        this.listRequest,
        data => this.setListResponse(data),
        response => this.setApiFailureMessages(response),
      );
    },
    updatePage(page) {
      this.setListRequestPage(page);
      this.fetchRecipesList();
    },
    updateTake(take) {
      this.setListRequestTake(take);
      this.setListRequestPage(1);
      this.fetchRecipesList();
    },
    cycleSort() {
      const { nextSort } = recipesApiModels.listSortOptions;
      this.setListRequestSort(nextSort(this.listRequest.sort).name);
      this.fetchRecipesList();
    },
    clearSearch() {
      this.resetListRequest();
      this.fetchRecipesList();
    },
    onTableRowClick(recipe) {
      router.push({ name: 'view', params: { id: recipe.id } }).catch(() => {});
    },
  },
};
</script>

<style lang="scss" scoped>
table.table-hover {
  cursor: pointer;
}
</style>
