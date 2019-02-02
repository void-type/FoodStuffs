<template>
  <div>
    <button :to="{name: 'new'}">New</button>
    <SelectSidebarList
      v-if="recent.length > 0"
      :recipes="recent"
      :title="'Recent'" />
    <SelectSidebarList
      :recipes="listResponse.items"
      :title="'Recipes'" />
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import SelectSidebarList from './SelectSidebarList.vue';

export default {
  components: {
    SelectSidebarList,
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
@import "../style/inputs";

div > * {
  margin-bottom: 1em;
}
</style>
