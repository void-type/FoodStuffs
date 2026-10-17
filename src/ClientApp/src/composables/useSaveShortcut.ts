import { useEventListener } from '@vueuse/core';
import { storeToRefs } from 'pinia';
import useAppStore from '@/stores/appStore';

// Saves on Ctrl+S (Cmd+S on Mac) from anywhere on the page, including while typing in a field.
export default function useSaveShortcut(save: () => unknown) {
  const { modalIsActive } = storeToRefs(useAppStore());

  let isSaving = false;

  useEventListener(window, 'keydown', async (event: KeyboardEvent) => {
    const isSaveKey
      = (event.ctrlKey || event.metaKey)
        && !event.shiftKey
        && !event.altKey
        && event.key.toLowerCase() === 's';

    if (!isSaveKey) {
      return;
    }

    // Always suppress the browser's "Save page" dialog.
    event.preventDefault();

    // Don't save from under an open dialog, on key repeat, or while a save is already running.
    if (event.repeat || isSaving || modalIsActive.value) {
      return;
    }

    isSaving = true;

    try {
      await save();
    } finally {
      isSaving = false;
    }
  });
}
