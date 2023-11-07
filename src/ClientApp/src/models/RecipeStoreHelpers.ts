import type {
  ListRecipesResponse,
  GetRecipeResponse,
  RecipesListParams,
} from '@/api/data-contracts';
import ListRecipesRequest from './ListRecipesRequest';

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

  static listRequestToQueryParams(listRequest: RecipesListParams) {
    const defaultRequest = new ListRecipesRequest();

    // Query params need to be string or number
    const qp = {
      ...listRequest,
    };

    const qpEntries = Object.entries(qp);
    const defaultValues = Object.entries(defaultRequest);

    const cleanedEntries = qpEntries
      .filter(([qpKey, qpValue]) => {
        const dValue = defaultValues.find(([q]) => q === qpKey)?.[1];
        return qpValue !== dValue && typeof qpValue !== 'undefined';
      })
      .map(([qpKey, qpValue]) => [qpKey, String(qpValue)]);

    return Object.fromEntries(cleanedEntries);
  }
}
