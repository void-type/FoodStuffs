<template>
  <form @keydown.ctrl.enter.prevent="saveClick(workingRecipe)">
    <h1>{{ workingRecipe.id > 0 ? 'Edit' : 'New' }} Recipe</h1>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('name')}">
        <input
          id="name"
          v-model="workingRecipe.name"
          type="text"
          name="name"
        >
        <label for="name">Name</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('ingredients')}">
        <textarea
          id="ingredients"
          v-model="workingRecipe.ingredients"
          name="ingredients"
        />
        <label for="ingredients">Ingredients</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('directions')}">
        <textarea
          id="directions"
          v-model="workingRecipe.directions"
          name="directions"
        />
        <label for="directions">Directions</label>
      </div>
    </div>
    <div class="form-row">
      <div :class="{'form-group': true, danger: isFieldInError('prepTimeMinutes')}">
        <input
          id="prepTimeMinutes"
          v-model="workingRecipe.prepTimeMinutes"
          type="number"
          name="prepTimeMinutes"
        >
        <label for="prepTimeMinutes">Prep Time Minutes</label>
      </div>
      <div :class="{'form-group': true, danger: isFieldInError('cookTimeMinutes')}">
        <input
          id="cookTimeMinutes"
          v-model="workingRecipe.cookTimeMinutes"
          type="number"
          name="cookTimeMinutes"
        >
        <label for="cookTimeMinutes">Cook Time Minutes</label>
      </div>
    </div>
    <div class="form-row">
      <TagEditor
        :class="{'form-group': true, danger: isFieldInError('categories')}"
        :tags="workingRecipe.categories"
        :on-add-tag="addCategory"
        :on-remove-tag="removeCategory"
        field-name="categories"
        label="Categories"
      />
    </div>
    <EntityDetailsAuditInfo
      v-if="sourceRecipe.id"
      :entity="sourceRecipe"
    />
    <div class="form-row button-row">
      <button
        @click.prevent="saveClick(workingRecipe)"
      >
        Save
      </button>
      <router-link
        v-if="workingRecipe.id > 0"
        :to="{name: 'new', params: {newRecipeSuggestion: workingRecipe}}"
        tag="button"
      >
        Copy
      </router-link>
      <router-link
        v-if="workingRecipe.id > 0"
        :to="{name: 'view', params: {id: sourceRecipe.id}}"
        tag="button"
      >
        Cancel
      </router-link>
      <button
        v-if="workingRecipe.id > 0"
        class="pull-right danger"
        @click.prevent="onDelete(workingRecipe.id)"
      >
        Delete
      </button>
    </div>
  </form>
</template>

<script>
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';
import TagEditor from './TagEditor.vue';
import recipesApiModels from '../models/recipesApiModels';
import trimAndTitleCase from '../util/trimAndTitleCase';

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
      workingRecipe: new recipesApiModels.SaveRequest(),
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
    saveClick(workingRecipe) {
      const sendableRecipe = new recipesApiModels.SaveRequest();

      Object.keys(sendableRecipe).forEach((key) => {
        sendableRecipe[key] = workingRecipe[key];
      });

      this.onSave(sendableRecipe);
    },
    addCategory(tag) {
      const categoryName = trimAndTitleCase(tag);

      const categoryDoesNotExist = this.workingRecipe.categories
        .map(value => value.toUpperCase())
        .indexOf(categoryName.toUpperCase()) < 0;

      if (categoryDoesNotExist && categoryName.length > 0) {
        this.workingRecipe.categories.push(categoryName);
      }
    },
    removeCategory(categoryName) {
      const categoryIndex = this.workingRecipe.categories.indexOf(categoryName);

      if (categoryIndex > -1) {
        this.workingRecipe.categories.splice(categoryIndex, 1);
      }
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
