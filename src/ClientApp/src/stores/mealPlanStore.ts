import type {
  GetMealPlanResponse,
  GetMealPlanResponsePantryIngredient,
  IItemSetOfListMealPlansResponse,
  MealPlansSearchParams,
  SaveMealPlanRequest,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import DateHelpers from '@/models/DateHelpers';
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

const messageStore = useMessageStore();
const api = ApiHelpers.client;

let isInitialized = false;

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
    currentMealPlan: new GetMealPlanResponseClass(),
  }),

  getters: {
    pantry: (state) => state.currentMealPlan?.pantryIngredients || [],

    recipes: (state) => state.currentMealPlan?.recipes || [],

    shoppingList(state): GetMealPlanResponsePantryIngredient[] {
      const ingredientCounts = this.recipes
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, []);

      (state.currentMealPlan?.pantryIngredients || []).forEach((x) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        subtractIngredient(ingredientCounts, x.name!, x.quantity);
      });

      return ingredientCounts;
    },
  },

  actions: {
    async clearPantry() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.pantryIngredients = [];
      await this.saveCurrentMealPlan();
    },

    async addToPantry(ingredient: string, count = 1) {
      if (this.currentMealPlan === null) {
        return;
      }

      addIngredient(this.currentMealPlan.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealPlan();
    },

    async removeFromPantry(ingredient: string, count = 1) {
      if (this.currentMealPlan === null) {
        return;
      }

      subtractIngredient(this.currentMealPlan.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealPlan();
    },

    async clearRecipes() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.recipes = [];
      await this.saveCurrentMealPlan();
    },

    async fetchMealPlanList() {
      try {
        const response = await api().mealPlansSearch(this.mealPlanListRequest);
        this.mealPlanListResponse = response.data;

        if (!isInitialized) {
        }
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async newMealPlan() {
      this.currentMealPlan = new GetMealPlanResponseClass();
      this.currentMealPlan.name = DateHelpers.dateForView(DateHelpers.getThisOrNextDayOfWeek(1));

      await this.saveCurrentMealPlan();
    },

    async setCurrentMealPlan(mealPlanId: number | null | undefined) {
      if (isNil(mealPlanId)) {
        return;
      }

      try {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const response = await api().mealPlansGet(mealPlanId!);
        this.currentMealPlan = response.data;
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    unsetCurrentMealPlan() {
      this.newMealPlan();
    },

    async saveCurrentMealPlan() {
      const current = this.currentMealPlan;

      if (current === null) {
        return;
      }

      const request: SaveMealPlanRequest = {
        id: current.id,
        name: current.name,
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        recipeIds: this.recipes.map((x) => x.id!).filter((x) => !isNil(x)),
        pantryIngredients: current.pantryIngredients,
      };

      try {
        const response = await api().mealPlansSave(request);

        if (response.data.message) {
          messageStore.setSuccessMessage(response.data.message);
        }

        await this.fetchMealPlanList();
        await this.setCurrentMealPlan(response.data.id);
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async deleteCurrentMealPlan() {
      const id = this.currentMealPlan?.id;

      if (!id) {
        return;
      }

      try {
        const response = await api().mealPlansDelete(id);

        if (response.data.message) {
          messageStore.setSuccessMessage(response.data.message);
        }

        await this.fetchMealPlanList();

        if (this.currentMealPlan?.id === id) {
          this.unsetCurrentMealPlan();
        }
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
