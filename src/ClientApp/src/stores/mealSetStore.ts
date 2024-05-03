import type {
  GetMealSetResponse,
  GetMealSetResponsePantryIngredient,
  IItemSetOfListMealSetsResponse,
  MealSetsSearchParams,
  SaveMealSetRequest,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import DateHelpers from '@/models/DateHelpers';
import { isNil } from '@/models/FormatHelpers';
import GetMealSetResponseClass from '@/models/GetMealSetResponseClass';
import Choices from '@/models/Choices';
import { defineStore } from 'pinia';
import useMessageStore from './messageStore';
import {
  countIngredients,
  subtractIngredient,
  addIngredient,
} from '../models/PantryIngredientHelpers';

interface MealSetStoreState {
  mealSetListResponse: IItemSetOfListMealSetsResponse;
  mealSetListRequest: MealSetsSearchParams;
  currentMealSet: GetMealSetResponse;
}

const messageStore = useMessageStore();
const api = ApiHelpers.client;

let isInitialized = false;

export default defineStore('mealSets', {
  state: (): MealSetStoreState => ({
    mealSetListResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    mealSetListRequest: {
      name: '',
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
    },
    currentMealSet: new GetMealSetResponseClass(),
  }),

  getters: {
    pantry: (state) => state.currentMealSet?.pantryIngredients || [],

    recipes: (state) => state.currentMealSet?.recipes || [],

    shoppingList(state): GetMealSetResponsePantryIngredient[] {
      const ingredientCounts = this.recipes
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, []);

      (state.currentMealSet?.pantryIngredients || []).forEach((x) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        subtractIngredient(ingredientCounts, x.name!, x.quantity);
      });

      return ingredientCounts;
    },
  },

  actions: {
    async clearPantry() {
      if (this.currentMealSet === null) {
        return;
      }

      this.currentMealSet.pantryIngredients = [];
      await this.saveCurrentMealSet();
    },

    async addToPantry(ingredient: string, count = 1) {
      if (this.currentMealSet === null) {
        return;
      }

      addIngredient(this.currentMealSet.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealSet();
    },

    async removeFromPantry(ingredient: string, count = 1) {
      if (this.currentMealSet === null) {
        return;
      }

      subtractIngredient(this.currentMealSet.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealSet();
    },

  async clearRecipes() {
      if (this.currentMealSet === null) {
        return;
      }

      this.currentMealSet.recipes = [];
      await this.saveCurrentMealSet();
    },

    async fetchMealSetList() {
      try {
        const response = await api().mealSetsSearch(this.mealSetListRequest);
        this.mealSetListResponse = response.data;

        if (!isInitialized) {

        }
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async newMealSet() {
      this.currentMealSet = new GetMealSetResponseClass();
      this.currentMealSet.name = DateHelpers.dateForView(DateHelpers.getThisOrNextDayOfWeek(1));

      await this.saveCurrentMealSet();
    },

    async setCurrentMealSet(mealSetId: number | null | undefined) {
      if (isNil(mealSetId)) {
        return;
      }

      try {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const response = await api().mealSetsGet(mealSetId!);
        this.currentMealSet = response.data;
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    unsetCurrentMealSet() {
      this.newMealSet();
    },

    async saveCurrentMealSet() {
      const current = this.currentMealSet;

      if (current === null) {
        return;
      }

      const request: SaveMealSetRequest = {
        id: current.id,
        name: current.name,
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        recipeIds: this.recipes.map((x) => x.id!).filter((x) => !isNil(x)),
        pantryIngredients: current.pantryIngredients,
      };

      try {
        const response = await api().mealSetsSave(request);

        if (response.data.message) {
          messageStore.setSuccessMessage(response.data.message);
        }

        await this.fetchMealSetList();
        await this.setCurrentMealSet(response.data.id);
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async deleteCurrentMealSet() {
      const id = this.currentMealSet?.id;

      if (!id) {
        return;
      }

      try {
        const response = await api().mealSetsDelete(id);

        if (response.data.message) {
          messageStore.setSuccessMessage(response.data.message);
        }

        await this.fetchMealSetList();

        if (this.currentMealSet?.id === id) {
          this.unsetCurrentMealSet();
        }
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});
