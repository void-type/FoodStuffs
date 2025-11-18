<script lang="ts" setup>
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';
import MealPlanRecipeCard from '@/components/MealPlanRecipeCard.vue';
import MealPlanGroceryItemList from '@/components/MealPlanGroceryItemList.vue';
import useMessageStore from '@/stores/messageStore';
import ApiHelper from '@/models/ApiHelper';
import { VueDraggable } from 'vue-draggable-plus';
import type {
  ListGroceryAislesResponse,
  GetMealPlanResponseRecipe,
  SearchGroceryItemsResultItem,
  GetMealPlanResponseExcludedGroceryItem,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import { useRouter } from 'vue-router';
import RouterHelper from '@/models/RouterHelper';

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: 0,
  },
});

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();
const messageStore = useMessageStore();
const router = useRouter();
const api = ApiHelper.client;

const initialized = ref(false);

// Determine if we're editing a specific meal plan or the current one
const { currentMealPlan, editingMealPlan } = storeToRefs(mealPlanStore);
const isEditingCurrent = computed(() => (props.id || 0) === currentMealPlan.value?.id);

// Use the appropriate meal plan based on whether we're editing current or a specific one
const activeMealPlan = computed(() =>
  isEditingCurrent.value ? currentMealPlan.value : editingMealPlan.value
);

const completedRecipes = computed({
  get() {
    return activeMealPlan.value?.recipes?.filter((recipe) => recipe.isComplete) || [];
  },
  set(value) {
    if (activeMealPlan.value === undefined || activeMealPlan.value === null) {
      return;
    }
    const incompleteList =
      activeMealPlan.value.recipes?.filter((recipe) => !recipe.isComplete) || [];
    activeMealPlan.value.recipes = [...value, ...incompleteList];
  },
});

const incompleteRecipes = computed({
  get() {
    return activeMealPlan.value?.recipes?.filter((recipe) => !recipe.isComplete) || [];
  },
  set(value) {
    if (activeMealPlan.value === undefined || activeMealPlan.value === null) {
      return;
    }
    const completedList = activeMealPlan.value.recipes?.filter((recipe) => recipe.isComplete) || [];
    activeMealPlan.value.recipes = [...completedList, ...value];
  },
});

const shoppingList = computed(() => {
  if (isEditingCurrent.value) {
    return mealPlanStore.currentShoppingList;
  }

  // Calculate shopping list for editing meal plan
  const groceryItemCounts = (activeMealPlan.value?.recipes || [])
    .flatMap((c) => c.groceryItems || [])
    .reduce((acc, item) => {
      if (!item.id) return acc;
      const existing = acc.find((x) => x.id === item.id);
      if (existing) {
        existing.quantity = (existing.quantity || 0) + (item.quantity || 0);
      } else {
        acc.push({ ...item });
      }
      return acc;
    }, [] as Array<GetMealPlanResponseExcludedGroceryItem>);

  (activeMealPlan.value?.excludedGroceryItems || []).forEach((x) => {
    if (!x.id) return;
    const existing = groceryItemCounts.find((item) => item.id === x.id);
    if (existing) {
      existing.quantity = Math.max(0, (existing.quantity || 0) - (x.quantity || 0));
    }
  });

  return groceryItemCounts.filter((item) => (item.quantity || 0) > 0);
});

const pantry = computed(() => {
  if (isEditingCurrent.value) {
    return mealPlanStore.currentPantry;
  }
  return activeMealPlan.value?.excludedGroceryItems || [];
});

const showSortHandle = ref(false);

async function onSaveMealPlan(quickSave: boolean = false) {
  if (isEditingCurrent.value) {
    await mealPlanStore.saveCurrentMealPlan([], quickSave);
  } else {
    const savedId = await mealPlanStore.saveMealPlan(activeMealPlan.value);
    if (savedId && !props.id) {
      // If this was a new meal plan, redirect to the edit page with the new ID
      router.push({ name: 'mealPlanEdit', params: { id: savedId } });
    }
  }
}

