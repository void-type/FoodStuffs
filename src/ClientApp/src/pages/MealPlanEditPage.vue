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
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { currentMealPlan, currentRecipes } = storeToRefs(mealPlanStore);

const shoppingItemOptions = ref([] as Array<ListShoppingItemsResponse>);

const useCompactView = ref(false);

async function onDeleteMealPlan(id: number | null | undefined) {
  const parameters: ModalParameters = {
    title: 'Delete meal plan',
    description: 'Do you really want to delete this meal plan?',
    okAction: () => mealPlanStore.deleteMealPlan(id),
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
    <AppBreadcrumbs />
    <h1 class="mt-3">{{ (currentMealPlan?.id || 0) < 1 ? 'New' : 'Edit' }} Meal Plan</h1>
    <div class="grid mt-4">
      <div class="g-col-12">
        <div class="btn-toolbar sticky-top pt-1">
          <button
            class="btn btn-primary me-2"
            @click.stop.prevent="() => mealPlanStore.saveCurrentMealPlan()"
          >
            Save
          </button>
          <button
            v-if="(currentMealPlan?.id || 0) > 0"
            class="btn btn-secondary me-2"
            @click.stop.prevent="() => mealPlanStore.clearCurrentRecipes()"
          >
            Empty
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
                @click.stop.prevent="() => onDeleteMealPlan(currentMealPlan.id)"
              >
                Delete
              </button>
            </li>
          </ul>
        </div>
        <div class="grid mt-3">
          <div class="g-col-12 g-col-md-6">
            <label for="nameSearch" class="form-label">Meal plan name</label>
            <input
              id="mealPlanName"
              v-model="currentMealPlan.name"
              class="form-control"
              @keydown.stop.prevent.enter="() => mealPlanStore.saveCurrentMealPlan()"
            />
          </div>
          <div class="g-col-12">
            <div class="form-check form-switch mt-3">
              <label class="form-check-label" for="useCompactView" aria-label="Use compact view"
                ><font-awesome-icon class="me-2" icon="fa-moon" />Compact view</label
              >
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
            <div v-else-if="useCompactView" class="grid mt-4">
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
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
