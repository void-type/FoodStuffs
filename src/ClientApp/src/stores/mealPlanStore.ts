import type {
  GetMealPlanResponse,
  GetMealPlanResponseExcludedGroceryItem,
  IItemSetOfListMealPlansResponse,
  MealPlansListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import { defineStore } from 'pinia';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { isNil } from '@/models/FormatHelper';
import ListMealPlansRequest from '@/models/MealPlansListRequest';
import {
  addGroceryItem,
  countGroceryItems,
  listRequestToQueryParams,
  storeCurrentMealPlanInStorage,
  subtractGroceryItem,
} from '@/models/MealPlanStoreHelper';
import MealPlanWorking from '@/models/MealPlanWorking';
import useMessageStore from './messageStore';

interface MealPlanStoreState {
  listResponse: IItemSetOfListMealPlansResponse;
  listRequest: MealPlansListParams;
  currentMealPlan: GetMealPlanResponse;
  editingMealPlan: GetMealPlanResponse;
}

const api = ApiHelper.client;

export default defineStore('mealPlan', {
  state: (): MealPlanStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: { ...new ListMealPlansRequest() },
    currentMealPlan: MealPlanWorking.createForStore(),
    editingMealPlan: MealPlanWorking.createForStore(),
  }),

  getters: {
    currentRecipes: state => state.currentMealPlan.recipes || [],

    currentPantry: state => state.currentMealPlan.excludedGroceryItems || [],

    currentRecipesContains: state => (recipeId: number | null | undefined) => {
      if (isNil(recipeId)) {
        return false;
      }

      return (state.currentMealPlan.recipes || []).map(x => x.id).includes(recipeId!);
    },

    currentShoppingList(state): GetMealPlanResponseExcludedGroceryItem[] {
      const groceryItemCounts = this.currentRecipes
        .flatMap(c => c.groceryItems || [])
        .reduce(countGroceryItems, []);

      (state.currentMealPlan.excludedGroceryItems || []).forEach((x) => {
        if (!x.id) {
          return;
        }

        subtractGroceryItem(groceryItemCounts, x.id, x.quantity);
      });

      return groceryItemCounts;
    },

    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async addToCurrentPantry(groceryItemId: number | undefined, count = 1) {
      if (this.currentMealPlan === null || isNil(groceryItemId)) {
        return;
      }

      addGroceryItem(this.currentMealPlan.excludedGroceryItems || [], groceryItemId, count);
      await this.saveCurrentMealPlan([], true);
    },

    async removeFromCurrentPantry(groceryItemId: number | undefined, count = 1) {
      if (this.currentMealPlan === null || isNil(groceryItemId)) {
        return;
      }

      subtractGroceryItem(this.currentMealPlan.excludedGroceryItems || [], groceryItemId, count);
      await this.saveCurrentMealPlan([], true);
    },

    async clearCurrentPantry() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.excludedGroceryItems = [];
      await this.saveCurrentMealPlan([], true);
    },

    async addCurrentRecipe(recipeId: number | null | undefined) {
      if (this.currentMealPlan === null) {
        return;
      }

      if (isNil(recipeId)) {
        return;
      }

      await this.saveCurrentMealPlan([recipeId]);
    },

    async removeCurrentRecipe(recipeId: number | null | undefined) {
      if (this.currentMealPlan === null) {
        return;
      }

      if (isNil(recipeId)) {
        return;
      }

      const { recipes } = this.currentMealPlan;

      if (recipes !== null && typeof recipes !== 'undefined') {
        this.currentMealPlan.recipes = recipes.filter(x => x.id !== recipeId);
      }

      await this.saveCurrentMealPlan();
    },

    async clearCurrentRecipes() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.recipes = [];
      await this.saveCurrentMealPlan();
    },

    async newCurrentMealPlan() {
      this.currentMealPlan = MealPlanWorking.createForStore();
      storeCurrentMealPlanInStorage(null);
    },

    async setCurrentMealPlan(id: number | null | undefined) {
      if (isNil(id)) {
        return;
      }

      try {
        const response = await api().mealPlansGet({ id });
        this.currentMealPlan = response.data;
        if (response.data.id) {
          storeCurrentMealPlanInStorage(response.data.id);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        this.newCurrentMealPlan();
      }
    },

    async saveCurrentMealPlan(additionalRecipeIds: number[] = [], quickSave: boolean = false) {
      const current = this.currentMealPlan;

      if (current === null) {
        return;
      }

      const request = current;

      if (additionalRecipeIds && additionalRecipeIds.length > 0) {
        additionalRecipeIds.forEach((additionalId) => {
          if (request.recipes === null || typeof request.recipes === 'undefined') {
            return;
          }

          const highestOrder = Math.max(...request.recipes.map(x => x.order || 0), 0);

          request.recipes?.push({
            id: additionalId,
            order: highestOrder + 1,
          });
        });
      }

      try {
        const response = await api().mealPlansSave(request);

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        // Quick save can be used for rapid changes that don't need refreshed data returned like updating pantry groceryItems.
        if (!quickSave) {
          await this.setCurrentMealPlan(response.data.id);
          await this.fetchMealPlanList();
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async deleteMealPlan(id: number | null | undefined) {
      if (!id) {
        return;
      }

      try {
        const response = await api().mealPlansDelete({ id });

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        await this.fetchMealPlanList();

        if (this.currentMealPlan.id === id) {
          this.newCurrentMealPlan();
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async fetchMealPlanList() {
      try {
        const response = await api().mealPlansList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async fetchMealPlan(id: number | null | undefined) {
      if (isNil(id)) {
        this.editingMealPlan = MealPlanWorking.createForStore();
        return;
      }

      try {
        const response = await api().mealPlansGet({ id });
        this.editingMealPlan = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        this.editingMealPlan = MealPlanWorking.createForStore();
      }
    },

    async saveMealPlan(
      mealPlan: GetMealPlanResponse,
      additionalRecipeIds: number[] = [],
    ): Promise<number | null> {
      if (mealPlan === null) {
        return null;
      }

      const request = mealPlan;

      if (additionalRecipeIds && additionalRecipeIds.length > 0) {
        additionalRecipeIds.forEach((additionalId) => {
          if (request.recipes === null || typeof request.recipes === 'undefined') {
            return;
          }

          const highestOrder = Math.max(...request.recipes.map(x => x.order || 0), 0);

          request.recipes?.push({
            id: additionalId,
            order: highestOrder + 1,
          });
        });
      }

      try {
        const response = await api().mealPlansSave(request);

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        // Refresh the editing meal plan with the saved data
        await this.fetchMealPlan(response.data.id);
        await this.fetchMealPlanList();

        return response.data.id || null;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        return null;
      }
    },
  },
});
