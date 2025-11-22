import type {
  GetRecipeResponse,
  IItemSetOfSearchRecipesResultItem,
  RecipesSearchParams,
  SearchFacet,
  SearchRecipesResponse,
  SearchRecipesResultItem,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import { defineStore } from 'pinia';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import RecipesSearchRequest from '@/models/RecipesSearchRequest';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import useMessageStore from './messageStore';

const recentLimit = 7;

interface RecipeStoreState {
  listResponse: IItemSetOfSearchRecipesResultItem;
  listRequest: RecipesSearchParams;
  listFacets: SearchFacet[];
  recentRecipes: Array<SearchRecipesResultItem>;
}

const api = ApiHelper.client;

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
    listRequest: { ...new RecipesSearchRequest(), take: Choices.defaultPaginationTake.value },
    recentRecipes: RecipeStoreHelper.getRecents(),
  }),

  getters: {
    currentQueryParams(state) {
      const { listRequest } = state;

      return RecipeStoreHelper.listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    setListResponse(data: SearchRecipesResponse) {
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

    addToRecent(recipe: GetRecipeResponse | null) {
      if (recipe === null || typeof recipe === 'undefined') {
        return;
      }

      const recentRecipes = this.recentRecipes.slice();

      const indexOfCurrentInRecents = recentRecipes
        .map(recentRecipe => recentRecipe.id)
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
      RecipeStoreHelper.storeRecents(this.recentRecipes);
    },

    removeFromRecent(id: number) {
      const recentRecipes = this.recentRecipes.slice();

      const indexOfCurrentInRecents = recentRecipes.findIndex(
        recentRecipe => recentRecipe.id === id,
      );

      if (indexOfCurrentInRecents > -1) {
        recentRecipes.splice(indexOfCurrentInRecents, 1);
      }

      this.recentRecipes = recentRecipes;
      RecipeStoreHelper.storeRecents(this.recentRecipes);
    },

    updateRecent(recipe: GetRecipeResponse | null) {
      if (recipe === null) {
        return;
      }

      const recentRecipes = this.recentRecipes.slice();

      const indexOfCurrentInRecents = recentRecipes
        .map(recentRecipe => recentRecipe.id)
        .indexOf(recipe.id);

      if (indexOfCurrentInRecents < 0) {
        return;
      }

      const recipeListItem = {
        ...recipe,
      };

      recentRecipes[indexOfCurrentInRecents] = recipeListItem;

      this.recentRecipes = recentRecipes;
      RecipeStoreHelper.storeRecents(this.recentRecipes);
    },

    queueRecent(recipe: GetRecipeResponse | null) {
      RecipeStoreHelper.storeQueuedRecent(recipe);
    },

    async fetchRecipesList() {
      try {
        const response = await api().recipesSearch(this.listRequest);

        if (response.data) {
          this.setListResponse(response.data);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});

export default useRecipeStore;
