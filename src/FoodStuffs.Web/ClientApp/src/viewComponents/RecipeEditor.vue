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
    <EntityDetailsAuditInfo
      v-if="currentRecipe.id"
      :entity="currentRecipe" />
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
  </form>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import TagEditor from './TagEditor.vue';
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';

export default {
  components: {
    TagEditor,
    EntityDetailsAuditInfo,
  },
  computed: {
    ...mapGetters({
      isFieldInError: 'app/isFieldInError',
    }),
    methods: {
      ...mapActions([
        'deleteRecipe',
        'fetchRecipesList',
        'saveRecipe',
        'addCategoryToRecipe',
        'removeCategoryFromRecipe',
      ]),
      cancelClick() {
        this.$router.push({ name: 'view' });
      },
      addCategoryToCurrentRecipe(categoryName) {
        const recipe = this.currentRecipe;
        this.addCategoryToRecipe({ recipe, categoryName });

      // addCategoryToRecipe(context, { recipe, categoryName }) {
      //   const cleanedCategoryName = trimAndCapitalize(categoryName);

      //   const categoryDoesNotExist = recipe.categories
      //     .map(value => value.toUpperCase())
      //     .indexOf(categoryName.toUpperCase()) < 0;

      //   if (categoryDoesNotExist && cleanedCategoryName.length > 0) {
      //     context.commit('ADD_CATEGORY_TO_RECIPE', { recipe, cleanedCategoryName });
      //   }
      // },
      // removeCategoryFromRecipe(context, { recipe, categoryName }) {
      //   const categoryIndex = recipe.categories.indexOf(categoryName);

      //   if (categoryIndex > -1) {
      //     context.commit('REMOVE_CATEGORY_FROM_RECIPE', { recipe, categoryIndex });
      //   }
      // },

      // ADD_CATEGORY_TO_RECIPE(state, { recipe, cleanedCategoryName }) {
      //   recipe.categories.push(cleanedCategoryName);
      // },
      // REMOVE_CATEGORY_FROM_RECIPE(state, { recipe, categoryIndex }) {
      //   recipe.categories.splice(categoryIndex, 1);
      // },
      },
      removeCategoryFromCurrentRecipe(categoryName) {
        const recipe = this.currentRecipe;
        this.removeCategoryFromRecipe({ recipe, categoryName });
      },
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

.pull-right {
  margin-left: auto;
}
</style>
