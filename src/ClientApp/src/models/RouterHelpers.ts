import type { RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';

export default class RouterHelpers {
  static setTitle(route: RouteLocationNormalizedLoaded) {
    const appStore = useAppStore();
    document.title = `${route.meta.title} | ${appStore.applicationName}`;
  }
}
