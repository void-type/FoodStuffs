<template>
  <div>
    <button @click="newRecipe()">New</button>

    <SelectSidebarList
      v-if="recentRecipes.length > 0"
      :recipes="recentRecipes"
      :title="'Recent'"
      @selectRecipe="selectRecipe" />

    <SelectSidebarList
      :recipes="recipesList"
      :title="'Recipes'"
      @selectRecipe="selectRecipe" />
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import SelectSidebarList from './SelectSidebarList.vue';

export default {
  components: {
    SelectSidebarList,
  },
  computed: {
    ...mapGetters(['recipesList', 'recentRecipes']),
  },
  methods: {
    ...mapActions(['selectRecipe']),
    newRecipe() {
      this.selectRecipe();
      this.$router.push({ name: 'edit' });
    },
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
