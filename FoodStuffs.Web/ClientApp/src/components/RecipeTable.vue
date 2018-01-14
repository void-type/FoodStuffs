<template>
  <div class="recipe-table">
    <button v-on:click="newRecipe()">New</button>
    <table class="hover">
      <tbody>
        <tr v-for="recipe in recipes" v-bind:key="recipe.id" v-on:click="selectRecipe(recipe)">
          <td>{{recipe.name}}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
  import { mapActions, mapMutations } from "vuex"

  export default {
    computed:
    {
      recipes() {
        return this.$store.state.recipes;
      }
    },
    methods: {
      ...mapActions({
        refresh: "fetchRecipes",
        newRecipe: "selectNewRecipe",
        selectRecipe: "selectRecipe"
      })
    },
    beforeMount() {
      this.refresh();
    }
  }
</script>

<style lang="scss">
  @import 'RecipeTable';
</style>
