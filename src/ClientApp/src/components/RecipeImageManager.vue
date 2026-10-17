<script lang="ts" setup>
import type bootstrap from 'bootstrap';
import type { PropType } from 'vue';
import type { HTMLInputEvent } from '@/models/HTMLInputEvent';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { onMounted, ref, watch } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import { clamp } from '@/models/FormatHelper';
import useImageLightboxStore from '@/stores/imageLightboxStore';
import useMessageStore from '@/stores/messageStore';
import ImagePlaceholder from './ImagePlaceholder.vue';

const props = defineProps({
  // Used to reset the carousel when a different recipe is loaded.
  recipeId: {
    type: Number,
    required: false,
    default: 0,
  },
  images: {
    type: Array as PropType<Array<string>>,
    required: true,
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
    type: Function as PropType<(file: File) => Promise<unknown>>,
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
const imageLightboxStore = useImageLightboxStore();

const uploadingCount = ref(0);
const isDragOver = ref(false);
const carouselIndex = ref(0);
const uniqueId = crypto.randomUUID();

const fileSizeLimit = 30000000;

function toMiB(bytes: number) {
  const mb = bytes / (1024 * 1024);
  return Math.round(mb * 100) / 100;
}

function uploadFile(file: File) {
  if (!file.type.startsWith('image/')) {
    messageStore.setValidationErrorMessages([
      {
        message: `"${file.name}" is not an image and was skipped.`,
        uiHandle: 'upload',
      },
    ]);

    return;
  }

  if (file.size > fileSizeLimit) {
    const sizeMiB = toMiB(file.size);
    const limitMiB = toMiB(fileSizeLimit);

    messageStore.setValidationErrorMessages([
      {
        message: `"${file.name}" (${sizeMiB} MB) exceeds the limit (${limitMiB} MB).`,
        uiHandle: 'upload',
      },
    ]);

    return;
  }

  uploadingCount.value += 1;

  props.onImageUpload(file).finally(() => {
    uploadingCount.value = Math.max(0, uploadingCount.value - 1);
  });
}

function processFiles(files: FileList | null) {
  if (files === null || files.length < 1) {
    return;
  }

  Array.from(files).forEach(uploadFile);
}

function uploadFileChange(event: Event) {
  const input = (event as HTMLInputEvent).target;

  processFiles(input?.files || null);

  if (input !== null && input !== undefined) {
    input.value = '';
  }
}

function dropzoneDrop(event: DragEvent) {
  isDragOver.value = false;
  processFiles(event.dataTransfer?.files || null);
}

function dropzoneDragOver() {
  isDragOver.value = true;
}

function dropzoneDragLeave() {
  isDragOver.value = false;
}

function deleteImageClick(name: string) {
  props.onImageDelete(name);
}

function pinImageClick(name: string) {
  props.onImagePin(name);
}

watch([() => props.images, () => props.suggestedImage], () => {
  const suggestedImageIndex
    = props.suggestedImage === null ? -1 : props.images.indexOf(props.suggestedImage);
  const newIndex = suggestedImageIndex > -1 ? suggestedImageIndex : carouselIndex.value;
  carouselIndex.value = clamp(newIndex, 0, props.images.length - 1);
});

watch(
  () => props.recipeId,
  () => {
    carouselIndex.value = 0;
  },
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
        <label
          for="upload-file"
          class="upload-dropzone d-flex flex-md-column align-items-center justify-content-center text-center px-3 py-2 p-md-4"
          :class="{
            'is-dragover': isDragOver,
            'is-invalid': isFieldInError('upload-file'),
          }"
          @dragover.prevent="dropzoneDragOver"
          @dragenter.prevent="dropzoneDragOver"
          @dragleave.prevent="dropzoneDragLeave"
          @drop.prevent="dropzoneDrop"
        >
          <FontAwesomeIcon icon="fa-cloud-arrow-up" class="upload-icon me-2 me-md-0 mb-md-2 text-secondary" />
          <div>
            <span>Drag and drop images here, or click to browse</span>
            <span v-if="uploadingCount > 0" class="d-block text-primary mt-1 mt-md-2">
              Uploading {{ uploadingCount }} image{{ uploadingCount === 1 ? '' : 's' }}...
            </span>
          </div>
          <input
            id="upload-file"
            type="file"
            class="visually-hidden"
            accept="image/*"
            multiple
            @change="uploadFileChange"
          >
        </label>
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
              :aria-label="`Show image ${i + 1}`"
            />
          </div>
          <div class="carousel-inner">
            <div
              v-for="(imageName, i) in images"
              :key="`${imageName}:${props.suggestedImage}`"
              class="carousel-item" :class="{ active: i === carouselIndex }"
            >
              <button
                v-if="images.length > 0 && imageName !== pinnedImage"
                type="button"
                class="btn btn-light btn-sm image-button image-button-left d-print-none"
                title="Pin image"
                @click.stop.prevent="pinImageClick(imageName)"
              >
                <span class="visually-hidden">Pin image</span>
                <FontAwesomeIcon icon="fa-thumbtack" />
              </button>
              <button
                v-if="images.length > 0"
                type="button"
                class="btn btn-danger btn-sm image-button image-button-right d-print-none"
                title="Delete image"
                @click.stop.prevent="deleteImageClick(imageName)"
              >
                <span class="visually-hidden">Delete image</span>
                <FontAwesomeIcon icon="fa-times" />
              </button>
              <img
                class="img-fluid rounded object-fit-cover"
                role="button"
                :src="ApiHelper.imageUrl(imageName)"
                :alt="`image ${i + 1}`"
                :loading="i > 0 ? 'lazy' : 'eager'"
                width="1600"
                height="1200"
                style="aspect-ratio: 4 / 3; cursor: pointer"
                @click="imageLightboxStore.open(props.images, i)"
              >
            </div>
          </div>
          <button
            class="carousel-control-prev d-print-none"
            type="button"
            :data-bs-target="`#image-carousel-${uniqueId}`"
            data-bs-slide="prev"
          >
            <span class="carousel-control-prev-icon" aria-hidden="true" />
            <span class="visually-hidden">Previous image</span>
          </button>
          <button
            class="carousel-control-next d-print-none"
            type="button"
            :data-bs-target="`#image-carousel-${uniqueId}`"
            data-bs-slide="next"
          >
            <span class="carousel-control-next-icon" aria-hidden="true" />
            <span class="visually-hidden">Next image</span>
          </button>
        </div>
        <ImagePlaceholder v-else class="img-fluid rounded" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.upload-dropzone {
  cursor: pointer;
  border: 2px dashed var(--bs-border-color);
  border-radius: var(--bs-border-radius);
  min-height: 100%;
  transition:
    background-color 0.15s ease-in-out,
    border-color 0.15s ease-in-out;

  &:hover,
  &.is-dragover {
    background-color: var(--bs-tertiary-bg);
    border-color: var(--bs-primary);
  }

  &.is-invalid {
    border-color: var(--bs-danger);
  }
}

.upload-icon {
  // Inline with the text on mobile, larger and stacked on md+.
  font-size: 1.25rem;
  flex-shrink: 0;

  @media (min-width: 768px) {
    font-size: 2rem;
  }
}

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
