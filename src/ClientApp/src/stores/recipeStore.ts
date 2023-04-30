import { defineStore } from 'pinia';
import type {
  GetRecipeResponse,
  ListRecipesResponse,
  ListRecipesResponseIItemSet,
  RecipesListParams,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';

const recentLimit = 5;

interface RecipeStoreState {
  listResponse: ListRecipesResponseIItemSet;
  listRequest: RecipesListParams;
  recentRecipes: Array<ListRecipesResponse>;
}

export const useRecipeStore = defineStore('recipes', {
  state: (): RecipeStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: new ListRecipesRequest(),
    recentRecipes: RecipeStoreHelpers.getRecents(),
  }),

  getters: {
    currentQueryParams(state) {
      // Query params need to be string or number
      return {
        ...state.listRequest,
        isForMealPlanning: String(state.listRequest.isForMealPlanning),
        sortDesc: String(state.listRequest.sortDesc),
        isPagingEnabled: String(state.listRequest.isPagingEnabled),
      };
    },
  },

  actions: {
    setListResponse(data: ListRecipesResponseIItemSet) {
      this.listResponse = data;
    },

    setListRequest(data: RecipesListParams) {
      this.listRequest = data;
    },

    addToRecent(recipe: GetRecipeResponse | null) {
      if (recipe === null || typeof recipe === 'undefined') {
        return;
      }

      const recentRecipes = this.recentRecipes.slice();

      const indexOfCurrentInRecents = recentRecipes
        .map((recentRecipe) => recentRecipe.id)
        .indexOf(recipe.id);

      const recipeListItem = {
        ...recipe,
      };

      if (indexOfCurrentInRecents > -1) {
        recentRecipes.splice(indexOfCurrentInRecents, 1);
      }

      if ((recipe.id || 0) > 0) {
        recentRecipes.unshift(recipeListItem);
      }

      const limitedRecipes = recentRecipes.slice(0, recentLimit);

      this.recentRecipes = limitedRecipes;
      RecipeStoreHelpers.storeRecents(this.recentRecipes);
    },

    removeFromRecent(id: number) {
      const recentRecipes = this.recentRecipes.slice();

      const indexOfCurrentInRecents = recentRecipes
        .map((recentRecipe) => recentRecipe.id)
        .indexOf(id);

      if (indexOfCurrentInRecents > -1) {
        recentRecipes.splice(indexOfCurrentInRecents, 1);
      }

      this.recentRecipes = recentRecipes;
      RecipeStoreHelpers.storeRecents(this.recentRecipes);
    },

    updateRecent(recipe: GetRecipeResponse | null) {
      if (recipe === null) {
        return;
      }

      const recentRecipes = this.recentRecipes.slice();

      const indexOfCurrentInRecents = recentRecipes
        .map((recentRecipe) => recentRecipe.id)
        .indexOf(recipe.id);

      if (indexOfCurrentInRecents < 0) {
        return;
      }

      const recipeListItem = {
        ...recipe,
      };

      recentRecipes[indexOfCurrentInRecents] = recipeListItem;

      this.recentRecipes = recentRecipes;
      RecipeStoreHelpers.storeRecents(this.recentRecipes);
    },

    queueRecent(recipe: GetRecipeResponse | null) {
      RecipeStoreHelpers.storeQueuedRecent(recipe);
    },
  },
});

export default useRecipeStore;
