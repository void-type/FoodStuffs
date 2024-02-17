import { defineStore } from 'pinia';
import type {
  GetRecipeResponse,
  SearchRecipesResponse,
  SearchRecipesResponseIItemSet,
  RecipesListParams,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';

const recentLimit = 10;

interface RecipeStoreState {
  listResponse: SearchRecipesResponseIItemSet;
  listRequest: RecipesListParams;
  recentRecipes: Array<SearchRecipesResponse>;
  discoverList: SearchRecipesResponse[];
  discoverPage: number;
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
    listRequest: { ...new SearchRecipesRequest(), take: Choices.defaultPaginationTake.value },
    recentRecipes: RecipeStoreHelpers.getRecents(),
    discoverList: [],
    discoverPage: 0,
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return RecipeStoreHelpers.listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    setListResponse(data: SearchRecipesResponseIItemSet) {
      this.listResponse = data;
    },

    setListRequest(data: RecipesListParams) {
      this.listRequest = data;
    },

    setDiscoverListResponse(data: SearchRecipesResponseIItemSet) {
      if (!data.items) {
        this.discoverPage = 0;
        this.discoverList = [];
        return;
      }

      if (data.page === 1) {
        this.discoverList = [];
      }

      this.discoverPage = data.page || 0;

      // If it wasn't a full page, make the next request load this page again.
      if ((data.count || 0) < (data.take || 1) && this.discoverPage > 1) {
        this.discoverPage -= 1;
      }

      const newItems = data.items.filter(
        (newItem) => !this.discoverList.some((existingItem) => existingItem.id === newItem.id)
      );

      if (newItems.length > 0) {
        this.discoverList = [...this.discoverList, ...newItems];
      }
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
