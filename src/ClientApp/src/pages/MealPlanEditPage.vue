<script lang="ts" setup>
import type {
  GetMealPlanResponseExcludedGroceryItem,
  GetMealPlanResponseRecipe,
  ListGroceryAislesResponse,
  SearchGroceryItemsResultItem,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import { storeToRefs } from 'pinia';
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import { VueDraggable } from 'vue-draggable-plus';
import { useRouter } from 'vue-router';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import MealPlanGroceryItemList from '@/components/MealPlanGroceryItemList.vue';
import MealPlanRecipeCard from '@/components/MealPlanRecipeCard.vue';
import useSaveShortcut from '@/composables/useSaveShortcut';
import useUnsavedChangesGuard from '@/composables/useUnsavedChangesGuard';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useAppStore from '@/stores/appStore';
import useMealPlanStore, { MEAL_PLAN_MESSAGE_KEY } from '@/stores/mealPlanStore';
import useMessageStore from '@/stores/messageStore';

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
const isEditingCurrent = computed(() => (props.id || 0) > 0 && (props.id || 0) === currentMealPlan.value?.id);

// Use the appropriate meal plan based on whether we're editing current or a specific one
const activeMealPlan = computed(() =>
  isEditingCurrent.value ? currentMealPlan.value : editingMealPlan.value,
);

const completedRecipes = computed({
  get() {
    return activeMealPlan.value?.recipes?.filter(recipe => recipe.isComplete) || [];
  },
  set(value) {
    if (activeMealPlan.value === undefined || activeMealPlan.value === null) {
      return;
    }
    const incompleteList
      = activeMealPlan.value.recipes?.filter(recipe => !recipe.isComplete) || [];
    activeMealPlan.value.recipes = [
      ...value,
      ...incompleteList,
    ];
  },
});

const incompleteRecipes = computed({
  get() {
    return activeMealPlan.value?.recipes?.filter(recipe => !recipe.isComplete) || [];
  },
  set(value) {
    if (activeMealPlan.value === undefined || activeMealPlan.value === null) {
      return;
    }
    const completedList = activeMealPlan.value.recipes?.filter(recipe => recipe.isComplete) || [];
    activeMealPlan.value.recipes = [
      ...completedList,
      ...value,
    ];
  },
});

const shoppingList = computed(() => {
  if (isEditingCurrent.value) {
    return mealPlanStore.currentShoppingList;
  }

  // Calculate shopping list for editing meal plan
  const groceryItemCounts = (activeMealPlan.value?.recipes || [])
    .flatMap(c => c.groceryItems || [])
    .reduce((acc, item) => {
      if (!item.id) {
        return acc;
      }
      const existing = acc.find(x => x.id === item.id);
      if (existing) {
        existing.quantity = (existing.quantity || 0) + (item.quantity || 0);
      } else {
        acc.push({ ...item });
      }
      return acc;
    }, [] as Array<GetMealPlanResponseExcludedGroceryItem>);

  (activeMealPlan.value?.excludedGroceryItems || []).forEach((x) => {
    if (!x.id) {
      return;
    }
    const existing = groceryItemCounts.find(item => item.id === x.id);
    if (existing) {
      existing.quantity = Math.max(0, (existing.quantity || 0) - (x.quantity || 0));
    }
  });

  return groceryItemCounts.filter(item => (item.quantity || 0) > 0);
});

const pantry = computed(() => {
  if (isEditingCurrent.value) {
    return mealPlanStore.currentPantry;
  }
  return activeMealPlan.value?.excludedGroceryItems || [];
});

const showSortHandle = ref(false);
const isHaEnabled = ref(false);
const thawMeatReminderOn = ref(false);

// The name is the only field that isn't saved as soon as it changes; everything else quick-saves.
// Edit a copy so unsaved typing is distinguishable from what's saved (and so quick saves don't persist it by accident).
const workingName = ref('');

watch(
  () => activeMealPlan.value?.name,
  (name) => {
    workingName.value = name || '';
  },
  { immediate: true },
);

const isDirty = computed(() => initialized.value && workingName.value !== (activeMealPlan.value?.name || ''));

const { navigateWithoutPrompt } = useUnsavedChangesGuard(isDirty);

async function onSaveMealPlan(quickSave: boolean = false, silent: boolean = false): Promise<boolean> {
  if (!quickSave) {
    activeMealPlan.value.name = workingName.value;
  }

  if (isEditingCurrent.value) {
    return mealPlanStore.saveCurrentMealPlan([], quickSave, false, silent);
  }

  const savedId = await mealPlanStore.saveMealPlan(activeMealPlan.value, [], silent);
  if (savedId && !props.id) {
    // If this was a new meal plan, redirect to the edit page with the new ID
    await navigateWithoutPrompt({
      name: 'mealPlanEdit',
      params: { id: savedId },
    });
  }

  return savedId !== null;
}

// Nothing to save until the meal plan has loaded.
useSaveShortcut(() => (initialized.value
  ? onSaveMealPlan()
  : undefined));

async function onSaveAndMakeCurrent() {
  activeMealPlan.value.name = workingName.value;

  const savedId = await mealPlanStore.saveMealPlan(activeMealPlan.value);
  if (savedId) {
    await mealPlanStore.setCurrentMealPlan(savedId);
    await navigateWithoutPrompt({
      name: 'mealPlanEdit',
      params: { id: savedId },
    });
  }
}

async function onMakeCurrent() {
  if (!activeMealPlan.value?.id) {
    return;
  }

  await mealPlanStore.setCurrentMealPlan(activeMealPlan.value.id);
  router.push({
    name: 'mealPlanEdit',
    params: { id: activeMealPlan.value.id },
  });
}

function announce(text: string, action?: { label: string; handler: () => void | Promise<void> }) {
  messageStore.setSuccessMessage(text, {
    key: MEAL_PLAN_MESSAGE_KEY,
    action,
    autoClear: action ? 10 : undefined,
  });
}

async function moveNonCurrentPantryItem(id: number, toPantry: boolean): Promise<boolean> {
  const mealPlan = activeMealPlan.value;

  if (toPantry) {
    mealPlan.excludedGroceryItems ??= [];

    const existing = mealPlan.excludedGroceryItems.find(x => x.id === id);
    if (existing) {
      existing.quantity = (existing.quantity || 0) + 1;
    } else {
      mealPlan.excludedGroceryItems.push({ id, quantity: 1 });
    }
  } else {
    const existing = mealPlan.excludedGroceryItems?.find(x => x.id === id);
    if (!existing) {
      return false;
    }

    existing.quantity = Math.max(0, (existing.quantity || 0) - 1);
    if (existing.quantity === 0) {
      mealPlan.excludedGroceryItems = mealPlan.excludedGroceryItems!.filter(x => x.id !== id);
    }
  }

  return onSaveMealPlan(true, true);
}

async function movePantryItem(id: number, toPantry: boolean, isUndo = false) {
  const mealPlanId = activeMealPlan.value?.id;

  let saved: boolean;
  if (isEditingCurrent.value) {
    saved = toPantry
      ? await mealPlanStore.addToCurrentPantry(id, 1, true)
      : await mealPlanStore.removeFromCurrentPantry(id, 1, true);
  } else {
    saved = await moveNonCurrentPantryItem(id, toPantry);
  }

  if (!saved) {
    return;
  }

  const name = findGroceryItem(id)?.name || 'Item';
  const destination = toPantry ? 'pantry' : 'shopping list';

  if (isUndo) {
    announce(`Moved ${name} back to the ${destination}.`);
    return;
  }

  announce(`Moved ${name} to the ${destination}.`, {
    label: 'Undo',
    handler: () => undoPantryMove(mealPlanId, id, !toPantry),
  });
}

async function undoPantryMove(mealPlanId: number | undefined, id: number, toPantry: boolean) {
  // The toast outlives page navigation, so make sure we're still on the plan the item came from.
  if (activeMealPlan.value?.id !== mealPlanId) {
    messageStore.setErrorMessage('Can\'t undo because the meal plan is no longer open.');
    return;
  }

  await movePantryItem(id, toPantry, true);
}

const addToPantry = (id: number) => movePantryItem(id, true);
const removeFromPantry = (id: number) => movePantryItem(id, false);

async function clearPantry() {
  let saved: boolean;
  if (isEditingCurrent.value) {
    saved = await mealPlanStore.clearCurrentPantry(true);
  } else {
    activeMealPlan.value.excludedGroceryItems = [];
    saved = await onSaveMealPlan(true, true);
  }

  if (saved) {
    announce('Cleared the pantry.');
  }
}

async function onClearRecipes() {
  const parameters: ModalParameters = {
    title: 'Clear meal plan',
    description: 'Do you really want to remove all recipes from this meal plan?',
    okAction: async () => {
      let saved: boolean;
      if (isEditingCurrent.value) {
        saved = await mealPlanStore.clearCurrentRecipes(true);
      } else {
        activeMealPlan.value.recipes = [];
        saved = await onSaveMealPlan(false, true);
      }

      if (saved) {
        announce('Removed all recipes from the meal plan.');
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
      await navigateWithoutPrompt({ name: 'mealPlanList' });
    },
  };

  appStore.showModal(parameters);
}

const groceryItemOptions = ref([] as Array<SearchGroceryItemsResultItem>);

async function fetchGroceryItems() {
  try {
    // TODO: use suggestion endpoint because this will be a huge response.
    const response = await api().groceryItemsSearch({ isPagingEnabled: false });
    groceryItemOptions.value = (response.data.results?.items
      || []) as Array<SearchGroceryItemsResultItem>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function findGroceryItem(id: number | undefined) {
  return groceryItemOptions.value.find(x => x.id === id);
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
  return groceryAisleOptions.value.find(x => x.id === id);
}

function findRecipesForGroceryItem(id: number | undefined) {
  if (!id) {
    return [];
  }
  return (activeMealPlan.value?.recipes || [])
    .filter(recipe => recipe.groceryItems?.some(gi => gi.id === id))
    .map(recipe => ({
      name: recipe.name || '',
      quantity: recipe.groceryItems?.find(gi => gi.id === id)?.quantity ?? 0,
    }));
}

function updateOrdersByIndex() {
  if (activeMealPlan.value === undefined || activeMealPlan.value === null) {
    return;
  }

  const completed = completedRecipes.value;
  const incomplete = incompleteRecipes.value;

  completed.forEach((x, i) => {
    x.order = i + 1;
  });

  incomplete.forEach((x, i) => {
    x.order = completed.length + i + 1;
  });
}

function onSortEnd() {
  nextTick(async () => {
    updateOrdersByIndex();
    if (await onSaveMealPlan(true, true)) {
      announce('Recipe order saved.');
    }
  });
}

async function onRecipeCompleted(recipe: GetMealPlanResponseRecipe) {
  recipe.isComplete = !recipe.isComplete;

  updateOrdersByIndex();
  if (await onSaveMealPlan(true, true)) {
    announce(`Marked ${recipe.name} ${recipe.isComplete ? 'complete' : 'not complete'}.`);
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
  const mealPlan = activeMealPlan.value;
  if (!mealPlan?.recipes) {
    return;
  }

  const index = mealPlan.recipes.findIndex(r => r.id === recipe.id);
  if (index < 0) {
    return;
  }

  const mealPlanId = mealPlan.id;
  mealPlan.recipes = mealPlan.recipes.filter(r => r.id !== recipe.id);

  updateOrdersByIndex();

  // The save message is silenced so the only toast is the one with Undo.
  if (!(await onSaveMealPlan(true, true))) {
    return;
  }

  announce(`Removed ${recipe.name} from the meal plan.`, {
    label: 'Undo',
    handler: () => onUndoRecipeRemoved(mealPlanId, recipe, index),
  });
}

async function onUndoRecipeRemoved(mealPlanId: number | undefined, recipe: GetMealPlanResponseRecipe, index: number) {
  const recipes = activeMealPlan.value?.recipes;

  // The toast outlives page navigation, so make sure we're still on the plan the recipe came from.
  if (!recipes || activeMealPlan.value.id !== mealPlanId) {
    messageStore.setErrorMessage('Can\'t undo because the meal plan is no longer open.');
    return;
  }

  if (recipes.some(r => r.id === recipe.id)) {
    return;
  }

  recipes.splice(Math.min(index, recipes.length), 0, recipe);

  updateOrdersByIndex();
  if (await onSaveMealPlan(true, true)) {
    announce(`Restored ${recipe.name} to the meal plan.`);
  }
}

const pageTitle = computed(() => {
  if (isEditingCurrent.value) {
    return 'Current Meal Plan';
  }

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
      0,
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
      (acc, recipe) => acc + (recipe.categories?.some(cat => cat.name === 'Side')
        ? 1
        : 0),
      0,
    ) || 0
  );
});

// Watch for ID changes to fetch the appropriate meal plan
watch(
  () => props.id,
  () => {
    if (isEditingCurrent.value) {
      mealPlanStore.refreshCurrentMealPlan();
    } else {
      mealPlanStore.fetchMealPlan(props.id || null);
    }
  },
  { immediate: true },
);

async function onThawMeatReminderToggle() {
  try {
    await api().homeAssistantSetReminderThawMeat({ state: thawMeatReminderOn.value });
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    // Revert toggle on failure
    thawMeatReminderOn.value = !thawMeatReminderOn.value;
  }
}

onMounted(async () => {
  await fetchGroceryItems();
  await fetchGroceryAisles();

  try {
    const haEnabledResponse = await api().homeAssistantIsEnabled();
    isHaEnabled.value = haEnabledResponse.data === true;

    if (isHaEnabled.value) {
      const stateResponse = await api().homeAssistantGetReminderThawMeatState();
      thawMeatReminderOn.value = stateResponse.data === true;
    }
  } catch {
    // HA errors are non-critical; don't surface to user
  }

  initialized.value = true;
});
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading :title="pageTitle" />
    <div v-if="initialized" class="mt-3">
      <div class="btn-toolbar sticky-top pt-1">
        <button
          class="btn btn-primary me-2"
          title="Save (Ctrl+S)"
          aria-keyshortcuts="Control+S Meta+S"
          @click.stop.prevent="() => onSaveMealPlan()"
        >
          Save
        </button>
        <button
          v-if="!isEditingCurrent && !(activeMealPlan?.id)"
          class="btn btn-primary me-2"
          @click.stop.prevent="() => onSaveAndMakeCurrent()"
        >
          Save and Make Current
        </button>
        <button
          v-if="!isEditingCurrent && (activeMealPlan?.id || 0) > 0"
          class="btn btn-secondary me-2"
          @click.stop.prevent="() => onMakeCurrent()"
        >
          Make Current
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
      <div v-if="isHaEnabled" class="form-check form-switch mt-3">
        <label class="form-check-label" for="thawMeatReminder">Thaw Meat Reminder</label>
        <input
          id="thawMeatReminder"
          v-model="thawMeatReminderOn"
          class="form-check-input"
          type="checkbox"
          @change="onThawMeatReminderToggle"
        >
      </div>
      <div class="grid mt-3">
        <div class="g-col-12 g-col-md-6">
          <label for="nameSearch" class="form-label">Name</label>
          <input
            id="mealPlanName"
            v-model="workingName"
            class="form-control"
            @keydown.stop.prevent.enter="() => onSaveMealPlan()"
          >
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
        >
      </div>
      <div class="mt-4">
        <div v-if="(incompleteRecipes?.length || 0) < 1" class="grid mt-3">
          <div class="g-col-12 p-4 text-center">
            None selected
          </div>
        </div>
        <div>
          <VueDraggable
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
              class="g-col-12 g-col-md-6 g-col-lg-4"
              @recipe-completed="onRecipeCompleted"
              @recipe-removed="onRecipeRemoved"
            />
          </VueDraggable>
        </div>
      </div>
      <div
        v-if="(completedRecipes?.length || 0) > 0"
        id="completedAccordion"
        class="mt-4 accordion accordion-flush"
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
            <VueDraggable
              v-model="completedRecipes"
              :animation="200"
              group="completed"
              ghost-class="ghost"
              handle=".sort-handle"
              class="grid gap-sm pt-3"
              @end="onSortEnd"
            >
              <MealPlanRecipeCard
                v-for="(recipe, i) in completedRecipes"
                :key="recipe.id"
                :recipe="recipe"
                :lazy="i > 6"
                :show-sort-handle="showSortHandle"
                :is-current-plan="isEditingCurrent"
                class="g-col-12 g-col-md-6 g-col-lg-4"
                @recipe-completed="onRecipeCompleted"
                @recipe-removed="onRecipeRemoved"
              />
            </VueDraggable>
          </div>
        </div>
      </div>
      <h2 class="mt-4">
        Lists
      </h2>
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
            :get-recipes-for-grocery-item="findRecipesForGroceryItem"
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
            :get-recipes-for-grocery-item="findRecipesForGroceryItem"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.accordion-button {
  border: var(--bs-border-width) solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
}
</style>
