<template>
  <div>
    <SelectSidebarList
      v-if="recentRecipes.length > 0"
      :recipes="recentRecipes"
      :title="'Recent'"
      :route-name="routeName"
    />
    <SelectSidebarList
      :recipes="searchedRecipes"
      :title="'Recipes'"
      :route-name="routeName"
      class="mt-2"
    />
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import webApi from '../webApi';
import SelectSidebarList from './SelectSidebarList.vue';

export default {
  components: {
    SelectSidebarList,
  },
  props: {
    routeName: {
      type: String,
      required: true,
    },
  },
  computed: {
    ...mapGetters({
      listResponse: 'recipes/listResponse',
      listRequest: 'recipes/listRequest',
      recentRecipes: 'recipes/recentRecipes',
    }),
    searchedRecipes() {
      const recentIds = this.recentRecipes
        .map((r) => r.id);

      return this.listResponse.items
        .filter((r) => !recentIds.includes(r.id));
    },
  },
  created() {
    if (this.listResponse.count === 0) {
      webApi.recipes.list(
        this.listRequest,
        (data) => this.setListResponse(data),
        (response) => this.setApiFailureMessages(response),
      );
    }
  },
  methods: {
    ...mapActions({
      setApiFailureMessages: 'app/setApiFailureMessages',
      setListResponse: 'recipes/setListResponse',
    }),
  },
};
</script>

<style lang="scss" scoped>
</style>
