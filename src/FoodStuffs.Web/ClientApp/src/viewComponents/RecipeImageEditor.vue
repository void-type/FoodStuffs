<template>
  <form
    id="recipe-images-form"
    name="recipe-images-form"
  >
    <b-form-row>
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
              :disabled="!(sourceImages.length > 0)"
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
            v-if="sourceImages.length > 0"
            id="image-carousel"
            v-model="carouselIndex"
            :interval="0"
            no-animation
            controls
            indicators
            class="mt-2 mb-2"
          >
            <b-carousel-slide
              v-for="image in sourceImages"
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
  </form>
</template>

<script>
import { mapActions } from 'vuex';
import { DeleteImageRequest } from '../models/imagesApiModels';
import webApi from '../webApi';

export default {
  props: {
    isFieldInError: {
      type: Function,
      required: true,
    },
    sourceImages: {
      type: Array,
      required: false,
      default: () => [],
    },
    onImageUpload: {
      type: Function,
      required: true,
    },
    onImageDelete: {
      type: Function,
      required: true,
    },
  },
  data() {
    return {
      uploadFile: null,
      carouselIndex: 0,
    };
  },
  watch: {
    sourceImages() {
      this.carouselIndex = Math.min(this.carouselIndex, this.sourceImages.length - 1);
    },
  },
  methods: {
    ...mapActions({
      setValidationErrorMessages: 'app/setValidationErrorMessages',
    }),
    imageUrl(id) {
      return webApi.images.url(id);
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

      this.onImageUpload(this.uploadFile);
    },
    deleteImageClick() {
      const imageId = this.sourceImages[this.carouselIndex];

      const request = new DeleteImageRequest();
      request.id = imageId;

      this.onImageDelete(request);
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
