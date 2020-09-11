<template>
  <b-container>
    <b-row>
      <b-col
        md="12"
        lg="3"
        class="no-print mt-4"
      >
        <SelectSidebar :route-name="'view'" />
      </b-col>
      <b-col
        class="mt-4"
      >
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

<script>
import { mapGetters, mapActions } from 'vuex';
import webApi from '../webApi';
import router from '../router';
import GetRecipeResponse from '../models/api/recipes/GetRecipeResponse';
import SaveImageRequest from '../models/api/images/SaveImageRequest';
import PinImageRequest from '../models/api/recipes/PinImageRequest';
import SelectSidebar from '../viewComponents/SelectSidebar.vue';
import RecipeEditor from '../viewComponents/RecipeEditor.vue';
import RecipeImageManager from '../viewComponents/RecipeImageManager.vue';

export default {
  components: {
    SelectSidebar,
    RecipeEditor,
    RecipeImageManager,
  },
  props: {
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
  },
  data() {
    return {
      sourceRecipe: new GetRecipeResponse(),
      sourceImages: new GetRecipeResponse().images,
      isRecipeDirty: false,
      suggestedImageId: -1,
      pinnedImageId: null,
    };
  },
  computed: {
    ...mapGetters({
      isFieldInError: 'app/isFieldInError',
      listRequest: 'recipes/listRequest',
    }),
    isCreateMode() {
      return this.sourceRecipe.id <= 0;
    },
  },
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
      setListResponse: 'recipes/setListResponse',
    }),
    fetchRecipesList() {
      webApi.recipes.list(
        this.listRequest,
        (data) => this.setListResponse(data),
        (response) => this.setApiFailureMessages(response),
      );
    },
    fetchRecipe(id) {
      if (this.id === 0) {
        this.setSources(this.newRecipeSuggestion || new GetRecipeResponse());
        return;
      }

      webApi.recipes.get(
        id,
        (data) => { this.setSources(data); },
        (response) => this.setApiFailureMessages(response),
      );
    },
    fetchImageIds(id) {
      webApi.recipes.get(
        id,
        (data) => {
          this.setImageSources(data);
        },
        (response) => this.setApiFailureMessages(response),
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
            router.push({ name: 'edit', params: { id: data.id } }).catch(() => {});
          } else {
            this.fetchRecipe(this.id);
          }

          this.fetchRecipesList();
          this.setSuccessMessage(data.message);
        },
        (response) => this.setApiFailureMessages(response),
      );
    },
    async onRecipeDelete(id) {
      const answer = await this.$bvModal.msgBoxConfirm(
        'Do you really want to delete this recipe?',
        {
          title: 'Delete recipe.',
          okTitle: 'Yes',
          cancelTitle: 'No',
        },
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
          router.push({ name: 'search' }).catch(() => {});
          this.setSuccessMessage(data.message);
        },
        (response) => this.setApiFailureMessages(response),
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
        },
        (response) => this.setApiFailureMessages(response),
      );
    },
    async onImageDelete(request) {
      const answer = await this.$bvModal.msgBoxConfirm(
        'Do you really want to delete this image?',
        {
          title: 'Delete image.',
          okTitle: 'Yes',
          cancelTitle: 'No',
        },
      );

      if (answer !== true) {
        return;
      }

      webApi.images.delete(
        request,
        (data) => {
          this.setSuccessMessage(data.message);
          this.fetchImageIds(this.id);
        },
        (response) => this.setApiFailureMessages(response),
      );
    },
    onImagePin(imageId) {
      const request = new PinImageRequest();
      request.imageId = imageId;
      request.recipeId = this.id;

      webApi.recipes.pinImage(
        request,
        (data) => {
          this.setSuccessMessage(data.message);
          this.fetchImageIds(this.id);
        },
        (response) => this.setApiFailureMessages(response),
      );
    },
    async beforeRouteChange(next) {
      if (this.isRecipeDirty) {
        const answer = await this.$bvModal.msgBoxConfirm(
          'Do you really want to leave?',
          {
            title: 'You have unsaved changes.',
            okTitle: 'Yes',
            cancelTitle: 'No',
          },
        );

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
</script>

<style lang="scss" scoped>
</style>
