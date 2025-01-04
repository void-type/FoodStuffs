import type { RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';
import { isNil } from './FormatHelper';

interface RouteRecipe {
  id?: number;
  slug?: string | null;
}

export default class RouterHelper {
  static setTitle(
    route: RouteLocationNormalizedLoaded,
    additionalTitle: string | null | undefined = null
  ) {
    const appStore = useAppStore();
    const title = [additionalTitle, `${route.meta.title}`, appStore.applicationName]
      .filter((x) => !isNil(x))
      .join(' | ');

    document.title = title;
  }

  static viewRecipe(recipe: RouteRecipe) {
    return { name: 'recipeView', params: { id: recipe.id, slug: recipe.slug } };
  }

  static editRecipe(recipe: RouteRecipe) {
    return { name: 'recipeEdit', params: { id: recipe.id, slug: recipe.slug } };
  }
}
