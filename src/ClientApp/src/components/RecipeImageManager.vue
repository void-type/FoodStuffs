<script lang="ts" setup>
import { onMounted, ref, watch, type PropType, type Ref } from 'vue';
import { clamp } from '@/models/FormatHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelper from '@/models/ApiHelper';
import type { HTMLInputEvent } from '@/models/HTMLInputEvent';
import type bootstrap from 'bootstrap';
import useMessageStore from '@/stores/messageStore';
import ImagePlaceholder from './ImagePlaceholder.vue';

const props = defineProps({
  images: {
    type: Array as PropType<Array<string>>,
    required: true,
    default: () => [],
  },
  suggestedImage: {
    type: String as PropType<string | null>,
    required: false,
    default: null,
  },
  pinnedImage: {
    type: String as PropType<string | null>,
    required: false,
    default: null,
  },
  onImageUpload: {
    type: Function,
    required: true,
  },
  imageUploadSuccessToken: {
    type: Number,
    required: true,
  },
  imageUploadFailToken: {
    type: Number,
    required: true,
  },
  recipeChangedToken: {
    type: Number,
    required: true,
  },
  onImageDelete: {
    type: Function,
    required: true,
  },
  onImagePin: {
    type: Function,
    required: true,
  },
  isFieldInError: {
    type: Function,
    required: false,
    default: () => {
      /* do nothing */
    },
  },
});

const messageStore = useMessageStore();

const uploadFile: Ref<File | null> = ref(null);
const uploadInProgress = ref(false);
const carouselIndex = ref(0);
const uniqueId = crypto.randomUUID();

function uploadImageClick() {
  if (uploadFile.value === null) {
    return;
  }

  const fileSizeLimit = 30000000;

  function toMiB(bytes: number) {
    const mb = bytes / (1024 * 1024);
    return Math.round(mb * 100) / 100;
  }

  if (uploadFile.value.size > fileSizeLimit) {
    const sizeMiB = toMiB(uploadFile.value.size);
    const limitMiB = toMiB(fileSizeLimit);

    messageStore.setValidationErrorMessages([
      {
        message: `Your file (${sizeMiB} MB) exceeds the limit (${limitMiB} MB).`,
        uiHandle: 'upload',
      },
    ]);

    return;
  }

  uploadInProgress.value = true;

  props.onImageUpload(uploadFile.value);
}

function uploadFileChange(event: Event | DragEvent) {
  const files =
    (event as HTMLInputEvent)?.target?.files ||
    (event as DragEvent)?.dataTransfer?.files ||
    new FileList();

  if (files.length < 1) {
    return;
  }

  // eslint-disable-next-line prefer-destructuring
  uploadFile.value = files[0];
}

function deleteImageClick(name: string) {
  props.onImageDelete(name);
}

function pinImageClick(name: string) {
  props.onImagePin(name);
}

watch([() => props.images, () => props.suggestedImage], () => {
  const suggestedImageIndex =
    props.suggestedImage === null ? -1 : props.images.indexOf(props.suggestedImage);
  const newIndex = suggestedImageIndex > -1 ? suggestedImageIndex : carouselIndex.value;
  carouselIndex.value = clamp(newIndex, 0, props.images.length - 1);
});

watch([() => props.recipeChangedToken], () => {
  carouselIndex.value = 0;
});

watch(
  () => props.imageUploadSuccessToken,
  () => {
    const fileInput = document.getElementById('upload-file') as HTMLInputElement;

    if (fileInput !== null) {
      fileInput.value = '';
    }

    uploadFile.value = null;
    uploadInProgress.value = false;
  }
);

watch(
  () => props.imageUploadFailToken,
  () => {
    uploadInProgress.value = false;
  }
);

onMounted(() => {
  const carouselElement = document.getElementById('image-carousel');

  if (carouselElement !== null) {
    carouselElement.addEventListener('slid.bs.carousel', (event) => {
      const carouselEvent = event as unknown as bootstrap.Carousel.Event;
      carouselIndex.value = carouselEvent.to;
    });
  }
});
</script>

<template>
  <div>
    <label for="upload-file" class="form-label">Upload image</label>
    <div class="grid">
      <div class="g-col-12 g-col-md-6">
        <input
          id="upload-file"
          type="file"
          :class="{
            'form-control': true,
            'is-invalid': isFieldInError('upload-file'),
          }"
          @drop="uploadFileChange"
          @change="uploadFileChange"
        />
        <div class="btn-toolbar mt-3">
          <button
            class="btn btn-primary"
            type="button"
            :disabled="uploadFile === null || uploadInProgress"
            @click.stop.prevent="uploadImageClick()"
          >
            {{ uploadInProgress ? 'Uploading...' : 'Upload' }}
          </button>
        </div>
      </div>
      <div class="g-col-12 g-col-md-6 text-center">
        <div
          v-if="images.length > 0"
          :id="`image-carousel-${uniqueId}`"
          class="carousel slide"
          data-bs-interval="false"
        >
          <div class="carousel-indicators d-print-none">
            <button
              v-for="(imageName, i) in images"
              :key="`${imageName}:${props.suggestedImage}`"
              type="button"
              :data-bs-target="`#image-carousel-${uniqueId}`"
              :data-bs-slide-to="i"
              :class="{ active: i === carouselIndex }"
              :aria-current="i === carouselIndex"
              :aria-label="`Show image ${i}`"
            ></button>
          </div>
          <div class="carousel-inner">
            <div
              v-for="(imageName, i) in images"
              :key="`${imageName}:${props.suggestedImage}`"
              :class="{ 'carousel-item': true, active: i === carouselIndex }"
            >
              <button
                v-if="images.length > 0 && imageName != pinnedImage"
                type="button"
                class="btn btn-light btn-sm image-button image-button-left d-print-none"
                title="Pin image"
                @click.stop.prevent="pinImageClick(imageName)"
              >
                <span class="visually-hidden">Pin image</span>
                <font-awesome-icon icon="fa-thumbtack" />
              </button>
              <button
                v-if="images.length > 0"
                type="button"
                class="btn btn-danger btn-sm image-button image-button-right d-print-none"
                title="Delete image"
                @click.stop.prevent="deleteImageClick(imageName)"
              >
                <span class="visually-hidden">Delete image</span>
                <font-awesome-icon icon="fa-times" />
              </button>
              <img
                class="img-fluid rounded object-fit-cover"
                :src="ApiHelper.imageUrl(imageName)"
                :alt="`image ${i}`"
                :loading="i > 0 ? 'lazy' : 'eager'"
                width="1600"
                height="1200"
                style="aspect-ratio: 4 / 3"
              />
            </div>
          </div>
          <button
            class="carousel-control-prev d-print-none"
            type="button"
            :data-bs-target="`#image-carousel-${uniqueId}`"
            data-bs-slide="prev"
          >
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous image</span>
          </button>
          <button
            class="carousel-control-next d-print-none"
            type="button"
            :data-bs-target="`#image-carousel-${uniqueId}`"
            data-bs-slide="next"
          >
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next image</span>
          </button>
        </div>
        <ImagePlaceholder v-else class="img-fluid rounded" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.image-button {
  position: absolute;
  top: 0;
  z-index: 2;

  &.image-button-right {
    right: 0;
  }

  &.image-button-left {
    left: 0;
  }
}
</style>
