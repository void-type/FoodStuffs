<script lang="ts" setup>
import { onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import RecipeCard from '@/components/RecipeCard.vue';
import MealPlanShoppingItemList from '@/components/MealPlanShoppingItemList.vue';

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();

const { currentMealPlan, currentRecipes } = storeToRefs(mealPlanStore);

async function onDeleteCurrentMealPlan() {
  const parameters: ModalParameters = {
    title: 'Delete meal plan',
    description: 'Do you really want to delete this meal plan?',
    okAction: () => mealPlanStore.deleteMealPlan(currentMealPlan.value.id),
  };

  appStore.showModal(parameters);
}

onMounted(async () => {
  mealPlanStore.fetchMealPlanList();
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4">{{ (currentMealPlan?.id || 0) < 1 ? 'New' : 'Edit' }} Plan</h1>
    <div class="grid mt-4">
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-md-6">
            <label for="nameSearch" class="form-label">Meal plan name</label>
            <input
              id="mealPlanName"
              v-model="currentMealPlan.name"
              class="form-control"
              @keydown.stop.prevent.enter="() => mealPlanStore.saveCurrentMealPlan()"
            />
          </div>
          <div class="g-col-12 btn-toolbar">
            <button
              class="btn btn-primary me-2"
              @click.stop.prevent="() => mealPlanStore.saveCurrentMealPlan()"
            >
              Save
            </button>
            <button
              class="btn btn-secondary me-2"
              @click.stop.prevent="() => mealPlanStore.newCurrentMealPlan()"
            >
              New
            </button>
            <button
              class="btn btn-secondary me-2"
              @click.stop.prevent="() => mealPlanStore.clearCurrentRecipes()"
            >
              Empty
            </button>
            <button
              class="btn btn-danger ms-auto"
              :disabled="(currentMealPlan?.id || 0) < 1"
              @click.stop.prevent="() => onDeleteCurrentMealPlan()"
            >
              Delete
            </button>
          </div>
        </div>
      </div>
    </div>
    <div class="grid mt-4">
      <div v-if="(currentRecipes?.length || 0) < 1" class="g-col-12 p-4 text-center">
        None selected
      </div>
      <RecipeCard
        v-for="(recipe, i) in currentRecipes"
        :key="recipe.id"
        :recipe="recipe"
        :lazy="i > 6"
        class="g-col-12 g-col-sm-6 g-col-md-4 g-col-lg-3"
      />
    </div>
    <div class="grid mt-5">
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-md-6">
            <MealPlanShoppingItemList
              title="Shopping list"
              :shopping-items="mealPlanStore.currentShoppingList"
              :on-item-click="mealPlanStore.addToCurrentPantry"
              :show-copy-list="true"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <MealPlanShoppingItemList
              title="Pantry"
              :shopping-items="mealPlanStore.currentPantry"
              :on-clear="mealPlanStore.clearCurrentPantry"
              :on-item-click="mealPlanStore.removeFromCurrentPantry"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
