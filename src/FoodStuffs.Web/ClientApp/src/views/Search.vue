<template>
  <div>
    <EntityTableControls
      :clear-search="clearSearch"
      :init-search="startSearch"
      class="mt-4"
    >
      <b-input-group
        slot="searchControls"
        prepend="Name contains"
        class="mr-1"
      >
        <b-form-input
          id="nameSearch"
          v-model="workingRequest.nameSearch"
          name="nameSearch"
        />
      </b-input-group>
      <b-input-group
        slot="searchControls"
        prepend="Categories contain"
        class="mr-1"
      >
        <b-form-input
          id="categorySearch"
          v-model="workingRequest.categorySearch"
          name="categorySearch"
        />
      </b-input-group>
    </EntityTableControls>
    <b-table
      :items="listResponse.items"
      :fields="tableFields"
      :sort-by.sync="tableSortBy"
      :sort-desc.sync="tableSortDesc"
      sort-icon-left
      no-local-sorting
      show-empty
      hover
      class="mt-3"
      @row-clicked="showDetails"
      @sort-changed="tableSortChanged"
    />
    <EntityTablePager
      :list-response="listResponse"
      :list-request="listRequest"
      :change-page="changePage"
      :change-take="changeTake"
    />
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import { ListRecipesRequest } from '../models/recipesApiModels';
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
      workingRequest: new ListRecipesRequest(),
      tableSortBy: null,
      tableSortDesc: false,
    };
  },
  computed: {
    ...mapGetters({
      listResponse: 'recipes/listResponse',
      listRequest: 'recipes/listRequest',
    }),
    tableFields() {
      return [
        {
          key: 'name',
          sortable: true,
        },
        {
          key: 'categories',
          formatter: value => value.join(', '),
        },
      ];
    },
  },
  watch: {
    workingRequest: {
      handler(request) {
        this.setListRequest(Object.assign({}, request));
      },
      deep: true,
    },
  },
  created() {
    this.workingRequest = Object.assign({}, this.listRequest);

    if (this.listResponse.count === 0) {
      this.fetchList();
    }
  },
  methods: {
    ...mapActions({
      setApiFailureMessages: 'app/setApiFailureMessages',
      setListResponse: 'recipes/setListResponse',
      setListRequest: 'recipes/setListRequest',
    }),
    fetchList() {
      webApi.recipes.list(
        this.workingRequest,
        data => this.setListResponse(data),
        response => this.setApiFailureMessages(response),
      );
    },
    clearSearch() {
      this.workingRequest = new ListRecipesRequest();
      this.fetchList();
    },
    startSearch() {
      this.workingRequest.page = 1;
      this.fetchList();
    },
    changePage(page) {
      this.workingRequest.page = page;
      this.fetchList();
    },
    changeTake(take) {
      this.workingRequest.isPagingEnabled = take !== null;
      this.workingRequest.take = take;
      this.workingRequest.page = 1;
      this.fetchList();
    },
    showDetails(recipe) {
      router.push({ name: 'view', params: { id: recipe.id } });
    },
    tableSortChanged() {
      this.workingRequest.sort = `${this.tableSortBy}${this.tableSortDesc ? 'Desc' : ''}`;
      this.fetchList();
    },
  },
};
</script>

<style lang="scss" scoped>
table.table-hover {
  cursor: pointer;
}
</style>
