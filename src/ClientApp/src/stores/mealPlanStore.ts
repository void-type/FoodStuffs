import type {
  GetMealPlanResponse,
  GetMealPlanResponsePantryShoppingItem,
  IItemSetOfListMealPlansResponse,
  MealPlansListParams,
  SaveMealPlanRequest,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import { isNil } from '@/models/FormatHelpers';
import GetMealPlanResponseClass from '@/models/GetMealPlanResponseClass';
import SearchMealPlansRequest from '@/models/SearchMealPlansRequest';
import MealPlanStoreHelpers from '@/models/MealPlanStoreHelpers';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import useMessageStore from './messageStore';
import {
  countIngredients,
  subtractIngredient,
  addIngredient,
} from '../models/PantryIngredientHelpers';

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
    currentMealPlan: GetMealPlanResponseClass.createForStore(),
  }),

  getters: {
    currentPantry: (state) => state.currentMealPlan.pantryShoppingItems || [],

    currentRecipes: (state) => state.currentMealPlan.recipes || [],

    currentRecipesContains: (state) => (recipeId: number | null | undefined) => {
      if (isNil(recipeId)) {
        return false;
      }

      return (state.currentMealPlan.recipes || []).map((x) => x.id).includes(recipeId!);
    },

    currentShoppingList(state): GetMealPlanResponsePantryShoppingItem[] {
      const ingredientCounts = this.currentRecipes
        .flatMap((c) => c.shoppingItems || [])
        .reduce(countIngredients, []);

      (state.currentMealPlan.pantryShoppingItems || []).forEach((x) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        subtractIngredient(ingredientCounts, x.name!, x.quantity);
      });

      return ingredientCounts;
    },

    currentQueryParams(state) {
      const { listRequest } = state;

      return MealPlanStoreHelpers.listRequestToQueryParams(listRequest);
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

    async addToCurrentPantry(ingredient: string, count = 1) {
      if (this.currentMealPlan === null) {
        return;
      }

      addIngredient(this.currentMealPlan.pantryShoppingItems || [], ingredient, count);
      await this.saveCurrentMealPlan([], true);
    },

    async removeFromCurrentPantry(ingredient: string, count = 1) {
      if (this.currentMealPlan === null) {
        return;
      }

      subtractIngredient(this.currentMealPlan.pantryShoppingItems || [], ingredient, count);
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

      await this.saveCurrentMealPlan([recipeId!]);
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
      this.currentMealPlan = GetMealPlanResponseClass.createForStore();
      MealPlanStoreHelpers.storeCurrentMealPlan(null);
    },

    async setCurrentMealPlan(mealPlanId: number | null | undefined) {
      if (isNil(mealPlanId)) {
        return;
      }

      try {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const response = await api().mealPlansGet(mealPlanId!);
        this.currentMealPlan = response.data;
        if (response.data.id) {
          MealPlanStoreHelpers.storeCurrentMealPlan(response.data.id);
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

      const request: SaveMealPlanRequest = {
        id: current.id,
        name: current.name,
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        recipes: this.currentRecipes
          .map((x) => x.id!)
          .filter((x) => !isNil(x))
          .map((id, order) => ({
            id,
            order,
          })),
        pantryShoppingItems: current.pantryShoppingItems,
      };

      if (additionalRecipeIds && additionalRecipeIds.length > 0) {
        additionalRecipeIds.forEach((additionalId) => {
          if (typeof request.recipes === 'undefined') {
            return;
          }

          const lastIndex = (request.recipes.length || 0) - 1;
          const lastOrder = request.recipes[lastIndex].order || 0;

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

        // Quick save can be used for rapid changes that don't need refreshed data returned like updating pantry ingredients.
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
