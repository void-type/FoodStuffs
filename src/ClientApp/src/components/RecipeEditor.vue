<script lang="ts" setup>
import SaveRecipeRequestClass from '@/models/SaveRecipeRequestClass';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { trimAndTitleCase } from '@/models/FormatHelpers';
import { computed, onMounted, ref, watch, type PropType, type Ref } from 'vue';
import EntityAuditInfo from './EntityAuditInfo.vue';
import RecipeTimeSpanEditor from './RecipeTimeSpanEditor.vue';
import TagEditor from './TagEditor.vue';

const props = defineProps({
  isFieldInError: {
    type: Function,
    required: true,
  },
  sourceRecipe: {
    type: Object as PropType<GetRecipeResponse>,
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
    default: () => {
      /* do nothing */
    },
  },
  isCreateMode: {
    type: Boolean,
    required: true,
  },
});

const workingRecipe: Ref<SaveRecipeRequestClass> = ref(new SaveRecipeRequestClass());

function reset() {
  workingRecipe.value = {
    ...new SaveRecipeRequestClass(),
    ...props.sourceRecipe,
  };
}

function addCategory(tag: string) {
  const categoryName = trimAndTitleCase(tag);

  const categories = workingRecipe.value.categories?.slice() || [];

  const categoryDoesNotExist =
    categories.map((value) => value.toUpperCase()).indexOf(categoryName.toUpperCase()) < 0;

  if (categoryDoesNotExist && categoryName.length > 0) {
    categories.push(categoryName);
    workingRecipe.value.categories = categories;
  }
}

function removeCategory(categoryName: string) {
  const categories = workingRecipe.value.categories?.slice() || [];
  const categoryIndex = categories.indexOf(categoryName);

  if (categoryIndex > -1) {
    categories.splice(categoryIndex, 1);
    workingRecipe.value.categories = categories;
  }
}

function saveClick() {
  const sendableRecipe: SaveRecipeRequestClass = JSON.parse(JSON.stringify(workingRecipe.value));
  props.onRecipeSave(sendableRecipe);
}

function getCopy() {
  return { ...workingRecipe.value };
}

watch(
  () => props.sourceRecipe,
  () => {
    reset();
  }
);

const isRecipeDirty = computed(() => {
  return (
    Object.entries(workingRecipe.value).find(
      // Loose comparison so numbers and strings of numbers are equal.
      // eslint-disable-next-line eqeqeq
      ([key, value]) => value != props.sourceRecipe[key as keyof GetRecipeResponse]
    ) === undefined
  );
});

watch(
  () => isRecipeDirty,
  (newValue) => {
    props.onRecipeDirtyStateChange(newValue);
  }
);

onMounted(() => {
  reset();
});
</script>

<template>
  <form id="recipe-details-form" name="recipe-details-form">
    <div class="row">
      <div class="col-md-12 mb-3">
        <label for="name" class="form-label">Name *</label>
        <input
          id="name"
          v-model="workingRecipe.name"
          required
          type="text"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('name') }"
        />
      </div>
      <div class="col-md-12 mb-3">
        <label for="ingredients" class="form-label">Ingredients *</label>
        <!-- TODO: make ingredient editor -->
        <textarea
          id="ingredients"
          v-model="workingRecipe.ingredients"
          required
          rows="1"
          :max-rows="Number.MAX_SAFE_INTEGER"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('ingredients') }"
        />
      </div>
      <div class="col-md-12 mb-3">
        <label for="directions" class="form-label">Directions *</label>
        <textarea
          id="directions"
          v-model="workingRecipe.directions"
          required
          rows="1"
          :max-rows="Number.MAX_SAFE_INTEGER"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('directions') }"
        />
      </div>
      <div class="col-md-12 mb-3">
        <label for="isForMealPlanning" class="form-label">For meal planning</label>
        <b-form-checkbox
          id="isForMealPlanning"
          v-model="workingRecipe.isForMealPlanning"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('isForMealPlanning') }"
        />
      </div>
      <div class="col-md-6">
        <label for="prepTimeMinutes" class="form-label">Prep Time Hours/Minutes</label>
        <RecipeTimeSpanEditor
          id="prepTimeMinutes"
          v-model="workingRecipe.prepTimeMinutes"
          :is-invalid="isFieldInError('prepTimeMinutes')"
          show-preview
        />
      </div>
      <div class="col-md-6">
        <label for="cookTimeMinutes" class="form-label">Cook Time Hours/Minutes</label>
        <RecipeTimeSpanEditor
          id="cookTimeMinutes"
          v-model="workingRecipe.cookTimeMinutes"
          :is-invalid="isFieldInError('cookTimeMinutes')"
          show-preview
        />
      </div>
      <div class="col-md-12 mb-3">
        <TagEditor
          :class="{ 'form-group': true, danger: isFieldInError('categories') }"
          :tags="workingRecipe.categories || []"
          :on-add-tag="addCategory"
          :on-remove-tag="removeCategory"
          field-name="categories"
          label="Categories"
        />
      </div>
    </div>
    <EntityAuditInfo v-if="sourceRecipe.id" class="mb-3" :entity="sourceRecipe" />
    <div class="row">
      <div class="col-md-12 mb-3">
        <b-button-toolbar>
          <b-button class="me-2" variant="primary" @click.stop.prevent="saveClick()">
            Save
          </b-button>
          <b-button
            v-if="!isCreateMode"
            :to="{ name: 'new', params: { newRecipeSuggestion: getCopy() } }"
            class="me-2"
          >
            Copy
          </b-button>
          <b-button v-if="!isCreateMode" :to="{ name: 'view', params: { id: sourceRecipe.id } }">
            Cancel
          </b-button>
          <b-button
            v-if="!isCreateMode"
            class="ms-auto"
            variant="danger"
            @click.stop.prevent="onRecipeDelete(workingRecipe.id)"
          >
            Delete
          </b-button>
        </b-button-toolbar>
      </div>
    </div>
  </form>
</template>

<style lang="scss" scoped>
textarea {
  overflow: hidden !important;
  resize: none;
}
</style>
