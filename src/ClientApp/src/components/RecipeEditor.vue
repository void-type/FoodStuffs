<script lang="ts" setup>
import WorkingRecipe from '@/models/WorkingRecipe';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { trimAndTitleCase } from '@/models/FormatHelpers';
import { computed, onMounted, reactive, watch, type PropType } from 'vue';
import { useRouter } from 'vue-router';
import WorkingRecipeIngredient from '@/models/WorkingRecipeIngredient';
import EntityAuditInfo from './EntityAuditInfo.vue';
import RecipeTimeSpanEditor from './RecipeTimeSpanEditor.vue';
import TagEditor from './TagEditor.vue';
import RecipeEditorIngredients from './RecipeEditorIngredients.vue';

const props = defineProps({
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
  isEditMode: {
    type: Boolean,
    required: true,
  },
  isFieldInError: {
    type: Function,
    required: true,
  },
});

const router = useRouter();

// This is a snapshot of our source recipe right after it became a working recipe so we can check if working is dirty.
let workingRecipeInitial = '';

const data = reactive({
  workingRecipe: new WorkingRecipe(),
});

function reset() {
  const sourceCopy: Record<string, unknown> = JSON.parse(JSON.stringify(props.sourceRecipe));

  const newWorkingClass = new WorkingRecipe();

  const validProperties = Object.keys(newWorkingClass);

  // Remove properties that aren't in the request class.
  Object.keys(sourceCopy).forEach((key) => {
    if (!validProperties.includes(key)) {
      delete sourceCopy[key];
    }
  });

  const ingredients = (props.sourceRecipe.ingredients || []).map((x) => ({
    ...new WorkingRecipeIngredient(),
    ...x,
  }));

  const newWorking: WorkingRecipe = {
    ...newWorkingClass,
    ...sourceCopy,
    ingredients,
    directions: props.sourceRecipe.directions || '',
  };

  workingRecipeInitial = JSON.stringify(newWorking);

  data.workingRecipe = newWorking;
}

function addCategory(tag: string) {
  const categoryName = trimAndTitleCase(tag);

  const categories = data.workingRecipe.categories?.slice() || [];

  const categoryDoesNotExist =
    categories.map((value) => value.toUpperCase()).indexOf(categoryName.toUpperCase()) < 0;

  if (categoryDoesNotExist && categoryName.length > 0) {
    categories.push(categoryName);
    data.workingRecipe.categories = categories;
  }
}

function removeCategory(categoryName: string) {
  const categories = data.workingRecipe.categories?.slice() || [];
  const categoryIndex = categories.indexOf(categoryName);

  if (categoryIndex > -1) {
    categories.splice(categoryIndex, 1);
    data.workingRecipe.categories = categories;
  }
}

function saveClick() {
  props.onRecipeSave(data.workingRecipe);
}

watch(
  () => props.sourceRecipe,
  () => {
    reset();
  }
);

const isRecipeDirty = computed(() => JSON.stringify(data.workingRecipe) !== workingRecipeInitial);

watch(isRecipeDirty, () => {
  props.onRecipeDirtyStateChange(isRecipeDirty.value);
});

onMounted(() => {
  reset();
});
</script>

<template>
  <form id="recipe-details-form" name="recipe-details-form">
    <div class="grid">
      <div class="g-col-12">
        <label for="name" class="form-label">Name *</label>
        <input
          id="name"
          v-model="data.workingRecipe.name"
          required
          type="text"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('name') }"
        />
      </div>
      <div class="g-col-12">
        <label for="ingredients" class="form-label">Ingredients</label>
        <RecipeEditorIngredients
          v-model="data.workingRecipe.ingredients"
          :is-field-in-error="isFieldInError"
        />
      </div>
      <div class="g-col-12">
        <label for="directions" class="form-label">Directions</label>
        <textarea
          id="directions"
          v-model="data.workingRecipe.directions"
          rows="10"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('directions') }"
        />
      </div>
      <div class="g-col-12 g-col-md-6">
        <label for="prepTimeMinutes" class="form-label">Prep Time Hours/Minutes</label>
        <RecipeTimeSpanEditor
          id="prepTimeMinutes"
          v-model="data.workingRecipe.prepTimeMinutes"
          :is-invalid="isFieldInError('prepTimeMinutes')"
        />
      </div>
      <div class="g-col-12 g-col-md-6">
        <label for="cookTimeMinutes" class="form-label">Cook Time Hours/Minutes</label>
        <RecipeTimeSpanEditor
          id="cookTimeMinutes"
          v-model="data.workingRecipe.cookTimeMinutes"
          :is-invalid="isFieldInError('cookTimeMinutes')"
        />
      </div>
      <div class="g-col-12">
        <TagEditor
          :class="{ 'form-group': true, danger: isFieldInError('categories') }"
          :tags="data.workingRecipe.categories || []"
          :on-add-tag="addCategory"
          :on-remove-tag="removeCategory"
          field-name="categories"
          label="Categories"
        />
      </div>
      <div class="g-col-12">
        <div class="form-check">
          <input
            id="isForMealPlanning"
            v-model="data.workingRecipe.isForMealPlanning"
            class="form-check-input"
            type="checkbox"
            :class="{ 'is-invalid': isFieldInError('isForMealPlanning') }"
          />
          <label for="isForMealPlanning" class="form-check-label">For meal planning</label>
        </div>
      </div>
      <EntityAuditInfo v-if="sourceRecipe.id" class="g-col-12" :entity="sourceRecipe" />
      <div class="btn-toolbar g-col-12">
        <button class="btn btn-primary me-2" @click.stop.prevent="saveClick()">Save</button>
        <button
          v-if="isEditMode"
          class="btn btn-secondary me-2"
          @click.stop.prevent="() => router.push({ name: 'new', query: { copy: sourceRecipe.id } })"
        >
          Copy
        </button>
        <button
          v-if="isEditMode"
          class="btn btn-secondary me-2"
          @click.stop.prevent="() => router.push({ name: 'view', params: { id: sourceRecipe.id } })"
        >
          Cancel
        </button>
        <button
          v-if="isEditMode"
          class="btn btn-danger d-inline ms-auto"
          @click.stop.prevent="onRecipeDelete(data.workingRecipe.id)"
        >
          Delete
        </button>
      </div>
    </div>
  </form>
</template>

<style lang="scss" scoped>
textarea {
  resize: vertical;
}
</style>
