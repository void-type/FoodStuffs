<script lang="ts" setup>
import { onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import useMealSetStore from '@/stores/mealSetStore';
import MealsIngredientList from '@/components/MealsIngredientList.vue';
import MealsCard from '@/components/MealsCard.vue';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const mealSetStore = useMealSetStore();

const { currentMealSet, selectedRecipes } = storeToRefs(mealSetStore);

async function onDeleteMealSet() {
  const parameters: ModalParameters = {
    title: 'Delete meal set',
    description: 'Do you really want to delete this meal set?',
    okAction: mealSetStore.deleteCurrentMealSet,
  };

  appStore.showModal(parameters);
}

onMounted(async () => {});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4">Meals</h1>
    <div class="grid">
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-lg-6">
            <h2 class="mt-4 mb-3">{{ (currentMealSet?.id || 0) < 1 ? 'New' : 'Edit' }} plan</h2>
            <label for="nameSearch" class="form-label">Meal set name</label>
            <input
              id="mealSetName"
              v-model="currentMealSet.name"
              class="form-control"
              @keydown.stop.prevent.enter="mealSetStore.saveCurrentMealSet"
            />
            <div class="btn-toolbar mt-3">
              <button
                class="btn btn-primary me-2"
                @click.stop.prevent="mealSetStore.saveCurrentMealSet"
              >
                Save
              </button>
              <button
                class="btn btn-secondary me-2"
                @click.stop.prevent="mealSetStore.clearRecipes"
              >
                Empty
              </button>
              <button
                class="btn btn-danger ms-auto"
                :disabled="(currentMealSet?.id || 0) < 1"
                @click.stop.prevent="onDeleteMealSet"
              >
                Delete
              </button>
            </div>
            <div class="grid mt-4">
              <div v-if="(selectedRecipes?.length || 0) < 1" class="g-col-12 p-4 text-center">
                None selected
              </div>
              <MealsCard
                v-for="recipe in selectedRecipes"
                :key="recipe.id"
                :recipe="recipe"
                :on-card-click="mealSetStore.toggleRecipe"
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
              :ingredients="mealSetStore.getShoppingList"
              :on-ingredient-click="mealSetStore.addToPantry"
              :show-copy-list="true"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <MealsIngredientList
              class="mt-4"
              title="Pantry"
              :ingredients="mealSetStore.getPantry"
              :on-clear="mealSetStore.clearPantry"
              :on-ingredient-click="mealSetStore.removeFromPantry"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
