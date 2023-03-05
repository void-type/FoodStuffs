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
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import SidebarRecipeResults from '@/components/SidebarRecipeResults.vue';
import SidebarRecipeRecent from '@/components/SidebarRecipeRecent.vue';
import RecipeImageManager from '@/components/RecipeImageManager.vue';
import RecipeEditor from '@/components/RecipeEditor.vue';
import GetRecipeResponseClass from '@/models/GetRecipeResponseClass';
import type { ModalParameters } from '@/models/ModalParameters';
import ApiHelpers from '@/models/ApiHelpers';

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
  sourceRecipe: new GetRecipeResponseClass() as GetRecipeResponse,
  sourceImages: [] as number[],
  isRecipeDirty: false,
  suggestedImageId: -1,
  pinnedImageId: null as number | null,
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const router = useRouter();
const api = ApiHelpers.client;

const { isFieldInError } = appStore;
const { listRequest } = storeToRefs(recipeStore);

const isEditMode = computed(() => (props.id || 0) > 0);
const isCreateMode = computed(() => !isEditMode.value);
const isCreateCopyMode = computed(() => isCreateMode.value && (props.copy || 0) > 0);
const isCreateNewMode = computed(() => isCreateMode.value && !isCreateCopyMode.value);

function setImageSources(getRecipeResponse: GetRecipeResponse) {
  const { images, pinnedImageId } = getRecipeResponse;
  data.sourceImages = images || [];
  data.pinnedImageId = pinnedImageId || -1;
}

function setSources(getRecipeResponse: GetRecipeResponse) {
  if (isCreateCopyMode.value === true) {
    data.sourceRecipe = {
      ...getRecipeResponse,
      id: 0,
      name: `${getRecipeResponse.name} copy`,
      images: [],
      pinnedImageId: null,
    };
  } else {
    data.sourceRecipe = getRecipeResponse;
  }
  setImageSources(data.sourceRecipe);
  data.suggestedImageId = -1;
}

function fetchRecipe() {
  if (isCreateNewMode.value) {
    setSources(new GetRecipeResponseClass());
    return;
  }

  const id = isCreateCopyMode.value ? props.copy : props.id;

  api()
    .recipesDetail(id)
    .then((response) => {
      setSources(response.data);
      if (isEditMode.value) {
        recipeStore.queueRecent(response.data);
      }
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
      data.sourceRecipe = new GetRecipeResponseClass();
    });
}

function fetchRecipesList() {
  api()
    .recipesList(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}

function fetchImageIds(recipeId: number) {
  api()
    .recipesDetail(recipeId)
    .then((response) => {
      setImageSources(response.data);
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
    });
}

function onRecipeSave(recipe: SaveRecipeRequest) {
  api()
    .recipesCreate(recipe)
    .then((response) => {
      data.isRecipeDirty = false;

      if (props.id === 0) {
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
    api()
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
  api()
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
    api()
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
  api()
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
    <h1 class="mt-4 mb-4">{{ isCreateNewMode ? 'New recipe' : data.sourceRecipe.name }}</h1>
    <div class="grid">
      <div class="g-col-12 g-col-lg-9">
        <RecipeImageManager
          v-if="isEditMode"
          :image-ids="data.sourceImages"
          :is-field-in-error="isFieldInError"
          :suggested-image-id="data.suggestedImageId"
          :pinned-image-id="data.pinnedImageId"
          :on-image-upload="onImageUpload"
          :on-image-delete="onImageDelete"
          :on-image-pin="onImagePin"
          class="mb-4"
        />
        <RecipeEditor
          :is-field-in-error="isFieldInError"
          :source-recipe="data.sourceRecipe"
          :on-recipe-save="onRecipeSave"
          :on-recipe-delete="onRecipeDelete"
          :on-recipe-dirty-state-change="onRecipeDirtyStateChange"
          :is-edit-mode="isEditMode"
        />
      </div>
      <div class="g-col-12 g-col-lg-3 d-print-none">
        <SidebarRecipeRecent :route-name="'edit'" class="mb-3" />
        <SidebarRecipeResults :route-name="'edit'" />
      </div>
    </div>
  </div>
  <AppModal ref="recipe-edit-modal" />
</template>

<style lang="scss" scoped></style>
