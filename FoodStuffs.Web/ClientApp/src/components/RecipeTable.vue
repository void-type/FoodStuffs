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
  var Recipe = require("../models/createdRecipeViewModel.js");

  Vue.component("recipe-table", {
    template: require("./recipe-table.html"),
    props: ["recipes"],
    methods: {
      selectRecipe: function (recipe) {
        this.$emit("recipe-selected", recipe);
      },

      newRecipe: function () {
        this.$emit("recipe-selected", new Recipe());
      }
    }
  });
</script>

<style lang="scss">
  @import 'RecipeTable';
</style>
