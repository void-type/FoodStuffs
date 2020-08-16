<template>
  <form
    id="recipe-details-form"
    name="recipe-details-form"
    @keydown.ctrl.enter.stop.prevent="saveClick()"
  >
    <b-form-row>
      <b-col
        md="12"
        sm="6"
      />
      <b-col
        md="12"
      >
        <b-form-group
          label="Name *"
          label-for="name"
        >
          <b-form-input
            id="name"
            v-model="workingRecipe.name"
            required
            :class="{'is-invalid': isFieldInError('name')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <b-form-group
          label="Ingredients *"
          label-for="ingredients"
        >
          <b-form-textarea
            id="ingredients"
            v-model="workingRecipe.ingredients"
            required
            rows="1"
            :max-rows="Number.MAX_SAFE_INTEGER"
            :class="{'is-invalid': isFieldInError('ingredients')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <b-form-group
          label="Directions *"
          label-for="directions"
        >
          <b-form-textarea
            id="directions"
            v-model="workingRecipe.directions"
            required
            rows="1"
            :max-rows="Number.MAX_SAFE_INTEGER"
            :class="{'is-invalid': isFieldInError('directions')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group
          label="Prep Time Hours/Minutes"
          label-for="prepTimeMinutes"
        >
          <RecipeTimeSpanEditor
            id="prepTimeMinutes"
            v-model="workingRecipe.prepTimeMinutes"
            :is-invalid="isFieldInError('prepTimeMinutes')"
            show-preview
          />
        </b-form-group>
      </b-col>
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group
          label="Cook Time Hours/Minutes"
          label-for="cookTimeMinutes"
        >
          <RecipeTimeSpanEditor
            id="cookTimeMinutes"
            v-model="workingRecipe.cookTimeMinutes"
            :is-invalid="isFieldInError('cookTimeMinutes')"
            show-preview
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <TagEditor
          :class="{'form-group': true, danger: isFieldInError('categories')}"
          :tags="workingRecipe.categories"
          :on-add-tag="addCategory"
          :on-remove-tag="removeCategory"
          field-name="categories"
          label="Categories"
        />
      </b-col>
    </b-form-row>
    <EntityDetailsAuditInfo
      v-if="sourceRecipe.id"
      class="mb-3"
      :entity="sourceRecipe"
    />
    <b-form-row>
      <b-col
        md="12"
      >
        <b-button-toolbar>
          <b-button
            class="mr-2"
            variant="primary"
            @click.stop.prevent="saveClick()"
          >
            Save
          </b-button>
          <b-button
            v-if="!isCreateMode"
            :to="{name: 'new', params: {newRecipeSuggestion: getCopy()}}"
            class="mr-2"
          >
            Copy
          </b-button>
          <b-button
            v-if="!isCreateMode"
            :to="{name: 'view', params: {id: sourceRecipe.id}}"
          >
            Cancel
          </b-button>
          <b-button
            v-if="!isCreateMode"
            class="ml-auto"
            variant="danger"
            @click.stop.prevent="onRecipeDelete(workingRecipe.id)"
          >
            Delete
          </b-button>
        </b-button-toolbar>
      </b-col>
    </b-form-row>
  </form>
</template>

<script>
import { mapActions } from 'vuex';
import { SaveRecipeRequest } from '../models/recipesApiModels';
import trimAndTitleCase from '../util/trimAndTitleCase';
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';
import RecipeTimeSpanEditor from './RecipeTimeSpanEditor.vue';
import TagEditor from './TagEditor.vue';

export default {
  components: {
    EntityDetailsAuditInfo,
    RecipeTimeSpanEditor,
    TagEditor,
  },
  props: {
    isFieldInError: {
      type: Function,
      required: true,
    },
    sourceRecipe: {
      type: Object,
      required: true,
    },
    onRecipeSave: {
      type: Function,
      required: true,
    },
    onRecipeDelete: {
      type: Function,
      required: true,
    },
    onRecipeDirtyStateChange: {
      type: Function,
      required: false,
      default: () => {},
    },
    isCreateMode: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      workingRecipe: new SaveRecipeRequest(),
      isRecipeDirty: false,
    };
  },
  watch: {
    sourceRecipe() {
      this.reset();
    },
    workingRecipe: {
      handler() {
        const changedValues = Object.keys(this.workingRecipe)
          // Loose comparison so numbers and strings of numbers are equal.
          // eslint-disable-next-line eqeqeq
          .map(key => this.workingRecipe[key] == this.sourceRecipe[key])
          .filter(value => value === false);

        const isDirty = changedValues.length > 0;
        this.isRecipeDirty = isDirty;
      },
      deep: true,
    },
    isRecipeDirty(newValue) {
      this.onRecipeDirtyStateChange(newValue);
    },
  },
  created() {
    this.reset();
  },
  methods: {
    ...mapActions({
      setValidationErrorMessages: 'app/setValidationErrorMessages',
    }),
    reset() {
      this.workingRecipe = Object.assign({}, this.sourceRecipe);
      this.isRecipeDirty = false;
    },
    addCategory(tag) {
      const categoryName = trimAndTitleCase(tag);

      const categories = this.workingRecipe.categories.slice();

      const categoryDoesNotExist = categories
        .map(value => value.toUpperCase())
        .indexOf(categoryName.toUpperCase()) < 0;

      if (categoryDoesNotExist && categoryName.length > 0) {
        categories.push(categoryName);
        this.workingRecipe.categories = categories;
      }
    },
    removeCategory(categoryName) {
      const categories = this.workingRecipe.categories.slice();
      const categoryIndex = categories.indexOf(categoryName);

      if (categoryIndex > -1) {
        categories.splice(categoryIndex, 1);
        this.workingRecipe.categories = categories;
      }
    },
    saveClick() {
      const sendableRecipe = new SaveRecipeRequest();

      Object.keys(sendableRecipe).forEach((key) => {
        sendableRecipe[key] = this.workingRecipe[key];
      });

      this.onRecipeSave(sendableRecipe);
    },
    getCopy() {
      return Object.assign({}, this.workingRecipe, { images: [] });
    },
  },
};
</script>

<style lang="scss" scoped>
textarea {
  overflow: hidden !important;
  resize: none;
}
</style>
