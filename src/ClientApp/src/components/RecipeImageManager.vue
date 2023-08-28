<script lang="ts" setup>
import { onMounted, ref, watch, type PropType, type Ref } from 'vue';
import { clamp } from '@/models/FormatHelpers';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelpers from '@/models/ApiHelpers';
import type { HTMLInputEvent } from '@/models/HTMLInputEvent';
import type bootstrap from 'bootstrap';
import useMessageStore from '@/stores/messageStore';
import ImagePlaceholder from './ImagePlaceholder.vue';

const props = defineProps({
  imageIds: {
    type: Array as PropType<Array<number>>,
    required: true,
    default: () => [],
  },
  suggestedImageId: {
    type: Number as PropType<number | null>,
    required: false,
    default: null,
  },
  pinnedImageId: {
    type: Number as PropType<number | null>,
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
const carouselIndex = ref(0);
const uniqueId = crypto.randomUUID();

function imageUrl(imageId: number) {
  return ApiHelpers.imageUrl(imageId);
}

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

function deleteImageClick(imageId: number) {
  props.onImageDelete(imageId);
}

function pinImageClick(imageId: number) {
  props.onImagePin(imageId);
}

watch([() => props.imageIds, () => props.suggestedImageId], () => {
  const suggestedImageIndex =
    props.suggestedImageId === null ? -1 : props.imageIds.indexOf(props.suggestedImageId);
  const newIndex = suggestedImageIndex > -1 ? suggestedImageIndex : carouselIndex.value;
  carouselIndex.value = clamp(newIndex, 0, props.imageIds.length - 1);
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
            'text-nowrap': true,
            'text-truncate': true,
            'is-invalid': isFieldInError('upload'),
          }"
          placeholder="Drop file or click to browse..."
          @drop="uploadFileChange"
          @change="uploadFileChange"
        />
        <div class="btn-toolbar mt-3">
          <button
            class="btn btn-primary"
            type="button"
            :disabled="uploadFile === null"
            @click.stop.prevent="uploadImageClick()"
          >
            Upload
          </button>
        </div>
      </div>
      <div class="g-col-12 g-col-md-6 text-center">
        <div
          v-if="imageIds.length > 0"
          :id="`image-carousel-${uniqueId}`"
          class="carousel slide"
          data-bs-interval="false"
        >
          <div class="carousel-indicators d-print-none">
            <button
              v-for="(imageId, i) in imageIds"
              :key="`${imageId}:${props.suggestedImageId}`"
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
              v-for="(imageId, i) in imageIds"
              :key="`${imageId}:${props.suggestedImageId}`"
              :class="{ 'carousel-item': true, active: i === carouselIndex }"
            >
              <button
                v-if="imageIds.length > 0 && imageId != pinnedImageId"
                type="button"
                class="btn btn-secondary btn-sm image-button image-button-pin d-print-none"
                title="Pin image"
                @click.stop.prevent="pinImageClick(imageId)"
              >
                <span class="visually-hidden">Pin image</span>
                <font-awesome-icon icon="thumbtack" />
              </button>
              <button
                v-if="imageIds.length > 0"
                type="button"
                class="btn btn-danger btn-sm image-button image-button-delete d-print-none"
                title="Delete image"
                @click.stop.prevent="deleteImageClick(imageId)"
              >
                <span class="visually-hidden">Delete image</span>
                <font-awesome-icon icon="times" />
              </button>
              <img
                class="img-fluid rounded"
                :src="imageUrl(imageId)"
                :alt="`image ${i}`"
                :loading="i > 0 ? 'lazy' : 'eager'"
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
            <span class="visually-hidden">Previous</span>
          </button>
          <button
            class="carousel-control-next d-print-none"
            type="button"
            :data-bs-target="`#image-carousel-${uniqueId}`"
            data-bs-slide="next"
          >
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
          </button>
        </div>
        <ImagePlaceholder v-else class="rounded" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
div.carousel-item img {
  max-height: 16rem;
}

.image-button {
  position: absolute;
  top: 0;
  min-width: 0;
  z-index: 2;

  &.image-button-delete {
    right: 0;
  }

  &.image-button-pin {
    left: 0;
  }
}
</style>
