import type {
  SearchRecipesResponse,
  GetRecipeResponse,
  RecipesListParams,
} from '@/api/data-contracts';
import SearchRecipesRequest from './SearchRecipesRequest';

const settingsKeyRecentRecipes = 'recentRecipes';
const settingsKeyQueuedRecentRecipe = 'queuedRecentRecipe';

export default class RecipeStoreHelpers {
  static getRecents() {
    return JSON.parse(
      localStorage.getItem(settingsKeyRecentRecipes) || '[]'
    ) as SearchRecipesResponse[];
  }

  static storeRecents(recentRecipes: Array<SearchRecipesResponse>) {
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
    { text: 'Relevance', value: '' },
    { text: 'Newest', value: 'newest' },
    { text: 'Oldest', value: 'oldest' },
    { text: 'A-Z', value: 'a-z' },
    { text: 'Z-A', value: 'z-a' },
    { text: 'Random', value: 'random' },
  ];

  static listRequestToQueryParams(listRequest: RecipesListParams) {
    // Query params need to be string or number
    const requestEntries = Object.entries({
      ...listRequest,
      categories: listRequest.categories?.join() || '',
    });

    const defaultEntries = Object.entries({
      ...new SearchRecipesRequest(),
      categories: new SearchRecipesRequest().categories?.join() || '',
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
