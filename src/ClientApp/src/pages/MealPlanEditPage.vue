<script lang="ts" setup>
import { computed, nextTick, onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';
import RecipeCard from '@/components/RecipeCard.vue';
import MealPlanShoppingItemList from '@/components/MealPlanShoppingItemList.vue';
import useMessageStore from '@/stores/messageStore';
import ApiHelper from '@/models/ApiHelper';
import { VueDraggable } from 'vue-draggable-plus';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();
const messageStore = useMessageStore();
const api = ApiHelper.client;

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

const shoppingItemOptions = ref([] as Array<ListShoppingItemsResponse>);

const useCompactView = ref(false);

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

async function fetchShoppingItems() {
  try {
    const response = await api().shoppingItemsList({ isPagingEnabled: false });
    shoppingItemOptions.value = (response.data.items || []) as Array<ListShoppingItemsResponse>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function findShoppingItem(id: number | undefined) {
  return shoppingItemOptions.value.find((x) => x.id === id);
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

const pageTitle = computed(() => {
  if (currentMealPlan.value?.id) {
    return '';
  }

  return 'New Meal Plan';
});

onMounted(async () => {
  await fetchShoppingItems();
});
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading :title="pageTitle" />
    <div class="btn-toolbar sticky-top pt-1 mt-3">
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
          <button class="dropdown-item text-danger" @click.stop.prevent="() => onDeleteMealPlan()">
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
      <label class="form-check-label" for="useCompactView">Compact view</label>
      <input
        id="useCompactView"
        v-model="useCompactView"
        :checked="useCompactView"
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
      <RecipeCard
        v-for="(recipe, i) in currentRecipes"
        :key="recipe.id"
        :recipe="recipe"
        :lazy="i > 6"
        :show-sort-handle="true"
        :show-compact-view="useCompactView"
        class="g-col-6 g-col-sm-4 g-col-md-3 g-col-lg-2"
      />
    </vue-draggable>
    <div class="grid mt-3 gap-sm">
      <div class="g-col-12 g-col-md-6">
        <MealPlanShoppingItemList
          title="Shopping List"
          :shopping-items="shoppingList"
          :on-item-click="addToPantry"
          :get-shopping-item-details="findShoppingItem"
          :show-copy-list="true"
        />
      </div>
      <div class="g-col-12 g-col-md-6">
        <MealPlanShoppingItemList
          title="Excluded Items"
          :shopping-items="pantry"
          :on-clear="clearPantry"
          :on-item-click="removeFromPantry"
          :get-shopping-item-details="findShoppingItem"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
