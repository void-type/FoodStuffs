<script lang="ts" setup>
import { onMounted, watch, reactive, computed } from 'vue';
import { storeToRefs } from 'pinia';
import { onBeforeRouteLeave, onBeforeRouteUpdate, useRouter } from 'vue-router';
import type { GetRecipeResponse, SaveRecipeRequest } from '@/api/data-contracts';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import SelectSidebar from '@/components/SelectSidebar.vue';
import RecipeImageManager from '@/components/RecipeImageManager.vue';
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

const data = reactive({
  sourceRecipe: new GetRecipeResponseClass() as GetRecipeResponse | null,
  sourceImages: [] as Array<number>,
  isRecipeDirty: false,
  suggestedImageId: -1,
  pinnedImageId: null as number | null,
});

const { isFieldInError } = appStore;
const { listRequest } = storeToRefs(recipeStore);

const isCreateMode = computed(() => data.sourceRecipe?.id || 0 <= 0);

function fetchRecipe(id: number) {
  if (props.id === 0) {
    setSources(props.newRecipeSuggestion || new GetRecipeResponseClass());
    return;
  }

  new Api()
    .recipesDetail(id)
    .then((response) => {
      setSources(response.data);
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
      data.sourceRecipe = null;
    });
}

function fetchRecipesList() {
  new Api()
    .recipesList(listRequest.value)
    .then((response) => recipeStore.setListResponse(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}

function fetchImageIds(id: number) {
  new Api()
    .recipesDetail(id)
    .then((response) => {
      setImageSources(response.data);
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
    });
}

function setSources(getRecipeResponse: GetRecipeResponse) {
  setImageSources(getRecipeResponse);
  data.suggestedImageId = -1;
  data.sourceRecipe = getRecipeResponse;
}

function setImageSources(getRecipeResponse: GetRecipeResponse) {
  const { images, pinnedImageId } = getRecipeResponse;
  data.sourceImages = images || [];
  data.pinnedImageId = pinnedImageId || -1;
}

function onRecipeSave(recipe: SaveRecipeRequest) {
  new Api()
    .recipesCreate(recipe)
    .then((response) => {
      if (props.id === 0) {
        data.isRecipeDirty = false;
        router.push({ name: 'edit', params: { id: response.data.id } }).catch(() => {});
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
  const answer = await $bvModal.msgBoxConfirm('Do you really want to delete this recipe?', {
    title: 'Delete recipe.',
    okTitle: 'Yes',
    cancelTitle: 'No',
  });

  if (answer !== true) {
    return;
  }

  new Api()
    .recipesDelete(id)
    .then((response) => {
      recipeStore.removeFromRecent(props.id);
      setSources(new GetRecipeResponseClass());
      fetchRecipesList();
      router
        .push({ name: 'search' })
        .then(() => {
          if (response.data.message) {
            appStore.setSuccessMessage(response.data.message);
          }
        })
        .catch(() => {});
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
    });
}

function onRecipeDirtyStateChange(value: boolean) {
  data.isRecipeDirty = value;
}

function onImageUpload(file) {
  const request = new SaveImageRequest();
  request.recipeId = this.id;
  request.file = file;

  webApi.images.upload(
    request,
    (data) => {
      this.setSuccessMessage(data.message);
      this.suggestedImageId = data.id;
      this.fetchImageIds(this.id);
      this.fetchRecipesList();
    },
    (response) => this.setApiFailureMessages(response)
  );
}

async function onImageDelete(imageId) {
  const answer = await this.$bvModal.msgBoxConfirm('Do you really want to delete this image?', {
    title: 'Delete image.',
    okTitle: 'Yes',
    cancelTitle: 'No',
  });

  if (answer !== true) {
    return;
  }

  webApi.images.delete(
    imageId,
    (data) => {
      this.setSuccessMessage(data.message);
      this.fetchImageIds(this.id);
      this.fetchRecipesList();
    },
    (response) => this.setApiFailureMessages(response)
  );
}

function onImagePin(imageId) {
  const request = new PinImageRequest();
  request.id = imageId;

  webApi.images.pin(
    request,
    (data) => {
      this.setSuccessMessage(data.message);
      this.suggestedImageId = imageId;
      this.fetchImageIds(this.id);
      this.fetchRecipesList();
    },
    (response) => this.setApiFailureMessages(response)
  );
}

async function beforeRouteChange(next) {
  if (this.isRecipeDirty) {
    const answer = await this.$bvModal.msgBoxConfirm('Do you really want to leave?', {
      title: 'You have unsaved changes.',
      okTitle: 'Yes',
      cancelTitle: 'No',
    });

    if (answer !== true) {
      next(false);
      return;
    }
  }

  this.addToRecent(this.sourceRecipe);
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

<!-- <script lang="ts" setup>
import { mapGetters, mapActions } from 'vuex';
import Api from '@/api/Api';
import type { GetRecipeResponse, SaveImageRequest, PinImageRequest } from '@/api/data-contracts';
import SelectSidebar from '@/viewComponents/SelectSidebar.vue';
import RecipeEditor from '@/viewComponents/RecipeEditor.vue';
import RecipeImageManager from '@/viewComponents/RecipeImageManager.vue';

  watch: {
    id() {
      this.fetchRecipe(this.id);
    },
  },
  created() {
    this.fetchRecipe(this.id);
  },
  methods: {
    ...mapActions({
      setSuccessMessage: 'app/setSuccessMessage',
      setApiFailureMessages: 'app/setApiFailureMessages',
      addToRecent: 'recipes/addToRecent',
      removeFromRecent: 'recipes/removeFromRecent',
      updateRecent: 'recipes/updateRecent',
      setListResponse: 'recipes/setListResponse',
    }),
    fetchRecipesList() {
      webApi.recipes.list(
        this.listRequest,
        (data) => this.setListResponse(data),
        (response) => this.setApiFailureMessages(response)
      );
    },
    fetchRecipe(id) {
      if (this.id === 0) {
        this.setSources(this.newRecipeSuggestion || new GetRecipeResponse());
        return;
      }

      webApi.recipes.get(
        id,
        (data) => {
          this.setSources(data);
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    fetchImageIds(id) {
      webApi.recipes.get(
        id,
        (data) => {
          this.setImageSources(data);
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    setSources(getRecipeResponse) {
      this.setImageSources(getRecipeResponse);
      this.suggestedImageId = -1;
      this.sourceRecipe = getRecipeResponse;
    },
    setImageSources(getRecipeResponse) {
      const { images, pinnedImageId } = getRecipeResponse;
      this.sourceImages = images;
      this.pinnedImageId = pinnedImageId;
    },
    onRecipeSave(recipe) {
      webApi.recipes.save(
        recipe,
        (data) => {
          if (this.id === 0) {
            this.isRecipeDirty = false;
            this.$router.push({ name: 'edit', params: { id: data.id } }).catch(() => {});
          } else {
            this.fetchRecipe(this.id);
          }

          this.setSuccessMessage(data.message);
          this.fetchRecipesList();
          this.updateRecent(recipe);
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    async onRecipeDelete(id) {
      const answer = await this.$bvModal.msgBoxConfirm(
        'Do you really want to delete this recipe?',
        {
          title: 'Delete recipe.',
          okTitle: 'Yes',
          cancelTitle: 'No',
        }
      );

      if (answer !== true) {
        return;
      }

      webApi.recipes.delete(
        id,
        (data) => {
          this.removeFromRecent(this.id);
          this.setSources(new GetRecipeResponse());
          this.fetchRecipesList();
          this.$router
            .push({ name: 'search' })
            .then(() => this.setSuccessMessage(data.message))
            .catch(() => {});
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    onRecipeDirtyStateChange(value) {
      this.isRecipeDirty = value;
    },
    onImageUpload(file) {
      const request = new SaveImageRequest();
      request.recipeId = this.id;
      request.file = file;

      webApi.images.upload(
        request,
        (data) => {
          this.setSuccessMessage(data.message);
          this.suggestedImageId = data.id;
          this.fetchImageIds(this.id);
          this.fetchRecipesList();
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    async onImageDelete(imageId) {
      const answer = await this.$bvModal.msgBoxConfirm('Do you really want to delete this image?', {
        title: 'Delete image.',
        okTitle: 'Yes',
        cancelTitle: 'No',
      });

      if (answer !== true) {
        return;
      }

      webApi.images.delete(
        imageId,
        (data) => {
          this.setSuccessMessage(data.message);
          this.fetchImageIds(this.id);
          this.fetchRecipesList();
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    onImagePin(imageId) {
      const request = new PinImageRequest();
      request.id = imageId;

      webApi.images.pin(
        request,
        (data) => {
          this.setSuccessMessage(data.message);
          this.suggestedImageId = imageId;
          this.fetchImageIds(this.id);
          this.fetchRecipesList();
        },
        (response) => this.setApiFailureMessages(response)
      );
    },
    async beforeRouteChange(next) {
      if (this.isRecipeDirty) {
        const answer = await this.$bvModal.msgBoxConfirm('Do you really want to leave?', {
          title: 'You have unsaved changes.',
          okTitle: 'Yes',
          cancelTitle: 'No',
        });

        if (answer !== true) {
          next(false);
          return;
        }
      }

      this.addToRecent(this.sourceRecipe);
      next();
    },
  },
  async beforeRouteUpdate(to, from, next) {
    await this.beforeRouteChange(next);
  },
  async beforeRouteLeave(to, from, next) {
    await this.beforeRouteChange(next);
  },
};
</script> -->

<template>
  <b-container>
    <b-row>
      <b-col md="12" lg="3" class="d-print-none mt-4">
        <SelectSidebar :route-name="'view'" />
      </b-col>
      <b-col class="mt-4">
        <h1>{{ isCreateMode ? 'New' : 'Edit' }} Recipe</h1>
        <RecipeEditor
          class="mt-4"
          :is-field-in-error="isFieldInError"
          :source-recipe="sourceRecipe"
          :on-recipe-save="onRecipeSave"
          :on-recipe-delete="onRecipeDelete"
          :on-recipe-dirty-state-change="onRecipeDirtyStateChange"
          :is-create-mode="isCreateMode"
        />
        <RecipeImageManager
          v-if="!isCreateMode"
          class="mt-4"
          :is-field-in-error="isFieldInError"
          :source-images="sourceImages"
          :suggested-image-id="suggestedImageId"
          :pinned-image-id="pinnedImageId"
          :on-image-upload="onImageUpload"
          :on-image-delete="onImageDelete"
          :on-image-pin="onImagePin"
        />
      </b-col>
    </b-row>
  </b-container>
</template>

<template>
  <div class="container-xxl">
    <div class="row">
      <h1 class="mt-4 mb-0">{{ data.sourceRecipe?.name || 'Loading...' }}</h1>
      <div class="col-md-12 col-lg-9 mt-4">
        <RecipeImageManager
          :is-field-in-error="() => {}"
          :image-ids="[1, 2, 3]"
          :suggested-image-id="2"
          :pinned-image-id="2"
          :on-image-upload="() => {}"
          :on-image-delete="() => {}"
          :on-image-pin="() => {}"
        />
        <RecipeViewer v-if="data.sourceRecipe !== null" class="mt-3" :recipe="data.sourceRecipe" />
      </div>
      <div class="col-md-12 col-lg-3 d-print-none mt-4">
        <SelectSidebar :route-name="'view'" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
