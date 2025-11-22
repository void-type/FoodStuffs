import type {
  GetRecipeResponse,
  RecipesSearchParams,
  SearchRecipesResultItem,
} from '@/api/data-contracts';
import RecipesSearchRequest from './RecipesSearchRequest';

const settingsKeyRecentRecipes = 'recentRecipes';
const settingsKeyQueuedRecentRecipe = 'queuedRecentRecipe';

export default class RecipeStoreHelper {
  static getRecents() {
    return JSON.parse(
      localStorage.getItem(settingsKeyRecentRecipes) || '[]',
    ) as SearchRecipesResultItem[];
  }

  static storeRecents(recentRecipes: Array<SearchRecipesResultItem>) {
    localStorage.setItem(settingsKeyRecentRecipes, JSON.stringify(recentRecipes));
  }

  static getQueuedRecent() {
    return JSON.parse(
      localStorage.getItem(settingsKeyQueuedRecentRecipe) || 'null',
    ) as GetRecipeResponse | null;
  }

  static storeQueuedRecent(recipe: GetRecipeResponse | null) {
    if (recipe === null) {
      return;
    }

    localStorage.setItem(settingsKeyQueuedRecentRecipe, JSON.stringify(recipe));
  }

  static listRequestToQueryParams(listRequest: RecipesSearchParams) {
    // Query params need to be string or number
    const requestEntries = Object.entries({
      ...listRequest,
      categories: listRequest.categories?.join() || '',
    });

    const defaultEntries = Object.entries({
      ...new RecipesSearchRequest(),
      categories: new RecipesSearchRequest().categories?.join() || '',
    });

    const cleanedEntries = requestEntries
      .filter(([rKey, rValue]) => {
        // Get the matching default value
        const dValue = defaultEntries.find(([dKey]) => dKey === rKey)?.[1];
        // Compare the values
        return JSON.stringify(rValue) !== JSON.stringify(dValue);
      })
      .map(([qpKey, qpValue]) => [qpKey, String(qpValue)]);

    return Object.fromEntries(cleanedEntries);
  }
}
