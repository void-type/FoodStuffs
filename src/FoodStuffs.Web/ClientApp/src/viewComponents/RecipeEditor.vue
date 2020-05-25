<template>
  <form @keydown.ctrl.enter.prevent="saveClick(workingRecipe)">
    <h1>{{ isEditingMode ? 'Edit' : 'New' }} Recipe</h1>
    <b-form-row
      v-if="isEditingMode"
    >
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group
          label="Images"
          label-for="upload"
        >
          <b-form-file
            id="upload"
            v-model="uploadFile"
            :state="isFieldInError('upload') ? false : null"
            name="upload"
            class="text-nowrap text-truncate"
            placeholder="Drop file or click to browse..."
            drop-placeholder="Drop file here..."
          />
        </b-form-group>
        <b-form-group>
          <b-button-toolbar>
            <b-button
              id="uploadImage"
              variant="primary"
              name="uploadImage"
              :disabled="uploadFile === null"
              @click.stop.prevent="uploadImageClick()"
            >
              Upload
            </b-button>
            <b-button
              id="deleteImage"
              class="ml-auto"
              variant="danger"
              name="deleteImage"
              :disabled="workingRecipe.images.length < 1"
              @click.stop.prevent="deleteImageClick()"
            >
              Delete
            </b-button>
          </b-button-toolbar>
        </b-form-group>
      </b-col>
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group>
          <b-carousel
            v-if="workingRecipe.images.length > 0"
            id="image-carousel"
            v-model="carouselIndex"
            :interval="0"
            no-animation
            controls
            indicators
            class="mt-2 mb-2"
          >
            <b-carousel-slide
              v-for="image in workingRecipe.images"
              :key="image"
              :img-src="imageUrl(image)"
            />
          </b-carousel>
          <b-card
            v-else
            class="text-center p-5"
          >
            No images.
          </b-card>
        </b-form-group>
      </b-col>
    </b-form-row>
    <b-form-row>
      <b-col
        md="12"
        sm="6"
      />
      <b-col
        md="12"
      >
        <b-form-group
          label="Name"
          label-for="name"
        >
          <b-form-input
            id="name"
            v-model="workingRecipe.name"
            :class="{'is-invalid': isFieldInError('name')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <b-form-group
          label="Ingredients"
          label-for="ingredients"
        >
          <b-form-textarea
            id="ingredients"
            v-model="workingRecipe.ingredients"
            rows="1"
            :max-rows="Number.MAX_SAFE_INTEGER"
            :class="{'is-invalid': isFieldInError('ingredients')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <b-form-group
          label="Directions"
          label-for="directions"
        >
          <b-form-textarea
            id="directions"
            v-model="workingRecipe.directions"
            rows="1"
            :max-rows="Number.MAX_SAFE_INTEGER"
            :class="{'is-invalid': isFieldInError('directions')}"
          />
        </b-form-group>
      </b-col>
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group
          label="Prep Time Minutes"
          label-for="prepTimeMinutes"
        >
          <b-form-input
            id="prepTimeMinutes"
            v-model="workingRecipe.prepTimeMinutes"
            :class="{'is-invalid': isFieldInError('prepTimeMinutes')}"
            type="number"
          />
        </b-form-group>
      </b-col>
      <b-col
        sm="12"
        md="6"
      >
        <b-form-group
          label="Cook Time Minutes"
          label-for="cookTimeMinutes"
        >
          <b-form-input
            id="cookTimeMinutes"
            v-model="workingRecipe.cookTimeMinutes"
            :class="{'is-invalid': isFieldInError('cookTimeMinutes')}"
            type="number"
          />
        </b-form-group>
      </b-col>
      <b-col
        md="12"
      >
        <TagEditor
          :class="{'form-group': true, danger: isFieldInError('categories')}"
          :tags="workingRecipe.categories"
          :on-add-tag="addCategory"
          :on-remove-tag="removeCategory"
          field-name="categories"
          label="Categories"
        />
      </b-col>
    </b-form-row>
    <EntityDetailsAuditInfo
      v-if="sourceRecipe.id"
      class="mb-3"
      :entity="sourceRecipe"
    />
    <b-form-row>
      <b-col
        md="12"
      >
        <b-button-toolbar>
          <b-button
            class="mr-2"
            variant="primary"
            @click.stop.prevent="saveClick(workingRecipe)"
          >
            Save
          </b-button>
          <b-button
            v-if="isEditingMode"
            :to="{name: 'new', params: {newRecipeSuggestion: getRecipeCopy(workingRecipe)}}"
            class="mr-2"
          >
            Copy
          </b-button>
          <b-button
            v-if="isEditingMode"
            :to="{name: 'view', params: {id: sourceRecipe.id}}"
          >
            Cancel
          </b-button>
          <b-button
            v-if="isEditingMode"
            class="ml-auto"
            variant="danger"
            @click.prevent="onDelete(workingRecipe.id)"
          >
            Delete
          </b-button>
        </b-button-toolbar>
      </b-col>
    </b-form-row>
  </form>