async function addToPantry(id: number) {
  if (isEditingCurrent.value) {
    await mealPlanStore.addToCurrentPantry(id);
  } else {
    // Add to editing meal plan pantry
    if (!activeMealPlan.value.excludedGroceryItems) {
      activeMealPlan.value.excludedGroceryItems = [];
    }
    const existing = activeMealPlan.value.excludedGroceryItems.find((x) => x.id === id);
    if (existing) {
      existing.quantity = (existing.quantity || 0) + 1;
    } else {
      activeMealPlan.value.excludedGroceryItems.push({ id, quantity: 1 });
    }
    await onSaveMealPlan(true);
  }
}

async function removeFromPantry(id: number) {
  if (isEditingCurrent.value) {
    await mealPlanStore.removeFromCurrentPantry(id);
  } else {
    // Remove from editing meal plan pantry
    if (!activeMealPlan.value.excludedGroceryItems) return;
    const existing = activeMealPlan.value.excludedGroceryItems.find((x) => x.id === id);
    if (existing) {
      existing.quantity = Math.max(0, (existing.quantity || 0) - 1);
      if (existing.quantity === 0) {
        activeMealPlan.value.excludedGroceryItems =
          activeMealPlan.value.excludedGroceryItems.filter((x) => x.id !== id);
      }
    }
    await onSaveMealPlan(true);
  }
}

async function clearPantry() {
  if (isEditingCurrent.value) {
    await mealPlanStore.clearCurrentPantry();
  } else {
    activeMealPlan.value.excludedGroceryItems = [];
    await onSaveMealPlan(true);
  }
}

async function onClearRecipes() {
  const parameters: ModalParameters = {
    title: 'Clear meal plan',
    description: 'Do you really want to remove all recipes from this meal plan?',
    okAction: () => {
      if (isEditingCurrent.value) {
        mealPlanStore.clearCurrentRecipes();
      } else {
        activeMealPlan.value.recipes = [];
        onSaveMealPlan();
      }
    },
  };

  appStore.showModal(parameters);
}

async function onDeleteMealPlan() {
  const parameters: ModalParameters = {
    title: 'Delete meal plan',
    description: 'Do you really want to delete this meal plan?',
    okAction: async () => {
      await mealPlanStore.deleteMealPlan(activeMealPlan.value.id);
      router.push({ name: 'mealPlanList' });
    },
  };

  appStore.showModal(parameters);
}

const groceryItemOptions = ref([] as Array<SearchGroceryItemsResultItem>);

async function fetchGroceryItems() {
  try {
    // TODO: use suggestion endpoint because this will be a huge response.
    const response = await api().groceryItemsSearch({ isPagingEnabled: false });
    groceryItemOptions.value = (response.data.results?.items ||
      []) as Array<SearchGroceryItemsResultItem>;
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
  if (activeMealPlan.value === undefined || activeMealPlan.value === null) {
    return;
  }

  const completed = completedRecipes.value;
  const incomplete = incompleteRecipes.value;

  completed.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = i + 1;
  });

  incomplete.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = completed.length + i + 1;
  });
}

function onSortEnd() {
  nextTick(() => {
    updateOrdersByIndex();
    if (isEditingCurrent.value) {
      mealPlanStore.saveCurrentMealPlan([], true);
    } else {
      onSaveMealPlan(true);
    }
  });
}

