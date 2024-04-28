import type {
  GetMealSetResponse,
  GetMealSetResponsePantryIngredient,
  IItemSetOfListMealSetsResponse,
  MealSetsSearchParams,
  RecipeSearchResultItemIngredient,
  SaveMealSetRequest,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import ApiHelpers from '@/models/ApiHelpers';
import DateHelpers from '@/models/DateHelpers';
import { isNil } from '@/models/FormatHelpers';
import GetMealSetResponseClass from '@/models/GetMealSetResponseClass';
import { defineStore } from 'pinia';
import useMessageStore from './messageStore';

interface MealSetStoreState {
  mealSetListResponse: IItemSetOfListMealSetsResponse;
  mealSetListRequest: MealSetsSearchParams;
  currentMealSet: GetMealSetResponse | null;
}

const messageStore = useMessageStore();
const api = ApiHelpers.client;

function countIngredients(
  acc: GetMealSetResponsePantryIngredient[],
  curr: RecipeSearchResultItemIngredient
) {
  const { name, quantity } = curr;

  if (isNil(name)) {
    return acc;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  let match = acc.find((x) => x.name === name!);

  if (!match) {
    match = { name, quantity: 0 };
    acc.push(match);
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  match.quantity! += quantity || 0;
  return acc;
}

function addCount(ingredients: GetMealSetResponsePantryIngredient[], name: string, count = 1) {
  let ingredient = ingredients.find((x) => x.name === name);

  if (!ingredient) {
    ingredient = { name, quantity: 0 };
    ingredients.push(ingredient);
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  ingredient.quantity! += count;
}

function subtractCount(ingredients: GetMealSetResponsePantryIngredient[], name: string, count = 1) {
  const ingredient = ingredients.find((x) => x.name === name);

  if (!ingredient) {
    return;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  ingredient.quantity! -= count;

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  if (ingredient.quantity! < 1) {
    ingredients.splice(ingredients.indexOf(ingredient), 1);
  }
}

function getRecipes(state: MealSetStoreState) {
  return state.currentMealSet?.recipes || [];
}

export default defineStore('mealSets', {
  state: (): MealSetStoreState => ({
    mealSetListResponse: {
      count: 0,
      items: [],
      isPagingEnabled: false,
      page: 1,
      take: -1,
      totalCount: 0,
    },
    mealSetListRequest: {
      name: '',
      isPagingEnabled: false,
      page: 1,
      take: -1,
    },
    currentMealSet: null,
  }),

  getters: {
    getPantry: (state) => state.currentMealSet?.pantryIngredients || [],

    getRecipes: (state) => getRecipes(state),

    getShoppingList: (state) => {
      const ingredientCounts = getRecipes(state)
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, []);

      (state.currentMealSet?.pantryIngredients || []).forEach((x) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        subtractCount(ingredientCounts, x.name!, x.quantity);
      });

      return ingredientCounts;
    },

    isRecipeInCurrentMealSet: (state) => (id: number | null | undefined) =>
      getRecipes(state).findIndex((x) => x.id === id) > -1,
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

      addCount(this.currentMealSet.pantryIngredients || [], ingredient, count);
      await this.saveCurrentMealSet();
    },

    async removeFromPantry(ingredient: string, count = 1) {
      if (this.currentMealSet === null) {
        return;
      }

      subtractCount(this.currentMealSet.pantryIngredients || [], ingredient, count);
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
      this.currentMealSet = null;
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
        recipeIds: this.getRecipes.map((x) => x.id!).filter((x) => !isNil(x)),
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
      const { id } = this.currentMealSet;

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
