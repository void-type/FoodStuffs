import type { GetRecipeResponse } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import { ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import { clamp } from '@/models/FormatHelper';
import useAppStore from '@/stores/appStore';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';

// Image state and actions for the recipe being edited.
export default function useRecipeImages(getRecipeId: () => number) {
  const api = ApiHelper.client;
  const appStore = useAppStore();
  const messageStore = useMessageStore();
  const recipeStore = useRecipeStore();

  const images = ref<string[]>([]);
  // The image the carousel should show after the next change.
  const suggestedImage = ref<string | null>(null);
  const pinnedImage = ref<string | null>(null);

  function applyImages(recipe: GetRecipeResponse) {
    images.value = recipe.images || [];
    pinnedImage.value = recipe.pinnedImage || null;
  }

  // Use when the recipe itself was loaded or replaced.
  function setSource(recipe: GetRecipeResponse) {
    applyImages(recipe);
    suggestedImage.value = null;
  }

  async function refresh() {
    try {
      const response = await api().recipesGet({ id: getRecipeId() });
      applyImages(response.data);
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  async function onImagesChanged() {
    await Promise.all([refresh(), recipeStore.fetchRecipesList()]);
  }

  async function uploadImage(file: File) {
    try {
      const response = await api().imagesUpload({ recipeId: getRecipeId() }, { file });

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      suggestedImage.value = response.data.id || null;
      await onImagesChanged();
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  function deleteImage(name: string) {
    async function deleteNow() {
      try {
        const response = await api().imagesDelete({ name });

        if (response.data.message) {
          messageStore.setSuccessMessage(response.data.message);
        }

        const suggestedImageIndex = clamp(
          images.value.indexOf(name) - 1,
          0,
          images.value.length - 1,
        );
        suggestedImage.value = images.value[suggestedImageIndex] || null;
        await onImagesChanged();
      } catch (error) {
        messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    }

    const parameters: ModalParameters = {
      title: 'Delete image',
      description: 'Do you really want to delete this image?',
      okAction: deleteNow,
    };

    appStore.showModal(parameters);
  }

  async function pinImage(name: string) {
    try {
      const response = await api().imagesPin({ name });

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      suggestedImage.value = name;
      await onImagesChanged();
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  return { images, suggestedImage, pinnedImage, setSource, uploadImage, deleteImage, pinImage };
}
