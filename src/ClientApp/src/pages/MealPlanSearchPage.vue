<script lang="ts" setup>
import { onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';
import MealsIngredientList from '@/components/MealsIngredientList.vue';
import MealsCard from '@/components/MealsCard.vue';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();

const { currentMealPlan, currentRecipes } = storeToRefs(mealPlanStore);

async function onDeleteMealPlan() {
  const parameters: ModalParameters = {
    title: 'Delete meal plan',
    description: 'Do you really want to delete this meal plan?',
    okAction: mealPlanStore.deleteCurrentMealPlan,
  };

  appStore.showModal(parameters);
}

onMounted(async () => {
  mealPlanStore.fetchMealPlanList();
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4">Meals</h1>
    <div class="grid">
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-lg-6">
            <h2 class="mt-4 mb-3">{{ (currentMealPlan?.id || 0) < 1 ? 'New' : 'Edit' }} plan</h2>
            <label for="nameSearch" class="form-label">Meal plan name</label>
            <input
              id="mealPlanName"
              v-model="currentMealPlan.name"
              class="form-control"
              @keydown.stop.prevent.enter="mealPlanStore.saveCurrentMealPlan"
            />
            <div class="btn-toolbar mt-3">
              <button
                class="btn btn-primary me-2"
                @click.stop.prevent="mealPlanStore.saveCurrentMealPlan"
              >
                Save
              </button>
              <button
                class="btn btn-secondary me-2"
                @click.stop.prevent="mealPlanStore.clearCurrentRecipes"
              >
                Empty
              </button>
              <button
                class="btn btn-danger ms-auto"
                :disabled="(currentMealPlan?.id || 0) < 1"
                @click.stop.prevent="onDeleteMealPlan"
              >
                Delete
              </button>
            </div>
            <div class="grid mt-4">
              <div v-if="(currentRecipes?.length || 0) < 1" class="g-col-12 p-4 text-center">
                None selected
              </div>
              <MealsCard
                v-for="recipe in currentRecipes"
                :key="recipe.id"
                :recipe="recipe"
                :on-card-click="() => {}"
                card-type="active"
                class="g-col-12"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="g-col-12 mt-5">
        <div class="grid">
          <div class="g-col-12 g-col-md-6">
            <MealsIngredientList
              class="mt-4"
              title="Shopping list"
              :ingredients="mealPlanStore.currentShoppingList"
              :on-ingredient-click="mealPlanStore.addToCurrentPantry"
              :show-copy-list="true"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <MealsIngredientList
              class="mt-4"
              title="Pantry"
              :ingredients="mealPlanStore.currentPantry"
              :on-clear="mealPlanStore.clearCurrentPantry"
              :on-ingredient-click="mealPlanStore.removeFromCurrentPantry"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
