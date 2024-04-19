<script lang="ts" setup>
import { computed, onMounted, ref, watch } from 'vue';
import { storeToRefs } from 'pinia';
import useMealStore from '@/stores/mealStore';
import { clamp, isNil, toInt } from '@/models/FormatHelpers';
import ApiHelpers from '@/models/ApiHelpers';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import MealsIngredientList from '@/components/MealsIngredientList.vue';
import MealsCard from '@/components/MealsCard.vue';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';
import useMessageStore from '@/stores/messageStore';
import type { SaveMealSetRequest } from '@/api/data-contracts';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import RecipeSearchCategoriesFilter from '@/components/RecipeSearchCategoriesFilter.vue';
import Choices from '@/models/Choices';

const appStore = useAppStore();
const mealStore = useMealStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { mealSetListRequest, currentMealSet } = storeToRefs(mealStore);

async function onDeleteMealSet() {
  async function deleteMealSet() {
    const { id } = currentMealSet.value;

    if (!id) {
      return;
    }

    try {
      const response = await api().mealSetsDelete(id);

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      await mealStore.fetchMealSetList();

      if (mealStore.currentMealSet.id === id) {
        mealStore.unsetCurrentMealSet();
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Delete meal set',
    description: 'Do you really want to delete this meal set?',
    okAction: deleteMealSet,
  };

  appStore.showModal(parameters);
}

onMounted(async () => {
  await fetchMealSetList();
  await changeMealSetIndex(0);
});

watch(selectedCategories, () => {
  recipeListRequest.value.categories = selectedCategories.value;
  fetchRecipeList();
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4">Meals</h1>
    <div class="grid">
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-lg-6">
            <h2 class="mt-4 mb-3">{{ (currentMealSet.id || 0) < 1 ? 'New' : 'Edit' }} plan</h2>
            <label for="nameSearch" class="form-label">Meal set name</label>
            <input
              id="mealSetName"
              v-model="currentMealSet.name"
              class="form-control"
              @keydown.stop.prevent.enter="saveMealSet"
            />
            <div class="btn-toolbar mt-3">
              <button class="btn btn-secondary me-2" @click.stop.prevent="changeMealSetIndexBack">
                Back
              </button>
              <button
                class="btn btn-secondary me-2"
                @click.stop.prevent="changeMealSetIndexForward"
              >
                Forward
              </button>
              <button class="btn btn-secondary ms-auto" @click.stop.prevent="mealStore.newMealSet">
                New
              </button>
            </div>
            <div class="btn-toolbar mt-3">
              <button class="btn btn-primary me-2" @click.stop.prevent="saveMealSet">Save</button>
              <button class="btn btn-secondary me-2" @click.stop.prevent="mealStore.clearRecipes">
                Empty
              </button>
              <button
                class="btn btn-danger ms-auto"
                :disabled="(currentMealSet.id || 0) < 1"
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
                :on-card-click="mealStore.toggleRecipe"
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
              :ingredients="mealStore.getShoppingList"
              :on-ingredient-click="mealStore.addToPantry"
              :show-copy-list="true"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <MealsIngredientList
              class="mt-4"
              title="Pantry"
              :ingredients="mealStore.getPantry"
              :on-clear="mealStore.clearPantry"
              :on-ingredient-click="mealStore.removeFromPantry"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
