import type { RouteLocationNormalized, RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';

export default class RouterHelpers {
  static setTitle(route: RouteLocationNormalizedLoaded) {
    const appStore = useAppStore();
    document.title = `${route.meta.title} | ${appStore.applicationName}`;
  }

  static newRecipeProps(route: RouteLocationNormalized) {
    const oldRecipe = route.params;
    if (oldRecipe === undefined || oldRecipe === null) {
      return oldRecipe;
    }

    const newRecipe = { ...oldRecipe, id: 0, name: `${oldRecipe.name} Copy` };

    return { newRecipeSuggestion: newRecipe };
  }
}
