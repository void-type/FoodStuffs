import type {
  GetMealSetResponse,
  GetMealSetResponsePantryIngredient,
  ListMealSetsResponseIItemSet,
  MealSetsListParams,
  RecipesListParams,
  RecipeSearchResultItemIItemSet,
  RecipeSearchResultItemIngredient,
  RecipeSearchFacet,
  RecipeSearchResponse,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import DateHelpers from '@/models/DateHelpers';
import { isNil } from '@/models/FormatHelpers';
import GetMealSetResponseClass from '@/models/GetMealSetResponseClass';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import { defineStore } from 'pinia';

interface MealStoreState {
  recipeListResponse: RecipeSearchResultItemIItemSet;
  recipeListRequest: RecipesListParams;
  recipeListFacets: RecipeSearchFacet[];
  mealSetListResponse: ListMealSetsResponseIItemSet;
  mealSetListRequest: MealSetsListParams;
  mealSetListIndex: number;
  currentMealSet: GetMealSetResponse;
}

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
      take: Choices.paginationTake[0].value,
      totalCount: 0,
    },
    recipeListRequest: {
      ...new SearchRecipesRequest(),
      take: Choices.paginationTake[0].value,
      isForMealPlanning: true,
    },
    recipeListFacets: [],
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
    mealSetListIndex: -1,
    currentMealSet: new GetMealSetResponseClass() as GetMealSetResponse,
  }),

  getters: {
    getPantry: (state) => state.currentMealSet.pantryIngredients || [],

    getSelectedRecipes: (state) => getSelectedRecipes(state),

    getShoppingList: (state) => {
      const ingredientCounts = getSelectedRecipes(state)
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, []);

      (state.currentMealSet.pantryIngredients || []).forEach((x) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        subtractCount(ingredientCounts, x.name!, x.quantity);
      });

      return ingredientCounts;
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
      this.currentMealSet.pantryIngredients = [];
    },

    addToPantry(ingredient: string) {
      addCount(this.currentMealSet.pantryIngredients || [], ingredient);
    },

    removeFromPantry(ingredient: string) {
      subtractCount(this.currentMealSet.pantryIngredients || [], ingredient);
    },

    clearSelectedRecipes() {
      this.currentMealSet.recipes = [];
    },

    setListResponse(data: RecipeSearchResponse) {
      if (data.results) {
        this.recipeListResponse = data.results;
      }

      if (data.facets) {
        this.recipeListFacets = data.facets;
      }
    },

    newMealSet() {
      this.currentMealSet = new GetMealSetResponseClass();
      this.currentMealSet.name = DateHelpers.dateForView(DateHelpers.getThisOrNextDayOfWeek(1));
      this.mealSetListIndex = -1;
    },
  },
});
