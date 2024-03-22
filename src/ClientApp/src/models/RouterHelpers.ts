import type { RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';
import { isNil } from './FormatHelpers';

interface RouteRecipe {
  id?: number;
  slug?: string | null;
}

export default class RouterHelpers {
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
    return { name: 'view', params: { id: recipe.id, slug: recipe.slug } };
  }

  static editRecipe(recipe: RouteRecipe) {
    return { name: 'edit', params: { id: recipe.id, slug: recipe.slug } };
  }
}
