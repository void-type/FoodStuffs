import type { RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';

export default function setTitle(route: RouteLocationNormalizedLoaded) {
  const appStore = useAppStore();
  document.title = `${route.meta.title} | ${appStore.applicationName}`;
}
