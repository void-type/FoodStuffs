<template>
  <form @keydown.ctrl.enter.prevent="onSave(workingRecipe)">
    <h1>Edit Recipe</h1>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('name')}">
        <input
          id="name"
          v-model="workingRecipe.name"
          type="text"
          name="name" >
        <label for="name">Name</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('ingredients')}">
        <textarea
          id="ingredients"
          v-model="workingRecipe.ingredients"
          name="ingredients"/>
        <label for="ingredients">Ingredients</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('directions')}">
        <textarea
          id="directions"
          v-model="workingRecipe.directions"
          name="directions"/>
        <label for="directions">Directions</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('prepTimeMinutes')}">
        <input
          id="prepTimeMinutes"
          v-model="workingRecipe.prepTimeMinutes"
          type="number"
          name="prepTimeMinutes" >
        <label for="prepTimeMinutes">Prep Time Minutes</label>
      </div>
      <div :class="{'form-group': true, danger: isFieldInError('cookTimeMinutes')}">
        <input
          id="cookTimeMinutes"
          v-model="workingRecipe.cookTimeMinutes"
          type="number"
          name="cookTimeMinutes" >
        <label for="cookTimeMinutes">Cook Time Minutes</label>
      </div>
    </div>
    <div class="form-row">
      <TagEditor
        :class="{'form-group': true, danger: isFieldInError('categories')}"
        :tags="categories"
        :on-add-tag="addCategoryToCurrentRecipe"
        :on-remove-tag="removeCategoryFromCurrentRecipe"
        field-name="categories"
        label="Categories" />
    </div>
    <EntityDetailsAuditInfo
      v-if="sourceRecipe.id"
      :entity="sourceRecipe" />
    <div class="form-row button-row">
      <button
        @click.prevent="onSave(workingRecipe)"
      >Save</button>
      <router-link
        :to="{name: 'view', params: {id: sourceRecipe.id}}"
        tag="button"
      >Cancel</router-link>
      <button
        class="pull-right danger"
        @click.prevent="onDelete(workingRecipe)"
      >Delete</button>
    </div>
  </form>
</template>

<script>
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';
import TagEditor from './TagEditor.vue';
import recipeApiModels from '../models/RecipeApiModels';

export default {
  components: {
    EntityDetailsAuditInfo,
    TagEditor,
  },
  props: {
    sourceRecipe: {
      type: Object,
      required: true,
    },
    isFieldInError: {
      type: Function,
      required: true,
    },
    onSave: {
      type: Function,
      required: true,
    },
    onDelete: {
      type: Function,
      required: true,
    },
  },
  data() {
    return {
      workingRecipe: new recipeApiModels.GetResponse(),
    };
  },
  watch: {
    sourceRecipe() {
      this.reset();
    },
  },
  created() {
    this.reset();
  },
  methods: {
    reset() {
      Object.assign(this.workingRecipe, this.sourceRecipe);
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
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

.pull-right {
  margin-left: auto;
}
</style>
