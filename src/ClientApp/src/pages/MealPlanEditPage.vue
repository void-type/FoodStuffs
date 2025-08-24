<script lang="ts" setup>
import { computed, nextTick, onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';
import MealPlanRecipeCard from '@/components/MealPlanRecipeCard.vue';
import MealPlanGroceryItemList from '@/components/MealPlanGroceryItemList.vue';
import useMessageStore from '@/stores/messageStore';
import ApiHelper from '@/models/ApiHelper';
import { VueDraggable } from 'vue-draggable-plus';
import type {
  ListGroceryAislesResponse,
  ListGroceryItemsResponse,
  GetMealPlanResponseRecipe,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import { useRouter } from 'vue-router';
import RouterHelper from '@/models/RouterHelper';

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();
const messageStore = useMessageStore();
const router = useRouter();
const api = ApiHelper.client;

const initialized = ref(false);

const { currentMealPlan } = storeToRefs(mealPlanStore);

const currentRecipes = computed({
  get() {
    return currentMealPlan.value?.recipes || [];
  },
  set(value) {
    if (currentMealPlan.value === undefined || currentMealPlan.value === null) {
      return;
    }

    currentMealPlan.value.recipes = value;
  },
});

const shoppingList = computed(() => mealPlanStore.currentShoppingList);

const pantry = computed(() => mealPlanStore.currentPantry);

const showSortHandle = ref(false);

async function addToPantry(id: number) {
  await mealPlanStore.addToCurrentPantry(id);
}

async function removeFromPantry(id: number) {
  await mealPlanStore.removeFromCurrentPantry(id);
}

async function clearPantry() {
  await mealPlanStore.clearCurrentPantry();
}

async function onClearRecipes() {
  const parameters: ModalParameters = {
    title: 'Clear meal plan',
    description: 'Do you really want to remove all recipes from this meal plan?',
    okAction: () => mealPlanStore.clearCurrentRecipes(),
  };

  appStore.showModal(parameters);
}

async function onSaveMealPlan() {
  await mealPlanStore.saveCurrentMealPlan();
}

async function onDeleteMealPlan() {
  const parameters: ModalParameters = {
    title: 'Delete meal plan',
    description: 'Do you really want to delete this meal plan?',
    okAction: () => mealPlanStore.deleteMealPlan(currentMealPlan.value.id),
  };

  appStore.showModal(parameters);
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

function findGroceryItem(id: number | undefined) {
  return groceryItemOptions.value.find((x) => x.id === id);
}

const groceryAisleOptions = ref([] as Array<ListGroceryAislesResponse>);

async function fetchGroceryAisles() {
  try {
    const response = await api().groceryAislesList({ isPagingEnabled: false });
    groceryAisleOptions.value = (response.data.items || []) as Array<ListGroceryAislesResponse>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function findGroceryAisle(id: number | undefined) {
  return groceryAisleOptions.value.find((x) => x.id === id);
}

function updateOrdersByIndex() {
  if (currentRecipes.value === undefined || currentRecipes.value === null) {
    return;
  }

  currentRecipes.value.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = i + 1;
  });
}

function onSortEnd() {
  nextTick(() => {
    updateOrdersByIndex();
    mealPlanStore.saveCurrentMealPlan([], true);
  });
}

function onRecipeCompleted(recipe: GetMealPlanResponseRecipe) {
  // eslint-disable-next-line no-param-reassign
  recipe.isComplete = !recipe.isComplete;
  const newList = currentRecipes.value.filter((x) => x.id !== recipe.id);
  if (recipe.isComplete) {
    newList.push(recipe);
  } else {
    newList.unshift(recipe);
  }
  currentRecipes.value = newList;
  updateOrdersByIndex();
  mealPlanStore.saveCurrentMealPlan([], true);

  if (!recipe.isComplete) {
    return;
  }

  const modalParameters: ModalParameters = {
    title: 'Recipe Completed',
    description: 'Would you like to go to the recipe to update inventory?',
    okAction: () => {
      router.push(RouterHelper.editRecipe(recipe));
    },
  };

  appStore.showModal(modalParameters);
}

const pageTitle = computed(() => {
  if (currentMealPlan.value?.id) {
    return '';
  }

  return 'New Meal Plan';
});

onMounted(async () => {
  await fetchGroceryItems();
  await fetchGroceryAisles();
  initialized.value = true;
});
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading :title="pageTitle" />
    <div v-if="initialized" class="mt-3">
      <div class="btn-toolbar sticky-top pt-1">
        <button class="btn btn-primary me-2" @click.stop.prevent="() => onSaveMealPlan()">
          Save
        </button>
        <button
          v-if="(currentMealPlan?.id || 0) > 0"
          class="btn btn-secondary me-2"
          @click.stop.prevent="() => onClearRecipes()"
        >
          Clear
        </button>
        <button
          v-if="(currentMealPlan?.id || 0) > 0"
          id="overflowMenuButton"
          class="btn btn-secondary dropdown-toggle"
          type="button"
          data-bs-toggle="dropdown"
          aria-expanded="false"
        >
          More
        </button>
        <ul
          v-if="(currentMealPlan?.id || 0) > 0"
          class="dropdown-menu"
          aria-labelledby="overflowMenuButton"
        >
          <li>
            <button
              class="dropdown-item text-danger"
              @click.stop.prevent="() => onDeleteMealPlan()"
            >
              Delete
            </button>
          </li>
        </ul>
      </div>
      <div class="grid mt-3">
        <div class="g-col-12 g-col-md-6">
          <label for="nameSearch" class="form-label">Name</label>
          <input
            id="mealPlanName"
            v-model="currentMealPlan.name"
            class="form-control"
            @keydown.stop.prevent.enter="() => onSaveMealPlan()"
          />
        </div>
      </div>
      <div class="form-check form-switch mt-3">
        <label class="form-check-label" for="showSortHandle">Enable Sort</label>
        <input
          id="showSortHandle"
          v-model="showSortHandle"
          :checked="showSortHandle"
          class="form-check-input"
          type="checkbox"
        />
      </div>
      <div v-if="(currentRecipes?.length || 0) < 1" class="grid mt-4">
        <div class="g-col-12 p-4 text-center">None selected</div>
      </div>
      <vue-draggable
        v-model="currentRecipes"
        :animation="200"
        group="item"
        ghost-class="ghost"
        handle=".sort-handle"
        class="grid mt-3 gap-sm"
        @end="onSortEnd"
      >
        <MealPlanRecipeCard
          v-for="(recipe, i) in currentRecipes"
          :key="recipe.id"
          :recipe="recipe"
          :lazy="i > 6"
          :show-sort-handle="showSortHandle"
          class="g-col-12 g-col-lg-6"
          @recipe-completed="onRecipeCompleted"
        />
      </vue-draggable>
      <h2 class="mt-4">Lists</h2>
      <div>Click an item to move it between lists.</div>
      <div class="grid mt-3 gap-sm">
        <div class="g-col-12 g-col-md-6">
          <MealPlanGroceryItemList
            title="Shopping List"
            :grocery-items="shoppingList"
            :on-item-click="addToPantry"
            :show-copy-list="true"
            :get-grocery-item-details="findGroceryItem"
            :get-grocery-aisle-details="findGroceryAisle"
          />
        </div>
        <div class="g-col-12 g-col-md-6">
          <MealPlanGroceryItemList
            title="Excluded Items"
            :grocery-items="pantry"
            :on-item-click="removeFromPantry"
            :on-clear="clearPantry"
            :get-grocery-item-details="findGroceryItem"
            :get-grocery-aisle-details="findGroceryAisle"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
