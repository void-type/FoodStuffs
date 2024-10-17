<script lang="ts" setup>
import WorkingRecipe from '@/models/WorkingRecipe';
import type { GetRecipeResponse, ListShoppingItemsResponse } from '@/api/data-contracts';
import { isNil, trimAndTitleCase } from '@/models/FormatHelpers';
import { computed, reactive, watch, type PropType, onMounted, ref } from 'vue';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import RouterHelpers from '@/models/RouterHelpers';
import useMessageStore from '@/stores/messageStore';
import WorkingRecipeIngredient from '@/models/WorkingRecipeIngredient';
import WorkingRecipeShoppingItem from '@/models/WorkingRecipeShoppingItem';
import EntityAuditInfo from './EntityAuditInfo.vue';
import RecipeTimeSpanEditor from './RecipeTimeSpanEditor.vue';
import TagEditor from './TagEditor.vue';
import RecipeEditorIngredients from './RecipeEditorIngredients.vue';
import RecipeMealButton from './RecipeMealButton.vue';
import RecipeEditorShoppingItems from './RecipeEditorShoppingItems.vue';

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
});

const messageStore = useMessageStore();
const api = ApiHelpers.client;

// This is a snapshot of our source recipe right after it became a working recipe so we can check if working is dirty.
let workingRecipeInitial = '';

const data = reactive({
  workingRecipe: new WorkingRecipe(),
});

const categoryOptions = ref([] as Array<string>);

