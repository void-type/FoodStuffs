import type {
  GetMealPlanResponse,
  GetMealPlanResponsePantryIngredient,
  IItemSetOfListMealPlansResponse,
  MealPlansSearchParams,
  SaveMealPlanRequest,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import { isNil } from '@/models/FormatHelpers';
import GetMealPlanResponseClass from '@/models/GetMealPlanResponseClass';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import useMessageStore from './messageStore';
import {
  countIngredients,
  subtractIngredient,
  addIngredient,
} from '../models/PantryIngredientHelpers';

interface MealPlanStoreState {
  mealPlanListResponse: IItemSetOfListMealPlansResponse;
  mealPlanListRequest: MealPlansSearchParams;
  currentMealPlan: GetMealPlanResponse;
}

const api = ApiHelpers.client;

export default defineStore('mealPlans', {
  state: (): MealPlanStoreState => ({
    mealPlanListResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    mealPlanListRequest: {
      name: '',
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
    },
    currentMealPlan: GetMealPlanResponseClass.createForStore(),
  }),

  getters: {
    currentPantry: (state) => state.currentMealPlan.pantryIngredients || [],

    currentRecipes: (state) => state.currentMealPlan.recipes || [],

    currentRecipesContains: (state) => (recipeId: number | undefined) =>
      (state.currentMealPlan.recipes || []).map((x) => x.id).includes(recipeId),

    currentShoppingList(state): GetMealPlanResponsePantryIngredient[] {
      const ingredientCounts = this.currentRecipes
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, []);

      (state.currentMealPlan.pantryIngredients || []).forEach((x) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        subtractIngredient(ingredientCounts, x.name!, x.quantity);
      });

      return ingredientCounts;
    },
  },

  actions: {
    async clearCurrentPantry() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.pantryIngredients = [];
      await this.saveCurrentMealPlan();
    },

    async addToCurrentPantry(ingredient: string, count = 1) {
      if (this.currentMealPlan === null) {
        return;
      }

      addIngredient(this.currentMealPlan.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealPlan();
    },

    async removeFromCurrentPantry(ingredient: string, count = 1) {
      if (this.currentMealPlan === null) {
        return;
      }

      subtractIngredient(this.currentMealPlan.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealPlan();
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
      // TODO: remove currentMealSet from localStorage
    },

    async setCurrentMealPlan(mealPlanId: number | null | undefined) {
      if (isNil(mealPlanId)) {
        return;
      }

      try {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const response = await api().mealPlansGet(mealPlanId!);
        this.currentMealPlan = response.data;
        // TODO: set currentMealSet in localStorage
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        this.newCurrentMealPlan();
      }
    },

    async saveCurrentMealPlan(additionalRecipeIds: number[] = []) {
      const current = this.currentMealPlan;

      if (current === null) {
        return;
      }

      const request: SaveMealPlanRequest = {
        id: current.id,
        name: current.name,
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        recipeIds: this.currentRecipes.map((x) => x.id!).filter((x) => !isNil(x)),
        pantryIngredients: current.pantryIngredients,
      };

      if (additionalRecipeIds) {
        additionalRecipeIds.forEach((additionalId) => {
          request.recipeIds?.push(additionalId);
        });
      }

      try {
        const response = await api().mealPlansSave(request);

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        await this.setCurrentMealPlan(response.data.id);
        await this.fetchMealPlanList();
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async deleteCurrentMealPlan() {
      const { id } = this.currentMealPlan;

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
        const response = await api().mealPlansSearch(this.mealPlanListRequest);
        this.mealPlanListResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
