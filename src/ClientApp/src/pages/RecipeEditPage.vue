<script lang="ts" setup>
import { watch, reactive, computed, onMounted, onBeforeUnmount } from 'vue';
import {
  onBeforeRouteLeave,
  onBeforeRouteUpdate,
  useRoute,
  useRouter,
  type NavigationGuardNext,
  type RouteLocationNormalized,
} from 'vue-router';
import type {
  GetRecipeResponse,
  IItemSetOfIFailure,
  EntityMessageOfInteger,
  SaveRecipeRequest,
} from '@/api/data-contracts';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import useDiscoveryStore from '@/stores/discoveryStore';
import RecipeImageManager from '@/components/RecipeImageManager.vue';
import RecipeEditor from '@/components/RecipeEditor.vue';
import RecipeGetResponse from '@/models/RecipeGetResponse';
import type { ModalParameters } from '@/models/ModalParameters';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import { clamp } from '@/models/FormatHelper';
import RouterHelper from '@/models/RouterHelper';
import useMessageStore from '@/stores/messageStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: 0,
  },
  copy: {
    type: Number,
    required: false,
    default: 0,
  },
});

const data = reactive({
  sourceRecipe: new RecipeGetResponse() as GetRecipeResponse,
  sourceImages: [] as string[],
  isRecipeDirty: false,
  suggestedImage: null as string | null,
  pinnedImage: null as string | null,
  imageUploadSuccessToken: 0,
  imageUploadFailToken: 0,
  recipeChangeToken: 0,
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const discoveryStore = useDiscoveryStore();
const router = useRouter();
const route = useRoute();
const api = ApiHelper.client;

// Editing existing
const isEditMode = computed(() => (props.id || 0) > 0);
// Create copy of existing
const isCreateCopyMode = computed(() => !isEditMode.value && (props.copy || 0) > 0);
// Create new
const isCreateNewMode = computed(() => !isEditMode.value && !isCreateCopyMode.value);

function setImageSources(getRecipeResponse: GetRecipeResponse) {
  const { images, pinnedImage } = getRecipeResponse;
  data.sourceImages = images || [];
  data.pinnedImage = pinnedImage || null;
}

function setSources(getRecipeResponse: GetRecipeResponse) {
  if (isCreateCopyMode.value === true) {
    data.sourceRecipe = {
      ...getRecipeResponse,
      id: 0,
      name: `${getRecipeResponse.name} copy`,
      images: [],
      pinnedImage: null,
    };
  } else {
    data.sourceRecipe = getRecipeResponse;
  }

  RouterHelper.setTitle(route, data.sourceRecipe.name);
  setImageSources(data.sourceRecipe);
  data.suggestedImage = null;
}

async function fetchRecipe() {
  if (isCreateNewMode.value) {
    setSources(new RecipeGetResponse());
    return;
  }

  const id = isCreateCopyMode.value ? props.copy : props.id;

  try {
    const response = await api().recipesGet(id);
    data.recipeChangeToken += 1;
    setSources(response.data);
    if (isEditMode.value) {
      recipeStore.queueRecent(response.data);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    data.sourceRecipe = new RecipeGetResponse();
  }
}

async function fetchImages(recipeId: number) {
  try {
    const response = await api().recipesGet(recipeId);
    setImageSources(response.data);
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function onRecipeSave(recipe: SaveRecipeRequest) {
  async function onPostSave(response: HttpResponse<EntityMessageOfInteger, IItemSetOfIFailure>) {
    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }

    await recipeStore.fetchRecipesList();
    recipeStore.updateRecent(recipe);
  }

  try {
    const response = await api().recipesSave(recipe);
    data.isRecipeDirty = false;

    if (!isEditMode.value) {
      await router.push({ name: 'recipeEdit', params: { id: response.data.id } });
    } else {
      await fetchRecipe();
    }

    await onPostSave(response);
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function onRecipeDelete(id: number) {
  function deleteRecipe() {
    api()
      .recipesDelete(id)
      .then(async (response) => {
        discoveryStore.removeFromList(props.id);
        recipeStore.removeFromRecent(props.id);
        setSources(new RecipeGetResponse());
        await recipeStore.fetchRecipesList();
        router.push({ name: 'recipeList', query: recipeStore.currentQueryParams }).then(() => {
          if (response.data.message) {
            messageStore.setSuccessMessage(response.data.message);
          }
        });
      })
      .catch((response) => {
        messageStore.setApiFailureMessages(response);
      });
  }

  const parameters: ModalParameters = {
    title: 'Delete recipe',
    description: 'Do you really want to delete this recipe?',
    okAction: deleteRecipe,
  };

  appStore.showModal(parameters);
}

function onRecipeDirtyStateChange(value: boolean) {
  data.isRecipeDirty = value;
}

function onImageUpload(file: File) {
  api()
    .imagesUpload({ recipeId: props.id }, { file })
    .then(async (response) => {
      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      data.suggestedImage = response.data.id || null;
      fetchImages(props.id);
      await recipeStore.fetchRecipesList();
      data.imageUploadSuccessToken += 1;
    })
    .catch((response) => {
      data.imageUploadFailToken += 1;
      messageStore.setApiFailureMessages(response);
    });
}

function onImageDelete(name: string) {
  function deleteImage() {
    api()
      .imagesDelete(name)
      .then(async (response) => {
        if (response.data.message) {
          messageStore.setSuccessMessage(response.data.message);
        }

        const suggestedImageIndex = clamp(
          data.sourceImages.indexOf(name) - 1,
          0,
          data.sourceImages.length - 1
        );
        data.suggestedImage = data.sourceImages[suggestedImageIndex];
        fetchImages(props.id);
        await recipeStore.fetchRecipesList();
      })
      .catch((response) => {
        messageStore.setApiFailureMessages(response);
      });
  }

  const parameters: ModalParameters = {
    title: 'Delete image',
    description: 'Do you really want to delete this image?',
    okAction: deleteImage,
  };

  appStore.showModal(parameters);
}

function onImagePin(name: string) {
  api()
    .imagesPin(name)
    .then(async (response) => {
      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
      data.suggestedImage = name;
      fetchImages(props.id);
      await recipeStore.fetchRecipesList();
    })
    .catch((response) => {
      messageStore.setApiFailureMessages(response);
    });
}

watch(
  () => props.id,
  () => {
    fetchRecipe();
  },
  { immediate: true }
);

let forceImmediateRouteChange = false;

function changeRouteFromModal(t: RouteLocationNormalized) {
  recipeStore.addToRecent(data.sourceRecipe);
  forceImmediateRouteChange = true;
  router.push(t).then(() => {
    forceImmediateRouteChange = false;
  });
}

function beforeRouteChange(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  if (forceImmediateRouteChange) {
    next();
    return;
  }

  if (data.isRecipeDirty) {
    const parameters: ModalParameters = {
      title: 'Unsaved changes',
      description: 'You have unsaved changes. Do you really want to leave?',
      okAction: () => changeRouteFromModal(to),
    };

    next(false);
    appStore.showModal(parameters);
    return;
  }

  recipeStore.addToRecent(data.sourceRecipe);
  changeRouteFromModal(to);
  next();
}

onBeforeRouteUpdate(async (to, from, next) => {
  beforeRouteChange(to, from, next);
});

onBeforeRouteLeave(async (to, from, next) => {
  beforeRouteChange(to, from, next);
});

function handleBeforeUnload(event) {
  if (data.isRecipeDirty) {
    event.preventDefault();
    // Required for Chrome to show the confirmation dialog
    // eslint-disable-next-line no-param-reassign
    event.returnValue = '';
    // Required for Firefox to show the confirmation dialog
    return '';
  }

  return null;
}

onMounted(() => {
  window.addEventListener('beforeunload', handleBeforeUnload);
});

onBeforeUnmount(() => {
  window.removeEventListener('beforeunload', handleBeforeUnload);
});
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <RecipeEditor
      :source-recipe="data.sourceRecipe"
      :on-recipe-save="onRecipeSave"
      :on-recipe-delete="onRecipeDelete"
      :on-recipe-dirty-state-change="onRecipeDirtyStateChange"
      :is-edit-mode="isEditMode"
      class="mt-3"
    />
    <RecipeImageManager
      v-if="isEditMode"
      :images="data.sourceImages"
      :suggested-image="data.suggestedImage"
      :pinned-image="data.pinnedImage"
      :on-image-upload="onImageUpload"
      :image-upload-success-token="data.imageUploadSuccessToken"
      :image-upload-fail-token="data.imageUploadFailToken"
      :recipe-changed-token="data.recipeChangeToken"
      :on-image-delete="onImageDelete"
      :on-image-pin="onImagePin"
      class="mt-3"
    />
    <div v-else class="alert alert-secondary mt-3">
      <strong>Note:</strong> You can upload images after saving.
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
