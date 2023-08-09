import type {
  GetMealSetResponse,
  ListMealSetsResponseIItemSet,
  ListRecipesResponseIItemSet,
  ListRecipesResponseIngredient,
  MealSetsListParams,
  RecipesListParams,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import DateHelpers from '@/models/DateHelpers';
import { isNil } from '@/models/FormatHelpers';
import GetMealSetResponseClass from '@/models/GetMealSetResponseClass';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import { defineStore } from 'pinia';

interface MealStoreState {
  recipeListResponse: ListRecipesResponseIItemSet;
  recipeListRequest: RecipesListParams;
  mealSetListResponse: ListMealSetsResponseIItemSet;
  mealSetListRequest: MealSetsListParams;
  mealSetListIndex: number;
  currentMealSet: GetMealSetResponse;
  pantry: Record<string, number>;
}

function countIngredients(acc: Record<string, number>, curr: ListRecipesResponseIngredient) {
  const { name, quantity } = curr;

  if (isNil(name)) {
    return acc;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  if (!acc[name!]) {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    acc[name!] = 0;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  acc[name!] += quantity || 0;
  return acc;
}

function addCount(dict: Record<string, number>, key: string, count = 1) {
  if (!dict[key]) {
    // eslint-disable-next-line no-param-reassign
    dict[key] = 0;
  }

  // eslint-disable-next-line no-param-reassign
  dict[key] += count;
}

function subtractCount(dict: Record<string, number>, key: string, count = 1) {
  if (!dict[key]) {
    return;
  }

  // eslint-disable-next-line no-param-reassign
  dict[key] -= count;

  if (dict[key] < 1) {
    // eslint-disable-next-line no-param-reassign
    delete dict[key];
  }
}

function getSelectedRecipes(state: MealStoreState) {
  return state.currentMealSet.recipes || [];
}

export default defineStore('meals', {
  state: (): MealStoreState => ({
    recipeListResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    recipeListRequest: {
      ...new ListRecipesRequest(),
      isForMealPlanning: true,
    },
    mealSetListResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    mealSetListRequest: {
      isPagingEnabled: false,
    },
    mealSetListIndex: -1,
    currentMealSet: new GetMealSetResponseClass() as GetMealSetResponse,
    pantry: {},
  }),

  getters: {
    getPantry: (state) => Object.entries(state.pantry),

    getSelectedRecipes: (state) => getSelectedRecipes(state),

    getShoppingList: (state) => {
      const ingredientCounts = getSelectedRecipes(state)
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, {});

      Object.entries(state.pantry).forEach(([ingredient, quantity]) => {
        subtractCount(ingredientCounts, ingredient, quantity);
      });

      return Object.entries(ingredientCounts);
    },

    isRecipeSelected: (state) => (id: number | null | undefined) =>
      getSelectedRecipes(state).findIndex((x) => x.id === id) > -1,
  },

  actions: {
    toggleRecipe(id: number) {
      const index = this.getSelectedRecipes.findIndex((x) => x.id === id);

      if (index > -1) {
        this.getSelectedRecipes.splice(index, 1);
        return;
      }

      const recipe = this.recipeListResponse.items?.find((c) => c.id === id);

      if (typeof recipe === 'undefined' || recipe === null) {
        return;
      }

      this.getSelectedRecipes.push(recipe);
    },

    clearPantry() {
      this.pantry = {};
    },

    addToPantry(ingredient: string) {
      addCount(this.pantry, ingredient);
    },

    removeFromPantry(ingredient: string) {
      subtractCount(this.pantry, ingredient);
    },

    clearSelectedRecipes() {
      this.currentMealSet.recipes = [];
    },

    newMealSet() {
      this.currentMealSet = new GetMealSetResponseClass();
      this.currentMealSet.name = DateHelpers.dateForView(DateHelpers.getThisOrNextDay(1));
      this.mealSetListIndex = -1;
    },
  },
});
