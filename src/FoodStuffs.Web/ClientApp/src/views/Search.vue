<template>
  <b-container>
    <h1 class="mt-4">
      Search Recipes
    </h1>
    <EntityTableControls
      :clear-search="clearSearch"
      :init-search="startSearch"
      class="mt-4"
    >
      <b-form-row
        slot="searchControls"
      >
        <b-col
          sm="12"
          md="6"
        >
          <b-input-group
            prepend="Name contains"
            class="mb-2"
          >
            <b-form-input
              id="nameSearch"
              v-model="workingRequest.name"
              name="nameSearch"
            />
          </b-input-group>
        </b-col>
        <b-col
          sm="12"
          md="6"
        >
          <b-input-group
            prepend="Categories contain"
            class="mb-2"
          >
            <b-form-input
              id="categorySearch"
              v-model="workingRequest.category"
              name="categorySearch"
            />
          </b-input-group>
        </b-col>
      </b-form-row>
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
      class="mt-4"
      @row-clicked="showDetails"
      @sort-changed="tableSortChanged"
    />
    <EntityTablePager
      :list-response="listResponse"
      :list-request="listRequest"
      :change-page="changePage"
      :change-take="changeTake"
      class="mt-4"
    />
  </b-container>
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
  props: {
    query: {
      type: Object,
      required: false,
      default: () => {},
    },
  },
  data() {
    return {
      workingRequest: new ListRecipesRequest(),
      tableSortBy: new ListRecipesRequest().sort,
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
    if (Object.keys(this.query).length !== 0) {
      const defaultRequest = new ListRecipesRequest();
      this.workingRequest = Object.assign(defaultRequest, this.query);
    } else {
      this.workingRequest = this.listRequest;
    }
    // TODO: there is bug in b-pagination that misses the page number from a URL navigation.
    // To replicate, go to page 2, hit F5, note page number is 1 with content of 2
    this.fetchList();
  },
  methods: {
    ...mapActions({
      setApiFailureMessages: 'app/setApiFailureMessages',
      setListResponse: 'recipes/setListResponse',
      setListRequest: 'recipes/setListRequest',
    }),
    fetchList() {
      router.replace({ query: this.workingRequest }).catch(() => {});

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
</style>
