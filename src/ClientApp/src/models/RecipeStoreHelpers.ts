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

  static sortOptions = [
    { text: 'Newest', value: 'newest' },
    { text: 'Oldest', value: 'oldest' },
    { text: 'A-Z', value: 'a-z' },
    { text: 'Z-A', value: 'z-a' },
    { text: 'Random', value: 'random' },
  ];
}
