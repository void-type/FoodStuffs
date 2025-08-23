<script lang="ts" setup>
import RecipeWorking from '@/models/RecipeWorking';
import type {
  GetRecipeResponse,
  IItemSetOfIFailure,
  ListCategoriesResponse,
  ListGroceryItemsResponse,
} from '@/api/data-contracts';
import { trimAndTitleCase } from '@/models/FormatHelper';
import { computed, reactive, watch, type PropType, onMounted, ref } from 'vue';
import type { HttpResponse } from '@/api/http-client';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useMessageStore from '@/stores/messageStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import { getCurrentMealPlanFromStorage } from '@/models/MealPlanStoreHelper';
import RecipeGroceryItemWorking from '@/models/RecipeGroceryItemWorking';
import type { Tag } from '@/models/Tag';
import EntityAuditInfo from './EntityAuditInfo.vue';
import RecipeTimeSpanEditor from './RecipeTimeSpanEditor.vue';
import TagEditor from './TagEditor.vue';
import RecipeMealButton from './RecipeMealButton.vue';
import RecipeEditorGroceryItems from './RecipeEditorGroceryItems.vue';
import RichTextEditor from './RichTextEditor.vue';

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
const mealPlanStore = useMealPlanStore();
const api = ApiHelper.client;

const data = reactive({
  workingRecipe: new RecipeWorking(),
  // This is a snapshot of our source recipe right after it became a working recipe so we can check if working is dirty.
  workingRecipeInitial: '',
});

const categoryOptions = ref([] as Array<ListCategoriesResponse>);

