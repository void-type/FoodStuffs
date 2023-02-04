<script lang="ts" setup>
import SaveRecipeRequestClass from '@/models/SaveRecipeRequestClass';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { trimAndTitleCase } from '@/models/FormatHelpers';
import { computed, onMounted, ref, watch, type PropType, type Ref } from 'vue';
import { useRouter } from 'vue-router';
import EntityAuditInfo from './EntityAuditInfo.vue';
import RecipeTimeSpanEditor from './RecipeTimeSpanEditor.vue';
import TagEditor from './TagEditor.vue';
import RecipeViewerIngredients from './RecipeViewerIngredients.vue';

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
  isEditMode: {
    type: Boolean,
    required: true,
  },
});

const router = useRouter();

const workingRecipe: Ref<SaveRecipeRequestClass> = ref(new SaveRecipeRequestClass());

function reset() {
  const sourceCopy: Record<string, unknown> = JSON.parse(JSON.stringify(props.sourceRecipe));

  const newWorkingClass = new SaveRecipeRequestClass();

  const validProperties = Object.keys(newWorkingClass);

  // Remove properties that aren't in the request class.
  Object.keys(sourceCopy).forEach((key) => {
    if (!validProperties.includes(key)) {
      delete sourceCopy[key];
    }
  });

  const newWorking: SaveRecipeRequestClass = {
    ...newWorkingClass,
    ...sourceCopy,
    ingredients: props.sourceRecipe.ingredients || [],
    directions: props.sourceRecipe.directions || '',
  };

  workingRecipe.value = newWorking;
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
  props.onRecipeSave(workingRecipe.value);
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
  () => {
    props.onRecipeDirtyStateChange(isRecipeDirty);
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
      <!-- TODO: make ingredient editor -->
      <!-- <div class="col-md-12 mb-3">
        <label for="ingredients" class="form-label">Ingredients *</label>
        <textarea
          id="ingredients"
          v-model="workingRecipe.ingredients"
          required
          rows="1"
          :max-rows="Number.MAX_SAFE_INTEGER"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('ingredients') }"
        />
      </div> -->
      <div class="col-md-12 mb-3">
        <RecipeViewerIngredients :ingredients="workingRecipe.ingredients" />
      </div>
      <div class="col-md-12 mb-3">
        <label for="directions" class="form-label">Directions *</label>
        <textarea
          id="directions"
          v-model="workingRecipe.directions"
          required
          rows="10"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('directions') }"
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
      <div class="col-md-12 mb-3">
        <div class="form-check">
          <input
            id="isForMealPlanning"
            v-model="workingRecipe.isForMealPlanning"
            class="form-check-input"
            type="checkbox"
            :class="{ 'is-invalid': isFieldInError('isForMealPlanning') }"
          />
          <label for="isForMealPlanning" class="form-check-label">For meal planning</label>
        </div>
      </div>
    </div>
    <EntityAuditInfo v-if="sourceRecipe.id" class="mb-3" :entity="sourceRecipe" />
    <div class="w-100 mb-3 d-flex">
      <button class="btn btn-primary me-2" @click.stop.prevent="saveClick()">Save</button>
      <button
        v-if="isEditMode"
        class="btn btn-secondary me-2"
        @click="() => router.push({ name: 'new', query: { copy: sourceRecipe.id } })"
      >
        <!-- TODO: copy sometimes takes me to edit with a categories query string. -->
        Copy
      </button>
      <button
        v-if="isEditMode"
        class="btn btn-secondary me-2"
        @click="() => router.push({ name: 'view', params: { id: sourceRecipe.id } })"
      >
        Cancel
      </button>
      <button
        v-if="isEditMode"
        class="btn btn-danger d-inline ms-auto"
        @click.stop.prevent="onRecipeDelete(workingRecipe.id)"
      >
        Delete
      </button>
    </div>
  </form>
</template>

<style lang="scss" scoped>
textarea {
  resize: vertical;
}
</style>
