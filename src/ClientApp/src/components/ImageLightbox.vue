<script lang="ts" setup>
import type bootstrap from 'bootstrap';
import { Carousel, Modal } from 'bootstrap';
import { storeToRefs } from 'pinia';
import { onMounted, onUnmounted, watch } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import useImageLightboxStore from '@/stores/imageLightboxStore';

const store = useImageLightboxStore();
const { isActive, images, currentIndex } = storeToRefs(store);

function getModal() {
  return Modal.getOrCreateInstance('#image-lightbox');
}

function getCarousel() {
  const el = document.getElementById('image-lightbox-carousel');
  if (el === null) {
    return null;
  }
  return Carousel.getOrCreateInstance(el, { interval: false });
}

let pushedState = false;

function onPopState() {
  pushedState = false;
  store.close();
}

watch(isActive, (active) => {
  if (active) {
    history.pushState({ lightbox: true }, '');
    pushedState = true;
    getModal().show();
  } else {
    getModal().hide();
    if (pushedState) {
      pushedState = false;
      history.back();
    }
  }
});

onUnmounted(() => {
  window.removeEventListener('popstate', onPopState);
});

onMounted(() => {
  window.addEventListener('popstate', onPopState);
  const modalEl = document.getElementById('image-lightbox');

  if (modalEl !== null) {
    modalEl.addEventListener('hidden.bs.modal', () => {
      Carousel.getInstance(document.getElementById('image-lightbox-carousel')!)?.dispose();
      store.close();
    });
    modalEl.addEventListener('shown.bs.modal', () => {
      const carouselEl = document.getElementById('image-lightbox-carousel');
      if (carouselEl !== null) {
        carouselEl.addEventListener('slid.bs.carousel', (event) => {
          const carouselEvent = event as unknown as bootstrap.Carousel.Event;
          currentIndex.value = carouselEvent.to;
        });
      }
      getCarousel()?.to(currentIndex.value);
    });
  }
});

function handleKeydown(event: KeyboardEvent) {
  if (event.key === 'ArrowLeft') {
    event.preventDefault();
    getCarousel()?.prev();
  } else if (event.key === 'ArrowRight') {
    event.preventDefault();
    getCarousel()?.next();
  }
}
</script>

<template>
  <Teleport to="body">
    <div
      id="image-lightbox"
      class="modal fade d-print-none"
      tabindex="-1"
      aria-hidden="true"
      @keydown="handleKeydown"
    >
      <button
        type="button"
        class="image-lightbox-close-button btn btn-dark rounded-circle d-flex align-items-center justify-content-center position-fixed top-0 end-0 m-2 p-0"
        aria-label="Close"
        @click="store.close()"
      >
        <div
          class="btn-close btn-close-white"
        />
      </button>
      <div
        class="modal-dialog modal-dialog-centered"
      >
        <div
          class="modal-content bg-transparent border-0 shadow-none"
        >
          <div
            v-if="images.length > 0"
            id="image-lightbox-carousel"
            class="carousel slide"
            data-bs-interval="false"
          >
            <div class="carousel-inner">
              <div
                v-for="(imageName, i) in images"
                :key="imageName"
                class="carousel-item"
                :class="{ active: i === currentIndex }"
              >
                <img
                  class="lightbox-image rounded d-block mx-auto"
                  :src="ApiHelper.imageUrl(imageName)"
                  :alt="`Enlarged image ${i + 1}`"
                >
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        v-if="images.length > 1"
        class="carousel-indicators"
      >
        <button
          v-for="(imageName, i) in images"
          :key="imageName"
          type="button"
          data-bs-target="#image-lightbox-carousel"
          :data-bs-slide-to="i"
          :class="{ active: i === currentIndex }"
          :aria-current="i === currentIndex"
          :aria-label="`Show image ${i + 1}`"
        />
      </div>
      <button
        v-if="images.length > 1"
        type="button"
        class="carousel-control-prev"
        data-bs-target="#image-lightbox-carousel"
        data-bs-slide="prev"
      >
        <span class="carousel-control-prev-icon" aria-hidden="true" />
        <span class="visually-hidden">Previous image</span>
      </button>
      <button
        v-if="images.length > 1"
        type="button"
        class="carousel-control-next"
        data-bs-target="#image-lightbox-carousel"
        data-bs-slide="next"
      >
        <span class="carousel-control-next-icon" aria-hidden="true" />
        <span class="visually-hidden">Next image</span>
      </button>
    </div>
  </Teleport>
</template>

<style lang="scss" scoped>
.lightbox-image {
  max-width: 100%;
  height: auto;
  max-height: 80vh;
}

button.image-lightbox-close-button {
  --bs-btn-bg: var(--bs-black);
  width: 2.5rem;
  height: 2.5rem;
  z-index: 2;
}

.modal-dialog {
  width: fit-content;
  max-width: 95vw;
  margin-left: auto;
  margin-right: auto;
}
</style>