async function fetchCategories() {
  try {
    const response = await api().categoriesList({ isPagingEnabled: false });
    categoryOptions.value =
      response.data.items?.map((x) => x.name || '').filter((x) => !isNil(x)) || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

const shoppingItemOptions = ref([] as Array<ListShoppingItemsResponse>);

async function fetchShoppingItems() {
  try {
    const response = await api().shoppingItemsList({ isPagingEnabled: false });
    shoppingItemOptions.value = (response.data.items || []) as Array<ListShoppingItemsResponse>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

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

  ingredients.sort((a, b) => (a.order || 0) - (b.order || 0));

  const shoppingItems = (props.sourceRecipe.shoppingItems || []).map((x) => ({
    ...new WorkingRecipeShoppingItem(),
    ...x,
  }));

  shoppingItems.sort((a, b) => (a.order || 0) - (b.order || 0));

  const newWorking: WorkingRecipe = {
    ...newWorkingClass,
    ...sourceCopy,
    ingredients,
    directions: props.sourceRecipe.directions || '',
    sides: props.sourceRecipe.sides || '',
    shoppingItems,
  };

  workingRecipeInitial = JSON.stringify(newWorking);

  data.workingRecipe = newWorking;
}

async function createShoppingItem(name: string) {
  try {
    const response = await api().shoppingItemsSave({ name });

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }
    await fetchShoppingItems();
    return response.data.id;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    return null;
  }
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
  fetchCategories();
  fetchShoppingItems();
}

watch(
  () => props.sourceRecipe,
  () => {
    reset();
  },
  { immediate: true }
);

const isRecipeDirty = computed(() => JSON.stringify(data.workingRecipe) !== workingRecipeInitial);

watch(isRecipeDirty, () => {
  props.onRecipeDirtyStateChange(isRecipeDirty.value);
});

onMounted(() => {
  fetchCategories();
  fetchShoppingItems();
});
</script>

<template>
  <form id="recipe-details-form" name="recipe-details-form">
    <div class="grid">
      <div class="btn-toolbar g-col-12">
        <button type="button" class="btn btn-primary me-2" @click.stop.prevent="saveClick()">
          Save
        </button>
        <router-link
          v-if="isEditMode"
          type="button"
          class="btn btn-secondary me-2"
          :to="{ name: 'recipeNew', query: { copy: sourceRecipe.id } }"
        >
          Copy
        </router-link>
        <RecipeMealButton v-if="isEditMode" class="me-2" :recipe-id="sourceRecipe.id" />
        <router-link
          v-if="isEditMode"
          type="button"
          class="btn btn-secondary me-2"
          :to="RouterHelpers.viewRecipe(sourceRecipe)"
        >
          Cancel
        </router-link>
        <button
          v-if="isEditMode"
          type="button"
          class="btn btn-danger d-inline ms-auto"
          @click.stop.prevent="onRecipeDelete(data.workingRecipe.id)"
        >
          Delete
        </button>
      </div>
      <div class="g-col-12 g-col-md-6">
        <div>
          <label for="name" class="form-label">Name*</label>
          <input
            id="name"
            v-model="data.workingRecipe.name"
            required
            type="text"
            :class="{ 'form-control': true, 'is-invalid': messageStore.isFieldInError('name') }"
          />
        </div>
        <div class="mt-4">
          <label for="directions" class="form-label">Directions</label>
          <textarea
            id="directions"
            v-model="data.workingRecipe.directions"
            rows="10"
            :class="{
              'form-control': true,
              'is-invalid': messageStore.isFieldInError('directions'),
            }"
          />
        </div>
        <div class="mt-4">
          <label for="sides" class="form-label">Sides</label>
          <textarea
            id="sides"
            v-model="data.workingRecipe.sides"
            rows="10"
            :class="{
              'form-control': true,
              'is-invalid': messageStore.isFieldInError('sides'),
            }"
          />
        </div>
      </div>
      <div class="grid g-col-12 g-col-md-6">
        <div class="g-col-12">
          <label for="ingredients" class="form-label">Shopping Items</label>
          <RecipeEditorShoppingItems
            v-model="data.workingRecipe.shoppingItems as WorkingRecipeShoppingItem[]"
            :is-field-in-error="messageStore.isFieldInError"
            :suggestions="shoppingItemOptions"
            :on-create-item="createShoppingItem"
          />
        </div>
        <div class="g-col-12">
          <label for="ingredients" class="form-label">Ingredients</label>
          <RecipeEditorIngredients
            v-model="data.workingRecipe.ingredients"
            :is-field-in-error="messageStore.isFieldInError"
          />
        </div>
      </div>
      <div class="g-col-12 g-col-md-6">
        <TagEditor
          :class="{ danger: messageStore.isFieldInError('categories') }"
          :tags="data.workingRecipe.categories || []"
          :on-add-tag="addCategory"
          :on-remove-tag="removeCategory"
          :suggestions="categoryOptions"
          field-name="categories"
          label="Categories"
        />
      </div>
      <div class="g-col-12 g-col-md-6">
        <div class="form-check mt-4">
          <input
            id="isForMealPlanning"
            v-model="data.workingRecipe.isForMealPlanning"
            class="form-check-input"
            type="checkbox"
            :class="{ 'is-invalid': messageStore.isFieldInError('isForMealPlanning') }"
          />
          <label for="isForMealPlanning" class="form-check-label">For meal planning</label>
        </div>
      </div>
      <div class="g-col-12 g-col-md-6">
        <label for="prepTimeMinutes" class="form-label">Prep Time Hours/Minutes</label>
        <RecipeTimeSpanEditor
          id="prepTimeMinutes"
          v-model="data.workingRecipe.prepTimeMinutes"
          :is-invalid="messageStore.isFieldInError('prepTimeMinutes')"
        />
      </div>
      <div class="g-col-12 g-col-md-6">
        <label for="cookTimeMinutes" class="form-label">Cook Time Hours/Minutes</label>
        <RecipeTimeSpanEditor
          id="cookTimeMinutes"
          v-model="data.workingRecipe.cookTimeMinutes"
          :is-invalid="messageStore.isFieldInError('cookTimeMinutes')"
        />
      </div>
      <EntityAuditInfo v-if="sourceRecipe.id" class="g-col-12" :entity="sourceRecipe" />
    </div>
  </form>
</template>

<style lang="scss" scoped>
textarea {
  resize: vertical;
}
</style>
