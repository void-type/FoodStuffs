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

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: 0,
  },
  newRecipeSuggestion: {
    type: Object,
    required: false,
    default: null,
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const router = useRouter();
const webApi = new Api();

const data = reactive({
  sourceRecipe: new GetRecipeResponseClass() as GetRecipeResponse,
  sourceImages: [] as Array<number>,
  isRecipeDirty: false,
  suggestedImageId: -1,
  pinnedImageId: null as number | null,
});

const { isFieldInError } = appStore;
const { listRequest } = storeToRefs(recipeStore);

const isCreateMode = computed(() => (data.sourceRecipe?.id || 0) <= 0);

function setImageSources(getRecipeResponse: GetRecipeResponse) {
  const { images, pinnedImageId } = getRecipeResponse;
  data.sourceImages = images || [];
  data.pinnedImageId = pinnedImageId || -1;
}

function setSources(getRecipeResponse: GetRecipeResponse) {
  setImageSources(getRecipeResponse);
  data.suggestedImageId = -1;
  data.sourceRecipe = getRecipeResponse;
}

function fetchRecipe(id: number) {
  if (props.id === 0) {
    setSources(props.newRecipeSuggestion || new GetRecipeResponseClass());
    return;
  }

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
        fetchRecipe(props.id);
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

async function onRecipeDelete(id: number) {
  // TODO: pop a modal
  // const answer = await this.$bvModal.msgBoxConfirm('Do you really want to delete this recipe?', {
  //   title: 'Delete recipe.',
  //   okTitle: 'Yes',
  //   cancelTitle: 'No',
  // });

  // if (answer !== true) {
  //   return;
  // }

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

async function onImageDelete(imageId: number) {
  // TODO: pop a modal
  // const answer = await this.$bvModal.msgBoxConfirm('Do you really want to delete this image?', {
  //   title: 'Delete image.',
  //   okTitle: 'Yes',
  //   cancelTitle: 'No',
  // });

  // if (answer !== true) {
  //   return;
  // }

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

async function beforeRouteChange(next: NavigationGuardNext) {
  // TODO: pop a modal
  // if (data.isRecipeDirty) {
  //   const answer = await this.$bvModal.msgBoxConfirm('Do you really want to leave?', {
  //     title: 'You have unsaved changes.',
  //     okTitle: 'Yes',
  //     cancelTitle: 'No',
  //   });

  //   if (answer !== true) {
  //     next(false);
  //     return;
  //   }
  // }

  recipeStore.addToRecent(data.sourceRecipe);
  next();
}

watch(
  () => props.id,
  (newValue) => {
    fetchRecipe(newValue);
  }
);

onMounted(() => {
  fetchRecipe(props.id);
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
      <h1 class="mt-4 mb-0">{{ data.sourceRecipe.name || 'Loading...' }}</h1>
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
</template>

<style lang="scss" scoped></style>
