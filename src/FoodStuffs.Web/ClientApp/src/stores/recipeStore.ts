import { defineStore } from 'pinia';
import type {
  ListRecipesResponse,
  ListRecipesResponseIItemSet,
  RecipesListParams,
} from '@/api/data-contracts';

interface RecipeStoreState {
  listResponse: ListRecipesResponseIItemSet;
  listRequest: RecipesListParams;
  recentRecipes: Array<ListRecipesResponse>;
}

export const useRecipeStore = defineStore('recipes', {
  state: (): RecipeStoreState => ({
    listResponse: {},
    listRequest: {},
    recentRecipes: [],
  }),

  getters: {},

  actions: {
    setListResponse(data: ListRecipesResponseIItemSet) {
      this.listResponse = data;
    },

    setListRequest(data: RecipesListParams) {
      this.listRequest = data;
    },

    addToRecent(recipe: ListRecipesResponse) {
      const limit = 3;

      if (recipe === null) {
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

      const limitedRecipes = recentRecipes.slice(0, limit);

      this.recentRecipes = limitedRecipes;
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
    },

    updateRecent(recipe: ListRecipesResponse) {
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
    },
  },
});

export default useRecipeStore;