function onRecipeCompleted(recipe: GetMealPlanResponseRecipe) {
  // eslint-disable-next-line no-param-reassign
  recipe.isComplete = !recipe.isComplete;

  updateOrdersByIndex();
  if (isEditingCurrent.value) {
    mealPlanStore.saveCurrentMealPlan([], true);
  } else {
    onSaveMealPlan(true);
  }

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

async function onRecipeRemoved(recipe: GetMealPlanResponseRecipe) {
  if (!activeMealPlan.value?.recipes) {
    return;
  }

  activeMealPlan.value.recipes = activeMealPlan.value.recipes.filter((r) => r.id !== recipe.id);

  updateOrdersByIndex();
  await onSaveMealPlan(true);
}

const pageTitle = computed(() => {
  if (activeMealPlan.value?.id) {
    return '';
  }

  return 'New Meal Plan';
});

const sidesNeeded = computed(() => {
  if (!activeMealPlan.value) {
    return 0;
  }

  return (
    activeMealPlan.value.recipes?.reduce(
      (acc, recipe) => acc + (recipe.mealPlanningSidesCount || 0),
      0
    ) || 0
  );
});

const sidesHave = computed(() => {
  if (!activeMealPlan.value) {
    return 0;
  }

  // Count how many sides are included in the meal plan. A side is recipe with Category that looks like {name: 'Side'}
  return (
    activeMealPlan.value.recipes?.reduce(
      (acc, recipe) => acc + (recipe.categories?.some((cat) => cat.name === 'Side') ? 1 : 0),
      0
    ) || 0
  );
});

// Watch for ID changes to fetch the appropriate meal plan
watch(
  () => props.id,
  () => {
    if (!isEditingCurrent.value) {
      mealPlanStore.fetchMealPlan(props.id);
    }
  },
  { immediate: true }
);

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
          v-if="(activeMealPlan?.id || 0) > 0"
          class="btn btn-secondary me-2"
          @click.stop.prevent="() => onClearRecipes()"
        >
          Clear
        </button>
        <button
          v-if="(activeMealPlan?.id || 0) > 0"
          id="overflowMenuButton"
          class="btn btn-secondary dropdown-toggle"
          type="button"
          data-bs-toggle="dropdown"
          aria-expanded="false"
        >
          More
        </button>
        <ul
          v-if="(activeMealPlan?.id || 0) > 0"
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
      <div class="mt-3">
        {{ sidesNeeded }} side{{ sidesNeeded !== 1 ? 's' : '' }} needed.
        <span v-if="sidesHave < sidesNeeded" class="text-danger">
          ({{ sidesNeeded - sidesHave }} more needed)
        </span>
      </div>
      <div class="grid mt-3">
        <div class="g-col-12 g-col-md-6">
          <label for="nameSearch" class="form-label">Name</label>
          <input
            id="mealPlanName"
            v-model="activeMealPlan.name"
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
      <div class="mt-4">
        <div v-if="(incompleteRecipes?.length || 0) < 1" class="grid mt-3">
          <div class="g-col-12 p-4 text-center">None selected</div>
        </div>
        <div>
          <vue-draggable
            v-model="incompleteRecipes"
            :animation="200"
            group="incomplete"
            ghost-class="ghost"
            handle=".sort-handle"
            class="grid mt-3 gap-sm"
            @end="onSortEnd"
          >
            <MealPlanRecipeCard
              v-for="(recipe, i) in incompleteRecipes"
              :key="recipe.id"
              :recipe="recipe"
              :lazy="i > 6"
              :show-sort-handle="showSortHandle"
              :is-current-plan="isEditingCurrent"
              class="g-col-12 g-col-lg-6"
              @recipe-completed="onRecipeCompleted"
              @recipe-removed="onRecipeRemoved"
            />
          </vue-draggable>
        </div>
      </div>
      <div
        v-if="(completedRecipes?.length || 0) > 0"
        id="completedAccordion"
        class="mt-4 accordion"
      >
        <div class="accordion-item">
          <div class="accordion-header">
            <button
              class="accordion-button collapsed"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#completedCollapse"
              aria-expanded="false"
              aria-controls="completedCollapse"
            >
              Completed ({{ completedRecipes.length }})
            </button>
          </div>
          <div
            id="completedCollapse"
            class="accordion-collapse collapse"
            data-bs-parent="#completedAccordion"
          >
            <vue-draggable
              v-model="completedRecipes"
              :animation="200"
              group="completed"
              ghost-class="ghost"
              handle=".sort-handle"
              class="grid gap-sm"
              @end="onSortEnd"
            >
              <MealPlanRecipeCard
                v-for="(recipe, i) in completedRecipes"
                :key="recipe.id"
                :recipe="recipe"
                :lazy="i > 6"
                :show-sort-handle="showSortHandle"
                class="g-col-12 g-col-lg-6"
                @recipe-completed="onRecipeCompleted"
              />
            </vue-draggable>
          </div>
        </div>
      </div>
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
            :collapsable="true"
            :collapsed="true"
            :get-grocery-item-details="findGroceryItem"
            :get-grocery-aisle-details="findGroceryAisle"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
