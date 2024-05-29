import { defineStore } from 'pinia';
import type {
  GetRecipeResponse,
  RecipeSearchResultItem,
  RecipeSearchResponse,
  RecipesSearchParams,
  IItemSetOfRecipeSearchResultItem,
  RecipeSearchFacet,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';

const recentLimit = 7;

interface RecipeStoreState {
  listResponse: IItemSetOfRecipeSearchResultItem;
  listRequest: RecipesSearchParams;
  listFacets: RecipeSearchFacet[];
  recentRecipes: Array<RecipeSearchResultItem>;
  discoverList: RecipeSearchResultItem[];
  discoverPage: number;
}

export const useRecipeStore = defineStore('recipe', {
  state: (): RecipeStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listFacets: [],
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
    setListResponse(data: RecipeSearchResponse) {
      if (data.results) {
        this.listResponse = data.results;
      }

      if (data.facets) {
        this.listFacets = data.facets;
      }
    },

    setListRequest(data: RecipesSearchParams) {
      this.listRequest = data;
    },

    setDiscoverListResponse(data: RecipeSearchResponse) {
      const { results } = data;

      if (!results?.items) {
        this.discoverPage = 0;
        this.discoverList = [];
        return;
      }

      if (results.page === 1) {
        this.discoverList = [];
      }

      this.discoverPage = results.page || 0;

      // If it wasn't a full page, make the next request load this page again.
      if ((results.count || 0) < (results.take || 1) && this.discoverPage > 1) {
        this.discoverPage -= 1;
      }

      const newItems = results.items.filter(
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
