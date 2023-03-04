import type { ListRecipesResponse, GetRecipeResponse } from '@/api/data-contracts';

const settingsKeyRecentRecipes = 'recentRecipes';
const settingsKeyQueuedRecentRecipe = 'queuedRecentRecipe';

export default class RecipeStoreHelpers {
  static getRecents() {
    return JSON.parse(
      localStorage.getItem(settingsKeyRecentRecipes) || '[]'
    ) as ListRecipesResponse[];
  }

  static storeRecents(recentRecipes: Array<ListRecipesResponse>) {
    localStorage.setItem(settingsKeyRecentRecipes, JSON.stringify(recentRecipes));
  }

  static getQueuedRecent() {
    return JSON.parse(
      localStorage.getItem(settingsKeyQueuedRecentRecipe) || 'null'
    ) as GetRecipeResponse | null;
  }

  static storeQueuedRecent(recipe: GetRecipeResponse | null) {
    if (recipe === null) {
      return;
    }

    localStorage.setItem(settingsKeyQueuedRecentRecipe, JSON.stringify(recipe));
  }
}
