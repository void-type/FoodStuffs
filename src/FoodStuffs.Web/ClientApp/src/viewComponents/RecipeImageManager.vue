<template>
  <form>
    <b-form-row>
      <b-col
        cols="12"
      >
        <b-form-group
          label="Images"
          label-for="image-editor"
        >
          <b-card
            id="image-editor"
          >
            <b-form-row>
              <b-col
                sm="12"
                md="6"
              >
                <b-form-group
                  label="Upload"
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
                  </b-button-toolbar>
                </b-form-group>
              </b-col>
              <b-col
                sm="12"
                md="6"
              >
                <b-carousel
                  v-if="sourceImages.length > 0"
                  id="image-carousel"
                  v-model="carouselIndex"
                  :interval="0"
                  no-animation
                  controls
                  indicators
                >
                  <b-carousel-slide
                    v-for="imageId in sourceImages"
                    :key="imageId"
                  >
                    <template v-slot:img>
                      <b-button
                        id="deleteImage"
                        class="imageDeleteButton"
                        size="sm"
                        variant="danger"
                        name="deleteImage"
                        :v-if="sourceImages.length > 0"
                        @click.stop.prevent="deleteImageClick(imageId)"
                      >
                        ✖
                      </b-button>
                      <b-img
                        fluid
                        rounded
                        :src="imageUrl(imageId)"
                      />
                    </template>
                  </b-carousel-slide>
                </b-carousel>
                <b-card
                  v-else
                  class="text-center p-5"
                >
                  No images.
                </b-card>
              </b-col>
            </b-form-row>
          </b-card>
        </b-form-group>
      </b-col>
    </b-form-row>
  </form>
</template>

<script>
import { mapActions } from 'vuex';
import DeleteImageRequest from '../models/api/images/DeleteImageRequest';
import { clamp } from '../models/formatters';
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
    suggestedImageId: {
      type: Number,
      required: false,
      default: -1,
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
    sourceImages(sourceImages) {
      const suggestedImageIndex = sourceImages.indexOf(this.suggestedImageId);
      const newIndex = suggestedImageIndex > -1 ? suggestedImageIndex : this.carouselIndex;
      this.$nextTick(() => { this.carouselIndex = clamp(newIndex, 0, sourceImages.length - 1); });
    },
  },
  methods: {
    ...mapActions({
      setValidationErrorMessages: 'app/setValidationErrorMessages',
    }),
    imageUrl(imageId) {
      return webApi.images.url(imageId);
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
    deleteImageClick(imageId) {
      const request = new DeleteImageRequest();
      request.id = imageId;

      this.onImageDelete(request);
    },
  },
};
</script>

<style lang="scss" scoped>
.imageDeleteButton {
  position: absolute;
  top: 0;
  right: 0;
  min-width: 0;
  z-index: 999;
}
</style>
