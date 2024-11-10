<script lang="ts" setup>
import { onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';
import RecipeCard from '@/components/RecipeCard.vue';
import MealPlanShoppingItemList from '@/components/MealPlanShoppingItemList.vue';
import useMessageStore from '@/stores/messageStore';
import ApiHelpers from '@/models/ApiHelpers';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import RecipeListItem from '@/components/RecipeListItem.vue';

const mealPlanStore = useMealPlanStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { currentMealPlan, currentRecipes } = storeToRefs(mealPlanStore);

const shoppingItemOptions = ref([] as Array<ListShoppingItemsResponse>);

const useListView = ref(false);

async function fetchShoppingItems() {
  try {
    const response = await api().shoppingItemsList({ isPagingEnabled: false });
    shoppingItemOptions.value = (response.data.items || []) as Array<ListShoppingItemsResponse>;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function getShoppingItem(id: number | undefined) {
  return shoppingItemOptions.value.find((x) => x.id === id);
}

onMounted(async () => {
  mealPlanStore.fetchMealPlanList();
  await fetchShoppingItems();
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
          </div>
        </div>
      </div>
    </div>
    <div class="form-check form-switch mt-3" title="Use list view">
      <label
        class="form-check-label"
        for="useListView"
        title="Use list view"
        aria-label="Use list view"
        ><font-awesome-icon class="me-2" icon="fa-moon" />List view</label
      >
      <input
        id="useListView"
        v-model="useListView"
        :checked="useListView"
        class="form-check-input"
        type="checkbox"
      />
    </div>
    <div v-if="(currentRecipes?.length || 0) < 1" class="grid mt-4">
      <div class="g-col-12 p-4 text-center">None selected</div>
    </div>
    <div v-else-if="useListView" class="grid mt-4">
      <RecipeListItem
        v-for="(recipe, i) in currentRecipes"
        :key="recipe.id"
        :recipe="recipe"
        :lazy="i > 6"
        class="g-col-12 g-col-sm-6 g-col-md-4 g-col-lg-3"
      />
    </div>
    <div v-else class="grid mt-4">
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
              :get-shopping-item-details="getShoppingItem"
              :show-copy-list="true"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <MealPlanShoppingItemList
              title="Pantry"
              :shopping-items="mealPlanStore.currentPantry"
              :on-clear="mealPlanStore.clearCurrentPantry"
              :on-item-click="mealPlanStore.removeFromCurrentPantry"
              :get-shopping-item-details="getShoppingItem"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
