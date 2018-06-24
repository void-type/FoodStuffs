<template>
  <form @keydown.ctrl.enter.prevent="saveRecipe(currentRecipe)">
    <h1>Edit Recipe</h1>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('name')}">
        <input
          id="name"
          v-model="name"
          type="text"
          name="name" >
        <label for="name">Name</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('ingredients')}">
        <textarea
          id="ingredients"
          v-model="ingredients"
          name="ingredients"/>
        <label for="ingredients">Ingredients</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('directions')}">
        <textarea
          id="directions"
          v-model="directions"
          name="directions"/>
        <label for="directions">Directions</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('prepTimeMinutes')}">
        <input
          id="prepTimeMinutes"
          v-model="prepTimeMinutes"
          type="number"
          name="prepTimeMinutes" >
        <label for="prepTimeMinutes">Prep Time Minutes</label>
      </div>
      <div :class="{'form-group': true, danger: isFieldInError('cookTimeMinutes')}">
        <input
          id="cookTimeMinutes"
          v-model="cookTimeMinutes"
          type="number"
          name="cookTimeMinutes" >
        <label for="cookTimeMinutes">Cook Time Minutes</label>
      </div>
    </div>
    <div class="form-row">
      <TagEditor
        :class="{'form-group': true, danger: isFieldInError('categories')}"
        :tags="categories"
        field-name="categories"
        label="Categories"
        @addTag="addCategoryToCurrentRecipe"
        @removeTag="removeCategoryFromCurrentRecipe" />
    </div>
    <div class="form-row button-row">
      <button @click.prevent="saveRecipe(currentRecipe)">
        Save
      </button>
      <button @click.prevent="cancelClick()">
        Cancel
      </button>
      <button
        class="pull-right danger"
        @click.prevent="deleteRecipe(currentRecipe)">
        Delete
      </button>
    </div>
    <RecipeAudit
      v-if="currentRecipe.id"
      :recipe="currentRecipe" />
  </form>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import TagEditor from './TagEditor.vue';
import RecipeAudit from './RecipeAudit.vue';

export default {
  components: {
    TagEditor,
    RecipeAudit,
  },
  computed: {
    ...mapGetters(['currentRecipe', 'isFieldInError']),
    name: {
      get() {
        return this.currentRecipe.name;
      },
      set(value) {
        const recipe = this.currentRecipe;
        this.$store.dispatch('setRecipeName', { recipe, value });
      },
    },
    ingredients: {
      get() {
        return this.currentRecipe.ingredients;
      },
      set(value) {
        const recipe = this.currentRecipe;
        this.$store.dispatch('setRecipeIngredients', { recipe, value });
      },
    },
    directions: {
      get() {
        return this.currentRecipe.directions;
      },
      set(value) {
        const recipe = this.currentRecipe;
        this.$store.dispatch('setRecipeDirections', { recipe, value });
      },
    },
    prepTimeMinutes: {
      get() {
        return this.currentRecipe.prepTimeMinutes;
      },
      set(value) {
        const recipe = this.currentRecipe;
        this.$store.dispatch('setRecipePrepTimeMinutes', { recipe, value });
      },
    },
    cookTimeMinutes: {
      get() {
        return this.currentRecipe.cookTimeMinutes;
      },
      set(value) {
        const recipe = this.currentRecipe;
        this.$store.dispatch('setRecipeCookTimeMinutes', { recipe, value });
      },
    },
    categories: {
      get() {
        return this.currentRecipe.categories;
      },
    },
  },
  methods: {
    ...mapActions([
      'deleteRecipe',
      'fetchRecipesList',
      'saveRecipe',
      'addCategoryToRecipe',
      'removeCategoryFromRecipe',
    ]),
    cancelClick() {
      this.fetchRecipesList(this.currentRecipe.id);
      this.$router.push({ name: 'home' });
    },
    addCategoryToCurrentRecipe(categoryName) {
      const recipe = this.currentRecipe;
      this.addCategoryToRecipe({ recipe, categoryName });
    },
    removeCategoryFromCurrentRecipe(categoryName) {
      const recipe = this.currentRecipe;
      this.removeCategoryFromRecipe({ recipe, categoryName });
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/variables";
@import "../style/inputs";

.pull-right {
    margin-left: auto;
}
</style>
