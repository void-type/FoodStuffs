<script lang="ts" setup>
import SaveRecipeRequestClass from '@/models/SaveRecipeRequestClass';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { trimAndTitleCase } from '@/models/FormatHelpers';
import { onMounted, ref, watch, type PropType, type Ref } from 'vue';
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
const isRecipeDirty = ref(false);

function reset() {
  workingRecipe.value = { ...props.sourceRecipe };
  isRecipeDirty.value = false;
}

function addCategory(tag: string) {
  const categoryName = trimAndTitleCase(tag);

  const categories = workingRecipe.value.categories.slice();

  const categoryDoesNotExist =
    categories.map((value) => value.toUpperCase()).indexOf(categoryName.toUpperCase()) < 0;

  if (categoryDoesNotExist && categoryName.length > 0) {
    categories.push(categoryName);
    workingRecipe.value.categories = categories;
  }
}

function removeCategory(categoryName: string) {
  const categories = workingRecipe.value.categories.slice();
  const categoryIndex = categories.indexOf(categoryName);

  if (categoryIndex > -1) {
    categories.splice(categoryIndex, 1);
    workingRecipe.value.categories = categories;
  }
}

function saveClick() {
  const sendableRecipe = new SaveRecipeRequestClass();

  Object.keys(sendableRecipe).forEach((key) => {
    sendableRecipe[key] = workingRecipe[key];
  });

  props.onRecipeSave(sendableRecipe);
}

function getCopy() {
  return { ...workingRecipe, images: [] };
}

watch(
  () => props.sourceRecipe,
  () => {
    reset();
  }
);

// Probably just use a computed
watch(
  () => workingRecipe.value,
  () => {
    const wRecipe = workingRecipe.value;
    const changedValues = Object.keys(wRecipe)
      // Loose comparison so numbers and strings of numbers are equal.
      .map((key) => wRecipe[key] == props.sourceRecipe[key])
      .filter((value) => value === false);

    const isDirty = changedValues.length > 0;
    isRecipeDirty.value = isDirty;
  }
);

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
  <form
    id="recipe-details-form"
    name="recipe-details-form"
    @keydown.ctrl.enter.stop.prevent="saveClick()"
  >
    <b-form-row>
      <b-col md="12" sm="6" />
      <b-col md="12">
        <b-form-group label="Name *" label-for="name">
          <b-form-input
            id="name"
            v-model="workingRecipe.name"
            required
            :class="{ 'is-invalid': isFieldInError('name') }"
          />
        </b-form-group>
      </b-col>
      <b-col md="12">
        <b-form-group label="Ingredients *" label-for="ingredients">
          <b-form-textarea
            id="ingredients"
            v-model="workingRecipe.ingredients"
            required
            rows="1"
            :max-rows="Number.MAX_SAFE_INTEGER"
            :class="{ 'is-invalid': isFieldInError('ingredients') }"
          />
        </b-form-group>
      </b-col>
      <b-col md="12">
        <b-form-group label="Directions *" label-for="directions">
          <b-form-textarea
            id="directions"
            v-model="workingRecipe.directions"
            required
            rows="1"
            :max-rows="Number.MAX_SAFE_INTEGER"
            :class="{ 'is-invalid': isFieldInError('directions') }"
          />
        </b-form-group>
      </b-col>
      <b-col sm="12" md="6">
        <b-form-group label="Prep Time Hours/Minutes" label-for="prepTimeMinutes">
          <RecipeTimeSpanEditor
            id="prepTimeMinutes"
            v-model="workingRecipe.prepTimeMinutes"
            :is-invalid="isFieldInError('prepTimeMinutes')"
            show-preview
          />
        </b-form-group>
      </b-col>
      <b-col sm="12" md="6">
        <b-form-group label="Cook Time Hours/Minutes" label-for="cookTimeMinutes">
          <RecipeTimeSpanEditor
            id="cookTimeMinutes"
            v-model="workingRecipe.cookTimeMinutes"
            :is-invalid="isFieldInError('cookTimeMinutes')"
            show-preview
          />
        </b-form-group>
      </b-col>
      <b-col md="12">
        <TagEditor
          :class="{ 'form-group': true, danger: isFieldInError('categories') }"
          :tags="workingRecipe.categories"
          :on-add-tag="addCategory"
          :on-remove-tag="removeCategory"
          field-name="categories"
          label="Categories"
        />
      </b-col>
    </b-form-row>
    <EntityAuditInfo v-if="sourceRecipe.id" class="mb-3" :entity="sourceRecipe" />
    <b-form-row>
      <b-col md="12">
        <b-button-toolbar>
          <b-button class="mr-2" variant="primary" @click.stop.prevent="saveClick()">
            Save
          </b-button>
          <b-button
            v-if="!isCreateMode"
            :to="{ name: 'new', params: { newRecipeSuggestion: getCopy() } }"
            class="mr-2"
          >
            Copy
          </b-button>
          <b-button v-if="!isCreateMode" :to="{ name: 'view', params: { id: sourceRecipe.id } }">
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

<style lang="scss" scoped>
textarea {
  overflow: hidden !important;
  resize: none;
}
</style>
