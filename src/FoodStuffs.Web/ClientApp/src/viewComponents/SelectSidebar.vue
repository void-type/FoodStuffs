<template>
  <div class="no-print">
    <SelectSidebarList
      v-if="recent.length > 0"
      :recipes="recent"
      :title="'Recent'"
      :route-name="routeName"
    />
    <SelectSidebarList
      :recipes="listResponse.items"
      :title="'Recipes'"
      :route-name="routeName"
    />
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
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
      recent: 'recipes/recent',
    }),
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";

div > * {
  margin-bottom: 1rem;
}
</style>