</template>

<script>
import { mapActions } from 'vuex';
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';
import TagEditor from './TagEditor.vue';
import { SaveRecipeRequest } from '../models/recipesApiModels';
import { SaveImageRequest, DeleteImageRequest } from '../models/imagesApiModels';
import trimAndTitleCase from '../util/trimAndTitleCase';
import webApi from '../webApi';

export default {
  components: {
    EntityDetailsAuditInfo,
    TagEditor,
  },
  props: {
    sourceRecipe: {
      type: Object,
      required: true,
    },
    isFieldInError: {
      type: Function,
      required: true,
    },
    onSave: {
      type: Function,
      required: true,
    },
    onDelete: {
      type: Function,
      required: true,
    },
    onUploadImage: {
      type: Function,
      required: true,
    },
    onDeleteImage: {
      type: Function,
      required: true,
    },
  },
  data() {
    return {
      workingRecipe: new SaveRecipeRequest(),
      uploadFile: null,
      carouselIndex: 0,
    };
  },
  computed: {
    isEditingMode() {
      return this.workingRecipe.id > 0;
    },
  },
  watch: {
    sourceRecipe() {
      this.reset();
    },
  },
  created() {
    this.reset();
  },
  methods: {
    ...mapActions({
      setValidationErrorMessages: 'app/setValidationErrorMessages',
    }),
    imageUrl(id) {
      return webApi.images.url(id);
    },
    reset() {
      Object.assign(this.workingRecipe, this.sourceRecipe);
      if (this.carouselIndex > this.workingRecipe.images.length - 1) {
        this.carouselIndex = this.workingRecipe.images.length - 1;
      }
    },
    addCategory(tag) {
      const categoryName = trimAndTitleCase(tag);

      const categoryDoesNotExist = this.workingRecipe.categories
        .map(value => value.toUpperCase())
        .indexOf(categoryName.toUpperCase()) < 0;

      if (categoryDoesNotExist && categoryName.length > 0) {
        this.workingRecipe.categories.push(categoryName);
      }
    },
    removeCategory(categoryName) {
      const categoryIndex = this.workingRecipe.categories.indexOf(categoryName);

      if (categoryIndex > -1) {
        this.workingRecipe.categories.splice(categoryIndex, 1);
      }
    },
    saveClick(workingRecipe) {
      const sendableRecipe = new SaveRecipeRequest();

      Object.keys(sendableRecipe).forEach((key) => {
        sendableRecipe[key] = workingRecipe[key];
      });

      this.onSave(sendableRecipe);
    },
    getRecipeCopy(workingRecipe) {
      return Object.assign({}, workingRecipe, { images: [] });
    },
    uploadImageClick() {
      if (this.uploadFile === null) {
        return;
      }

      const fileSizeLimit = 30000000;

      function toMiB(bytes) {
        const mb = bytes / (1024 * 1024);
        return Math.round(mb * 100) / 100;
      }

      if (this.uploadFile.size > fileSizeLimit) {
        this.setValidationErrorMessages({
          errorMessages: [`Your file (${toMiB(this.uploadFile.size)} MB) exceeds the limit (${toMiB(fileSizeLimit)} MB).`],
          fieldNames: ['upload'],
        });

        return;
      }

      const request = new SaveImageRequest();
      request.recipeId = this.sourceRecipe.id;
      request.file = this.uploadFile;

      this.onUploadImage(request);
    },
    deleteImageClick() {
      const imageId = this.sourceRecipe.images[this.carouselIndex];

      const request = new DeleteImageRequest();
      request.id = imageId;

      this.onDeleteImage(request);
    },
  },
};
</script>

<style lang="scss" scoped>
textarea {
  overflow: hidden !important;
  resize: none;
}
</style>
