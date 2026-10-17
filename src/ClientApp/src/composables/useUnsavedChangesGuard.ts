import type { MaybeRefOrGetter } from 'vue';
import type { NavigationGuardNext, RouteLocationNormalized, RouteLocationRaw } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import { onBeforeUnmount, onMounted, toValue } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate, useRouter } from 'vue-router';
import useAppStore from '@/stores/appStore';

interface UnsavedChangesGuardOptions {
  // Runs whenever the user is allowed to leave the page.
  onLeave?: () => void;
}

// Prompts before leaving a page (route change or browser unload) when it has unsaved changes.
export default function useUnsavedChangesGuard(
  isDirty: MaybeRefOrGetter<boolean>,
  options: UnsavedChangesGuardOptions = {},
) {
  const appStore = useAppStore();
  const router = useRouter();

  let skipPrompt = false;

  function changeRoute(to: RouteLocationRaw) {
    options.onLeave?.();
    skipPrompt = true;
    router.push(to).then(() => {
      skipPrompt = false;
    });
  }

  function beforeRouteChange(
    to: RouteLocationNormalized,
    from: RouteLocationNormalized,
    next: NavigationGuardNext,
  ) {
    if (skipPrompt) {
      next();
      return;
    }

    if (toValue(isDirty)) {
      const parameters: ModalParameters = {
        title: 'Unsaved changes',
        description: 'You have unsaved changes. Do you really want to leave?',
        okAction: () => changeRoute(to),
      };

      next(false);
      appStore.showModal(parameters);
      return;
    }

    changeRoute(to);
    next();
  }

  onBeforeRouteUpdate(beforeRouteChange);
  onBeforeRouteLeave(beforeRouteChange);

  function handleBeforeUnload(event: BeforeUnloadEvent) {
    if (toValue(isDirty)) {
      event.preventDefault();
      // Required for Chrome to show the confirmation dialog
      event.returnValue = '';
    }
  }

  onMounted(() => {
    window.addEventListener('beforeunload', handleBeforeUnload);
  });

  onBeforeUnmount(() => {
    window.removeEventListener('beforeunload', handleBeforeUnload);
  });

  // For navigation the page starts itself, such as redirecting to the edit page after creating.
  async function navigateWithoutPrompt(to: RouteLocationRaw) {
    skipPrompt = true;

    try {
      await router.push(to);
    } finally {
      skipPrompt = false;
    }
  }

  return { navigateWithoutPrompt };
}
