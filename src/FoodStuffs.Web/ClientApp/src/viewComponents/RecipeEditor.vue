<template>
  <form @keydown.ctrl.enter.prevent="saveClick(workingRecipe)">
    <h1>{{ workingRecipe.id > 0 ? 'Edit' : 'New' }} Recipe</h1>
    <b-form-row>
      <b-col
        md="12"
      >
        <b-form-group
          label="Name"
          label-for="name"
        >
          <b-form-input
            id="name"
            v-model="workingRecipe.name"
            :class="{'is-invalid': isFieldInError('name')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <b-form-group
          label="Ingredients"
          label-for="ingredients"
        >
          <b-form-textarea
            id="ingredients"
            v-model="workingRecipe.ingredients"
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
          label="Directions"
          label-for="directions"
        >
          <b-form-textarea
            id="directions"
            v-model="workingRecipe.directions"
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
          label="Prep Time Minutes"
          label-for="prepTimeMinutes"
        >
          <b-form-input
            id="prepTimeMinutes"
            v-model="workingRecipe.prepTimeMinutes"
            :class="{'is-invalid': isFieldInError('prepTimeMinutes')}"
            type="number"
          />
        </b-form-group>
      </b-col>
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group
          label="Cook Time Minutes"
          label-for="cookTimeMinutes"
        >
          <b-form-input
            id="cookTimeMinutes"
            v-model="workingRecipe.cookTimeMinutes"
            :class="{'is-invalid': isFieldInError('cookTimeMinutes')}"
            type="number"
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
            @click.stop.prevent="saveClick(workingRecipe)"
          >
            Save
          </b-button>
          <b-button
            v-if="workingRecipe.id > 0"
            :to="{name: 'new', params: {newRecipeSuggestion: workingRecipe}}"
            class="mr-2"
          >
            Copy
          </b-button>
          <b-button
            v-if="workingRecipe.id > 0"
            :to="{name: 'view', params: {id: sourceRecipe.id}}"
          >
            Cancel
          </b-button>
          <b-button
            v-if="workingRecipe.id > 0"
            class="ml-auto"
            variant="danger"
            @click.prevent="onDelete(workingRecipe.id)"
          >
            Delete
          </b-button>
        </b-button-toolbar>
      </b-col>
    </b-form-row>
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

.pull-right {
  margin-left: auto;
}
</style>