async function fetchCategories() {
  try {
    const response = await api().categoriesList({ isPagingEnabled: false });
    categoryOptions.value = response.data.items || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

const groceryItemOptions = ref([] as Array<ListGroceryItemsResponse>);

async function fetchGroceryItems() {
  try {
    const response = await api().groceryItemsList({ isPagingEnabled: false });
    groceryItemOptions.value = (response.data.items || []) as Array<ListGroceryItemsResponse>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function reset() {
  const sourceCopy: Record<string, unknown> = JSON.parse(JSON.stringify(props.sourceRecipe));

  const newWorkingClass = new RecipeWorking();

  const validProperties = Object.keys(newWorkingClass);

  // Remove properties that aren't in the request class.
  Object.keys(sourceCopy).forEach((key) => {
    if (!validProperties.includes(key)) {
      delete sourceCopy[key];
    }
  });

  const groceryItems = (props.sourceRecipe.groceryItems || []).map((x) => ({
    ...new RecipeGroceryItemWorking(),
    ...x,
  }));

  groceryItems.sort((a, b) => (a.order || 0) - (b.order || 0));

  const newWorking: RecipeWorking = {
    ...newWorkingClass,
    ...sourceCopy,
    directions: props.sourceRecipe.directions || '',
    sides: props.sourceRecipe.sides || '',
    groceryItems,
  };

  data.workingRecipeInitial = JSON.stringify(newWorking);

  data.workingRecipe = newWorking;
}

async function createGroceryItem(name: string) {
  try {
    const response = await api().groceryItemsSave({ name, storageLocations: [] });

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }
    await fetchGroceryItems();
    return response.data.id;
  } catch (error) {
    const response = error as HttpResponse<unknown, unknown>;
    const failureSet = response.error as IItemSetOfIFailure;

    // Add sub-type prefix to uiHandle
    if (typeof failureSet !== 'undefined' && failureSet !== null) {
      failureSet.items?.forEach((failure) => {
        // eslint-disable-next-line no-param-reassign
        failure.uiHandle = `groceryItems-${failure.uiHandle}`;
      });
    }

    messageStore.setApiFailureMessages(response);
    return null;
  }
}

function addCategory(tag: string) {
  const categoryName = trimAndTitleCase(tag);

  const categories = data.workingRecipe.categories?.slice() || [];

  const categoryDoesNotExist =
    categories.map((value) => value?.name?.toUpperCase()).indexOf(categoryName.toUpperCase()) < 0;

  if (categoryDoesNotExist && categoryName.length > 0) {
    categories.push({ id: 0, name: categoryName, color: '#000000' });
    data.workingRecipe.categories = categories;
  }
}

function removeCategory(category: Tag) {
  const categories = data.workingRecipe.categories?.slice() || [];
  const categoryIndex = categories.findIndex((x) => x.name === category.name);

  if (categoryIndex > -1) {
    categories.splice(categoryIndex, 1);
    data.workingRecipe.categories = categories;
  }
}

async function saveClick() {
  await props.onRecipeSave(data.workingRecipe);
  fetchCategories();
  fetchGroceryItems();
  await mealPlanStore.setCurrentMealPlan(getCurrentMealPlanFromStorage());
}

watch(
  () => props.sourceRecipe,
  () => {
    reset();
  },
  { immediate: true }
);

const isRecipeDirty = computed(() => {
  const working: RecipeWorking = JSON.parse(JSON.stringify(data.workingRecipe));
  working.groceryItems.forEach((x) => {
    // eslint-disable-next-line no-param-reassign
    delete x.inventoryQuantity;
  });
  const workingJson = JSON.stringify(working);

  const initial: RecipeWorking = JSON.parse(data.workingRecipeInitial);
  initial.groceryItems?.forEach((x) => {
    // eslint-disable-next-line no-param-reassign
    delete x.inventoryQuantity;
  });
  const initialJson = JSON.stringify(initial);

  return workingJson !== initialJson;
});

watch(isRecipeDirty, () => {
  props.onRecipeDirtyStateChange(isRecipeDirty.value);
});

onMounted(async () => {
  fetchCategories();
  fetchGroceryItems();
});
</script>

<template>
  <div>
    <div class="btn-toolbar sticky-top pt-1">
      <button type="button" class="btn btn-primary me-2" @click.stop.prevent="saveClick()">
        Save
      </button>
      <RecipeMealButton v-if="isEditMode" class="me-2" :recipe-id="sourceRecipe.id" />
      <div class="dropdown">
        <button
          v-if="isEditMode"
          id="overflowMenuButton"
          class="btn btn-secondary dropdown-toggle"
          type="button"
          data-bs-toggle="dropdown"
          aria-expanded="false"
        >
          More
        </button>
        <ul v-if="isEditMode" class="dropdown-menu" aria-labelledby="overflowMenuButton">
          <li>
            <router-link class="dropdown-item" :to="RouterHelper.viewRecipe(sourceRecipe)">
              View
            </router-link>
          </li>
          <li>
            <router-link
              class="dropdown-item"
              :to="{ name: 'recipeNew', query: { copy: sourceRecipe.id } }"
            >
              Copy
            </router-link>
          </li>
          <li>
            <button
              class="dropdown-item text-danger"
              @click.stop.prevent="onRecipeDelete(sourceRecipe.id)"
            >
              Delete
            </button>
          </li>
        </ul>
      </div>
    </div>
    <div class="grid mt-3">
      <div class="g-col-12 g-col-md-6">
        <label for="name" class="form-label">Name</label>
        <input
          id="name"
          v-model="data.workingRecipe.name"
          required
          type="text"
          :class="{ 'form-control': true, 'is-invalid': messageStore.isFieldInError('name') }"
        />
      </div>
      <TagEditor
        :class="{
          'g-col-12 g-col-md-6': true,
          danger: messageStore.isFieldInError('categories'),
        }"
        :tags="data.workingRecipe.categories"
        :on-add-tag="addCategory"
        :on-remove-tag="removeCategory"
        :suggestions="categoryOptions"
        field-name="categories"
        label="Categories"
      />
      <div class="g-col-12">
        <div class="form-check">
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
        <label for="grocery-item-list" class="form-label">Grocery items</label>
        <RecipeEditorGroceryItems
          v-model="data.workingRecipe.groceryItems as RecipeGroceryItemWorking[]"
          :is-field-in-error="messageStore.isFieldInError"
          :suggestions="groceryItemOptions"
          :on-create-item="createGroceryItem"
        />
      </div>
      <div class="g-col-12 g-col-md-6">
        <label for="sides" class="form-label">Sides</label>
        <textarea
          id="sides"
          v-model="data.workingRecipe.sides"
          rows="5"
          :class="{
            'form-control': true,
            'is-invalid': messageStore.isFieldInError('sides'),
          }"
        />
      </div>
      <div class="g-col-12">
        <label for="directions" class="form-label">Directions</label>
        <RichTextEditor
          id="directions"
          v-model="data.workingRecipe.directions"
          :class="{
            'is-invalid': messageStore.isFieldInError('directions'),
          }"
        />
      </div>
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-md-6">
            <label for="prepTimeMinutes" class="form-label">Prep time hours/minutes</label>
            <RecipeTimeSpanEditor
              id="prepTimeMinutes"
              v-model="data.workingRecipe.prepTimeMinutes"
              :is-invalid="messageStore.isFieldInError('prepTimeMinutes')"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <label for="cookTimeMinutes" class="form-label">Cook time hours/minutes</label>
            <RecipeTimeSpanEditor
              id="cookTimeMinutes"
              v-model="data.workingRecipe.cookTimeMinutes"
              :is-invalid="messageStore.isFieldInError('cookTimeMinutes')"
            />
          </div>
        </div>
      </div>
      <EntityAuditInfo v-if="sourceRecipe.id" class="g-col-12" :entity="sourceRecipe" />
    </div>
  </div>
</template>

<style lang="scss" scoped>
textarea {
  resize: vertical;
}
</style>
