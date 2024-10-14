import type {
  GetMealPlanResponse,
  GetMealPlanResponsePantryShoppingItem,
  IItemSetOfListMealPlansResponse,
  MealPlansListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import { isNil } from '@/models/FormatHelpers';
import WorkingMealPlan from '@/models/GetMealPlanResponseClass';
import SearchMealPlansRequest from '@/models/SearchMealPlansRequest';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import {
  addShoppingItem,
  countShoppingItems,
  listRequestToQueryParams,
  storeCurrentMealPlanInStorage,
  subtractShoppingItem,
} from '@/models/MealPlanStoreHelpers';
import useMessageStore from './messageStore';

interface MealPlanStoreState {
  listResponse: IItemSetOfListMealPlansResponse;
  listRequest: MealPlansListParams;
  currentMealPlan: GetMealPlanResponse;
}

const api = ApiHelpers.client;

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
    listRequest: { ...new SearchMealPlansRequest() },
    currentMealPlan: WorkingMealPlan.createForStore(),
  }),

  getters: {
    currentRecipes: (state) => state.currentMealPlan.recipes || [],

    currentPantry: (state) => state.currentMealPlan.pantryShoppingItems || [],

    currentRecipesContains: (state) => (recipeId: number | null | undefined) => {
      if (isNil(recipeId)) {
        return false;
      }

      return (state.currentMealPlan.recipes || []).map((x) => x.id).includes(recipeId!);
    },

    currentShoppingList(state): GetMealPlanResponsePantryShoppingItem[] {
      const shoppingItemCounts = this.currentRecipes
        .flatMap((c) => c.shoppingItems || [])
        .reduce(countShoppingItems, []);

      (state.currentMealPlan.pantryShoppingItems || []).forEach((x) => {
        if (!x.id) {
          return;
        }

        subtractShoppingItem(shoppingItemCounts, x.id, x.quantity);
      });

      return shoppingItemCounts;
    },

    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async clearCurrentPantry() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.pantryShoppingItems = [];
      await this.saveCurrentMealPlan();
    },

    async addToCurrentPantry(shoppingItemId: number | undefined, count = 1) {
      if (this.currentMealPlan === null || isNil(shoppingItemId)) {
        return;
      }

      addShoppingItem(this.currentMealPlan.pantryShoppingItems || [], shoppingItemId, count);
      await this.saveCurrentMealPlan([], true);
    },

    async removeFromCurrentPantry(shoppingItemId: number | undefined, count = 1) {
      if (this.currentMealPlan === null || isNil(shoppingItemId)) {
        return;
      }

      subtractShoppingItem(this.currentMealPlan.pantryShoppingItems || [], shoppingItemId, count);
      await this.saveCurrentMealPlan([], true);
    },

    async clearCurrentRecipes() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.recipes = [];
      await this.saveCurrentMealPlan();
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
        this.currentMealPlan.recipes = recipes.filter((x) => x.id !== recipeId);
      }

      await this.saveCurrentMealPlan();
    },

    async newCurrentMealPlan() {
      this.currentMealPlan = WorkingMealPlan.createForStore();
      storeCurrentMealPlanInStorage(null);
    },

    async setCurrentMealPlan(id: number | null | undefined) {
      if (isNil(id)) {
        return;
      }

      try {
        const response = await api().mealPlansGet(id);
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

          const lastIndex = (request.recipes.length || 0) - 1;
          const lastOrder = request.recipes[lastIndex]?.order || 0;

          request.recipes?.push({
            id: additionalId,
            order: lastOrder + 1,
          });
        });
      }

      try {
        const response = await api().mealPlansSave(request);

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        // Quick save can be used for rapid changes that don't need refreshed data returned like updating pantry shoppingItems.
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
        const response = await api().mealPlansDelete(id);

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
  },
});
