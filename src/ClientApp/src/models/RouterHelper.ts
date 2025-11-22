import type { RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';
import { isNil } from './FormatHelper';

interface RouteRecipe {
  id?: number;
  slug?: string | null;
}

interface RouteGroceryItem {
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

  static editGroceryItem(groceryItem: RouteGroceryItem) {
    return { name: 'groceryItemEdit', params: { id: groceryItem.id } };
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

  static scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });

    document.getElementById('main')?.focus();
  }
}
