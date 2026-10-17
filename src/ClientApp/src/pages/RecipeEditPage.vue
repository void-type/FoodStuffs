<script lang="ts" setup>
import type {
  EntityResponseOfGetRecipeResponse,
  GetRecipeResponse,
  IItemSetOfIFailure,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import type RecipeWorking from '@/models/RecipeWorking';
import { computed, reactive, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import RecipeEditor from '@/components/RecipeEditor.vue';
import RecipeImageManager from '@/components/RecipeImageManager.vue';
import useRecipeImages from '@/composables/useRecipeImages';
import useUnsavedChangesGuard from '@/composables/useUnsavedChangesGuard';
import ApiHelper from '@/models/ApiHelper';
import RecipeGetResponse from '@/models/RecipeGetResponse';
import RouterHelper from '@/models/RouterHelper';
import useAppStore from '@/stores/appStore';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';

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
  isRecipeDirty: false,
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const router = useRouter();
const route = useRoute();
const api = ApiHelper.client;

const {
  images: sourceImages,
  suggestedImage,
  pinnedImage,
  setSource: setImageSource,
  uploadImage,
  deleteImage,
  pinImage,
} = useRecipeImages(() => props.id);

useUnsavedChangesGuard(() => data.isRecipeDirty, {
  onLeave: () => recipeStore.addToRecent(data.sourceRecipe),
});

// Editing existing
const isEditMode = computed(() => (props.id || 0) > 0);
// Create copy of existing
const isCreateCopyMode = computed(() => !isEditMode.value && (props.copy || 0) > 0);
// Create new
const isCreateNewMode = computed(() => !isEditMode.value && !isCreateCopyMode.value);

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
  setImageSource(data.sourceRecipe);
}

async function fetchRecipe() {
  if (isCreateNewMode.value) {
    const newRecipe = new RecipeGetResponse();
    newRecipe.isForMealPlanning = true;
    setSources(newRecipe);
    return;
  }

  const id = isCreateCopyMode.value ? props.copy : props.id;

  try {
    const response = await api().recipesGet({ id });
    setSources(response.data);
    if (isEditMode.value) {
      recipeStore.queueRecent(response.data);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    data.sourceRecipe = new RecipeGetResponse();
  }
}

async function onRecipeSave(recipe: RecipeWorking) {
  async function onPostSave(
    response: HttpResponse<EntityResponseOfGetRecipeResponse, IItemSetOfIFailure>,
  ) {
    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }

    await recipeStore.fetchRecipesList();

    if (response.data.entity) {
      recipeStore.updateRecent(response.data.entity);
    }
  }

  try {
    const response = await api().recipesSave({
      ...recipe,
      categories: recipe.categories.map(x => x.name || ''),
    });
    data.isRecipeDirty = false;

    if (!isEditMode.value) {
      await router.push({ name: 'recipeEdit', params: { id: response.data.entity?.id } });
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
      .recipesDelete({ id })
      .then(async (response) => {
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

watch(
  () => props.id,
  () => {
    fetchRecipe();
  },
  { immediate: true },
);
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
    >
      <template #images>
        <RecipeImageManager
          v-if="isEditMode"
          :recipe-id="props.id"
          :images="sourceImages"
          :suggested-image="suggestedImage"
          :pinned-image="pinnedImage"
          :on-image-upload="uploadImage"
          :on-image-delete="deleteImage"
          :on-image-pin="pinImage"
        />
        <div v-else class="alert alert-secondary mb-0">
          <strong>Note:</strong> You can upload images after saving.
        </div>
      </template>
    </RecipeEditor>
  </div>
</template>

<style lang="scss" scoped></style>
