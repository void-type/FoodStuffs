<template>
    <div>
        <button @click="newRecipe()">New</button>

        <SelectSidebarList :recipes="recentRecipes"
                           :title="'Recent'"
                           v-if="recentRecipes.length > 0" />

        <SelectSidebarList :recipes="recipesList"
                           :title="'Recipes'" />
    </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import SelectSidebarList from "./SelectSidebarList";

export default {
  components: {
    SelectSidebarList
  },
  computed: {
    ...mapGetters(["recipesList", "recentRecipes"])
  },
  methods: {
    ...mapActions(["selectRecipe"]),
    newRecipe() {
      this.selectRecipe();
      this.$router.push({ name: "edit" });
    }
  }
};
</script>

<style lang="scss" scoped>
@import "../variables";
@import "../inputs";

div > * {
  margin-bottom: 1em;
}
</style>