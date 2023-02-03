<script lang="ts" setup>
import { onMounted, watch, reactive, computed } from 'vue';
import { storeToRefs } from 'pinia';
import {
  onBeforeRouteLeave,
  onBeforeRouteUpdate,
  useRouter,
  type NavigationGuardNext,
} from 'vue-router';
import type { GetRecipeResponse, SaveRecipeRequest } from '@/api/data-contracts';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import SelectSidebar from '@/components/SelectSidebar.vue';
import RecipeImageManager from '@/components/RecipeImageManager.vue';
import RecipeEditor from '@/components/RecipeEditor.vue';
import GetRecipeResponseClass from '@/models/GetRecipeResponseClass';
import type { ModalParameters } from '@/models/ModalParameters';

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
  sourceRecipe: { ...new GetRecipeResponseClass(), name: 'Loading...' } as GetRecipeResponse,
  sourceImages: [] as Array<number>,
  isRecipeDirty: false,
  suggestedImageId: -1,
  pinnedImageId: null as number | null,
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const router = useRouter();
const webApi = new Api();

const { isFieldInError } = appStore;
const { listRequest } = storeToRefs(recipeStore);

const isCopyMode = computed(() => (props.copy || 0) > 0);
const isNewMode = computed(() => !isCopyMode.value && (props.id || 0) <= 0);
const isCreateMode = computed(() => isCopyMode.value || isNewMode.value);

function setImageSources(getRecipeResponse: GetRecipeResponse) {
  const { images, pinnedImageId } = getRecipeResponse;
  data.sourceImages = images || [];
  data.pinnedImageId = pinnedImageId || -1;
}

function setSources(getRecipeResponse: GetRecipeResponse) {
  if (isCopyMode.value === true) {
    data.sourceRecipe = {
      ...getRecipeResponse,
      id: 0,
      name: `${getRecipeResponse.name} Copy`,
      pinnedImageId: null,
      images: [],
    };
  } else {
    data.sourceRecipe = getRecipeResponse;
  }

  setImageSources(data.sourceRecipe);
  data.suggestedImageId = -1;
}

function fetchRecipe() {
  if (isNewMode.value) {
    setSources({ ...new GetRecipeResponseClass() });
    return;
  }

  const id = isCopyMode.value ? props.copy : props.id;

  webApi
    .recipesDetail(id)
    .then((response) => {
      setSources(response.data);
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
      data.sourceRecipe = new GetRecipeResponseClass();
    });
}

function fetchRecipesList() {
  webApi
    .recipesList(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}

function fetchImageIds(id: number) {
  webApi
    .recipesDetail(id)
    .then((response) => {
      setImageSources(response.data);
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
    });
}

function onRecipeSave(recipe: SaveRecipeRequest) {
  webApi
    .recipesCreate(recipe)
    .then((response) => {
      if (props.id === 0) {
        data.isRecipeDirty = false;
        router.push({ name: 'edit', params: { id: response.data.id } });
      } else {
        fetchRecipe();
      }

      if (response.data.message) {
        appStore.setSuccessMessage(response.data.message);
      }
      fetchRecipesList();
      recipeStore.updateRecent(recipe);
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
    });
}

function onRecipeDelete(id: number) {
  function deleteRecipe() {
    webApi
      .recipesDelete(id)
      .then((response) => {
        recipeStore.removeFromRecent(props.id);
        setSources(new GetRecipeResponseClass());
        fetchRecipesList();
        router.push({ name: 'search', query: recipeStore.currentQueryParams }).then(() => {
          if (response.data.message) {
            appStore.setSuccessMessage(response.data.message);
          }
        });
      })
      .catch((response) => {
        appStore.setApiFailureMessages(response);
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
  webApi
    .imagesCreate({ recipeId: props.id }, { file })
    .then((response) => {
      if (response.data.message) {
        appStore.setSuccessMessage(response.data.message);
      }

      data.suggestedImageId = response.data.id || -1;

      fetchImageIds(props.id);
      fetchRecipesList();
    })
    .catch((response) => appStore.setApiFailureMessages(response));
}

function onImageDelete(imageId: number) {
  function deleteImage() {
    webApi
      .imagesDelete(imageId)
      .then((response) => {
        if (response.data.message) {
          appStore.setSuccessMessage(response.data.message);
        }
        fetchImageIds(props.id);
        fetchRecipesList();
      })
      .catch((response) => {
        appStore.setApiFailureMessages(response);
      });
  }

  const parameters: ModalParameters = {
    title: 'Delete image',
    description: 'Do you really want to delete this image?',
    okAction: deleteImage,
  };

  appStore.showModal(parameters);
}

function onImagePin(imageId: number) {
  webApi
    .imagesPinCreate({ id: imageId })
    .then((response) => {
      if (response.data.message) {
        appStore.setSuccessMessage(response.data.message);
      }
      data.suggestedImageId = imageId;
      fetchImageIds(props.id);
      fetchRecipesList();
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
    });
}

function beforeRouteChange(next: NavigationGuardNext) {
  function changeRoute() {
    recipeStore.addToRecent(data.sourceRecipe);
    next();
  }

  function cancelRouteChange() {
    next(false);
  }

  if (data.isRecipeDirty) {
    const parameters: ModalParameters = {
      title: 'Unsaved changes',
      description: 'You have unsaved changes. Do you really want to leave?',
      okAction: changeRoute,
      cancelAction: cancelRouteChange,
    };

    appStore.showModal(parameters);
  } else {
    changeRoute();
  }
}

watch(
  () => [props.id],
  () => {
    fetchRecipe();
  }
);

onMounted(() => {
  fetchRecipe();
});

onBeforeRouteUpdate(async (to, from, next) => {
  await beforeRouteChange(next);
});

onBeforeRouteLeave(async (to, from, next) => {
  await beforeRouteChange(next);
});
</script>

<template>
  <div class="container-xxl">
    <div class="row">
      <h1 class="mt-4 mb-0">{{ isNewMode ? 'New recipe' : data.sourceRecipe.name }}</h1>
      <div class="col-md-12 col-lg-9 mt-4">
        <RecipeEditor
          :is-field-in-error="isFieldInError"
          :source-recipe="data.sourceRecipe"
          :on-recipe-save="onRecipeSave"
          :on-recipe-delete="onRecipeDelete"
          :on-recipe-dirty-state-change="onRecipeDirtyStateChange"
          :is-create-mode="isCreateMode"
        />
        <RecipeImageManager
          v-if="!isCreateMode"
          class="mt-4"
          :is-field-in-error="isFieldInError"
          :source-images="data.sourceImages"
          :suggested-image-id="data.suggestedImageId"
          :pinned-image-id="data.pinnedImageId"
          :on-image-upload="onImageUpload"
          :on-image-delete="onImageDelete"
          :on-image-pin="onImagePin"
        />
      </div>
      <div class="col-md-12 col-lg-3 d-print-none mt-4">
        <SelectSidebar :route-name="'edit'" />
      </div>
    </div>
  </div>
  <AppModal ref="recipe-edit-modal" />
</template>

<style lang="scss" scoped></style>
