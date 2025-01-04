import { defineStore } from 'pinia';
import type {
  GetRecipeResponse,
  SearchRecipesResultItem,
  SearchRecipesResponse,
  RecipesSearchParams,
  IItemSetOfSearchRecipesResultItem,
  SearchFacet,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import RecipesListRequest from '@/models/RecipesListRequest';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import useMessageStore from './messageStore';

const recentLimit = 7;

interface RecipeStoreState {
  listResponse: IItemSetOfSearchRecipesResultItem;
  listRequest: RecipesSearchParams;
  listFacets: SearchFacet[];
  recentRecipes: Array<SearchRecipesResultItem>;
  discoverList: SearchRecipesResultItem[];
  discoverPage: number;
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
    listRequest: { ...new RecipesListRequest(), take: Choices.defaultPaginationTake.value },
    recentRecipes: RecipeStoreHelper.getRecents(),
    discoverList: [],
    discoverPage: 0,
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

    setDiscoverListResponse(data: SearchRecipesResponse) {
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
      RecipeStoreHelper.storeRecents(this.recentRecipes);
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
      RecipeStoreHelper.storeRecents(this.recentRecipes);
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
      RecipeStoreHelper.storeRecents(this.recentRecipes);
    },

    queueRecent(recipe: GetRecipeResponse | null) {
      RecipeStoreHelper.storeQueuedRecent(recipe);
    },

    async fetchRecipesList() {
      try {
        const response = await api().recipesSearch(this.listRequest);
        const { results } = response.data;

        if (results) {
          this.listResponse = results;
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },
  },
});

export default useRecipeStore;
