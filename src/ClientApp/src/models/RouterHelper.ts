import type { RouteLocationNormalizedLoaded } from 'vue-router';
import type { GroceryItemsSearchParams } from '@/api/data-contracts';
import useAppStore from '@/stores/appStore';
import { isNil } from './FormatHelper';
import GroceryItemsSearchRequest from './GroceryItemsSearchRequest';
import GroceryItemStoreHelper from './GroceryItemStoreHelper';
import RecipesSearchRequest from './RecipesSearchRequest';
import RecipeStoreHelper from './RecipeStoreHelper';

interface RouteRecipe {
  id?: number;
  slug?: string | null;
}

interface RouteCategory {
  id?: number;
}

interface RouteGroceryItem {
  id?: number;
}

interface RouteGroceryItemFilter {
  id?: number;
}

interface RouteMealPlan {
  id?: number;
}

export default class RouterHelper {
  static setTitle(
    route: RouteLocationNormalizedLoaded,
    additionalTitle: string | null | undefined = null,
  ) {
    const appStore = useAppStore();
    const title = [additionalTitle, `${route.meta.title}`, appStore.applicationName]
      .filter(x => !isNil(x))
      .join(' | ');

    document.title = title;
  }

  static viewRecipe(recipe: RouteRecipe) {
    return { name: 'recipeView', params: { id: recipe.id, slug: recipe.slug } };
  }

  static editRecipe(recipe: RouteRecipe) {
    return { name: 'recipeEdit', params: { id: recipe.id, slug: recipe.slug } };
  }

  static searchRecipesByCategory(category: RouteCategory) {
    // Search all recipes, not just the meal planning ones.
    const query = RecipeStoreHelper.listRequestToQueryParams(
      { ...new RecipesSearchRequest(), categories: [category.id || 0], isForMealPlanning: null },
      false,
    );

    return { name: 'recipeList', query };
  }

  static editGroceryItem(groceryItem: RouteGroceryItem) {
    return { name: 'groceryItemEdit', params: { id: groceryItem.id } };
  }

  static searchGroceryItemsByStorageLocation(storageLocation: RouteGroceryItemFilter) {
    return RouterHelper.searchGroceryItems({ storageLocations: [storageLocation.id || 0] });
  }

  static searchGroceryItemsByGroceryStore(groceryStore: RouteGroceryItemFilter) {
    return RouterHelper.searchGroceryItems({ groceryStores: [groceryStore.id || 0] });
  }

  static searchGroceryItemsByGroceryAisle(groceryAisle: RouteGroceryItemFilter) {
    return RouterHelper.searchGroceryItems({ groceryAisles: [groceryAisle.id || 0] });
  }

  private static searchGroceryItems(filter: Partial<GroceryItemsSearchParams>) {
    const query = GroceryItemStoreHelper.listRequestToQueryParams({
      ...new GroceryItemsSearchRequest(),
      ...filter,
    });

    return { name: 'groceryItemList', query };
  }

  static editMealPlan(mealPlan: RouteMealPlan) {
    return { name: 'mealPlanEdit', params: { id: mealPlan.id } };
  }

  static paramToInt(param: string | string[] | undefined | null) {
    if (Array.isArray(param)) {
      return param.length > 0 && param[0] ? Number.parseInt(param[0], 10) : 0;
    }

    if (typeof param === 'string') {
      return Number.parseInt(param, 10);
    }

    return 0;
  }

  static scrollToTop(behavior: ScrollBehavior = 'smooth') {
    document.getElementById('main')?.focus();

    window.scrollTo({
      top: 0,
      behavior,
    });
  }
}
