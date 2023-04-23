import type { RouteLocationNormalizedLoaded } from 'vue-router';
import useAppStore from '@/stores/appStore';
import { isNil } from './FormatHelpers';

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
}
