<template>
  <div>
    <SelectSidebarList
      v-if="recent.length > 0"
      :recipes="recent"
      :title="'Recent'"
      :route-name="routeName"
    />
    <SelectSidebarList
      :recipes="listResponse.items"
      :title="'Recipes'"
      class="mt-2"
      :route-name="routeName"
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
      recent: 'recipes/recent',
    }),
  },
  created() {
    if (this.listResponse.count === 0) {
      webApi.recipes.list(
        this.listRequest,
        data => this.setListResponse(data),
        response => this.setApiFailureMessages(response),
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
