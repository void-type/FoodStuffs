<template>
  <form class="recipe-form" v-if="currentRecipe !== undefined">
    <h1>Edit Recipe</h1>
    <div class="form-group">
      <label title="Name">Name</label>
      <input type="text" name="name" v-model="currentRecipe.name" />
    </div>
    <div class="form-group">
      <label title="Ingredients">Ingredients</label>
      <textarea name="ingredients" v-model="currentRecipe.ingredients"></textarea>
    </div>
    <div class="form-group">
      <label title="Directions">Directions</label>
      <textarea name="stuff" v-model="currentRecipe.directions"></textarea>
    </div>
    <div class="form-group">
      <label title="Prep Time (minutes)">Prep Time (minutes)</label>
      <input type="number" name="prepTimeMinutes" v-model="currentRecipe.prepTimeMinutes" />
    </div>
    <div class="form-group">
      <label title="Cook Time (minutes)">Cook Time (minutes)</label>
      <input type="number" name="cookTimeMinutes" v-model="currentRecipe.cookTimeMinutes" />
    </div>
    <div class="form-group">
      <label title="Categories">Categories</label>
      <div>
        <span v-for="category in currentRecipe.categories" v-bind:key="category">{{category}}</span>
      </div>
    </div>
    <div class="form-group">
      <label></label>
      <button v-on:click.prevent="saveClick()">
        Save
      </button>
      <button v-on:click.prevent="cancelClick()">
        Cancel
      </button>
      <button class="pull-right error" v-on:click.prevent="deleteClick()">
        Delete
      </button>
    </div>
  </form>
</template>

<script>
  var Recipe = require("../models/createdRecipeViewModel.js");

  Vue.component("recipe-form", {
    template: require("./recipe-form.html"),
    props: ["current-recipe", "fields-in-error"],
    methods: {
      saveClick: function () {
        if (this.currentRecipe.id === undefined || this.currentRecipe.id < 1) {
          this.$emit("create-recipe", this.currentRecipe);
        } else {
          this.$emit("update-recipe", this.currentRecipe);
        }
      },

      deleteClick: function () {
        this.$emit("delete-recipe", this.currentRecipe);
      },

      cancelClick: function () {
        this.currentRecipe = new Recipe();
      }
    }
  });
</script>

<style lang="scss">
  @import 'RecipeForm';
</style>
