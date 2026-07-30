<script lang="ts" setup>
import type bootstrap from 'bootstrap';
import { Carousel, Modal } from 'bootstrap';
import { storeToRefs } from 'pinia';
import { computed, onMounted, onUnmounted, ref, watch } from 'vue';
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
  return Carousel.getOrCreateInstance(el, { interval: false, touch: false });
}

let pushedState = false;

function onPopState() {
  pushedState = false;
  store.close();
}

// --- Zoom / Pan ---
const zoomScale = ref(1);
const zoomPanX = ref(0);
const zoomPanY = ref(0);
const isDraggingActive = ref(false);

const zoomStyle = computed(() => {
  if (zoomScale.value === 1 && zoomPanX.value === 0 && zoomPanY.value === 0) {
    return undefined;
  }
  return { transform: `translate(${zoomPanX.value}px, ${zoomPanY.value}px) scale(${zoomScale.value})` };
});

function resetZoom() {
  zoomScale.value = 1;
  zoomPanX.value = 0;
  zoomPanY.value = 0;
  isDraggingActive.value = false;
}

const activePointers = new Map<number, { x: number; y: number }>();
let pinchStartDist = 0;
let pinchStartScale = 1;
let dragStartX = 0;
let dragStartY = 0;
let dragStartPanX = 0;
let dragStartPanY = 0;
let swipeStartX = 0;
let swipeStartY = 0;
let lastTapTime = 0;
let lastTapX = 0;
let lastTapY = 0;

function dist2d(a: { x: number; y: number }, b: { x: number; y: number }) {
  return Math.hypot(b.x - a.x, b.y - a.y);
}

function onPointerDown(e: PointerEvent) {
  (e.currentTarget as HTMLElement).setPointerCapture(e.pointerId);
  activePointers.set(e.pointerId, { x: e.clientX, y: e.clientY });

  if (activePointers.size === 2) {
    const [p1, p2] = [...activePointers.values()];
    pinchStartDist = dist2d(p1, p2);
    pinchStartScale = zoomScale.value;
  } else if (activePointers.size === 1) {
    dragStartX = e.clientX;
    dragStartY = e.clientY;
    dragStartPanX = zoomPanX.value;
    dragStartPanY = zoomPanY.value;
    swipeStartX = e.clientX;
    swipeStartY = e.clientY;

    const now = Date.now();
    if (now - lastTapTime < 300 && dist2d({ x: e.clientX, y: e.clientY }, { x: lastTapX, y: lastTapY }) < 40) {
      zoomScale.value > 1 ? resetZoom() : (zoomScale.value = 2.5);
      lastTapTime = 0;
    } else {
      lastTapTime = now;
      lastTapX = e.clientX;
      lastTapY = e.clientY;
    }
  }
}

function onPointerMove(e: PointerEvent) {
  if (!activePointers.has(e.pointerId)) {
    return;
  }
  activePointers.set(e.pointerId, { x: e.clientX, y: e.clientY });

  if (activePointers.size === 2) {
    const [p1, p2] = [...activePointers.values()];
    zoomScale.value = Math.min(5, Math.max(1, (dist2d(p1, p2) / pinchStartDist) * pinchStartScale));
    if (zoomScale.value <= 1) {
      zoomPanX.value = 0;
      zoomPanY.value = 0;
    }
  } else if (activePointers.size === 1 && zoomScale.value > 1) {
    isDraggingActive.value = true;
    zoomPanX.value = dragStartPanX + (e.clientX - dragStartX);
    zoomPanY.value = dragStartPanY + (e.clientY - dragStartY);
  }
}

function onPointerUp(e: PointerEvent) {
  const hadSinglePointer = activePointers.size === 1;
  activePointers.delete(e.pointerId);
  isDraggingActive.value = false;

  if (hadSinglePointer && zoomScale.value <= 1) {
    const dx = e.clientX - swipeStartX;
    const dy = e.clientY - swipeStartY;
    if (Math.abs(dx) > 50 && Math.abs(dx) > Math.abs(dy)) {
      dx < 0 ? getCarousel()?.next() : getCarousel()?.prev();
    }
  }

  // Transitioning from pinch (2→1 touch): reset drag anchor to remaining finger
  if (!hadSinglePointer && activePointers.size === 1) {
    const [remaining] = [...activePointers.values()];
    dragStartX = remaining.x;
    dragStartY = remaining.y;
    dragStartPanX = zoomPanX.value;
    dragStartPanY = zoomPanY.value;
  }
}

function onPointerCancel(e: PointerEvent) {
  activePointers.delete(e.pointerId);
  isDraggingActive.value = false;
}

function onWheel(e: WheelEvent) {
  const delta = e.deltaY < 0 ? 0.2 : -0.2;
  zoomScale.value = Math.min(5, Math.max(1, zoomScale.value + delta));
  if (zoomScale.value <= 1) {
    zoomPanX.value = 0;
    zoomPanY.value = 0;
  }
}
// --- End Zoom / Pan ---

function onCarouselSlide() {
  resetZoom();
}

function onCarouselSlid(event: Event) {
  const carouselEvent = event as unknown as bootstrap.Carousel.Event;
  currentIndex.value = carouselEvent.to;
}

watch(isActive, (active) => {
  if (active) {
    history.pushState({ lightbox: true }, '');
    pushedState = true;
    getModal().show();
  } else {
    resetZoom();
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
      const carouselEl = document.getElementById('image-lightbox-carousel');
      if (carouselEl) {
        carouselEl.removeEventListener('slide.bs.carousel', onCarouselSlide);
        carouselEl.removeEventListener('slid.bs.carousel', onCarouselSlid);
      }
      Carousel.getInstance(document.getElementById('image-lightbox-carousel')!)?.dispose();
      store.close();
    });
    modalEl.addEventListener('shown.bs.modal', () => {
      const carouselEl = document.getElementById('image-lightbox-carousel');
      if (carouselEl !== null) {
        carouselEl.addEventListener('slide.bs.carousel', onCarouselSlide);
        carouselEl.addEventListener('slid.bs.carousel', onCarouselSlid);
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
            class="zoom-wrapper"
            :class="{ 'is-zoomed': zoomScale > 1, 'is-dragging': isDraggingActive }"
            :style="zoomStyle"
            @pointerdown="onPointerDown"
            @pointermove="onPointerMove"
            @pointerup="onPointerUp"
            @pointercancel="onPointerCancel"
            @wheel.prevent="onWheel"
          >
            <div
              id="image-lightbox-carousel"
              class="carousel slide"
              data-bs-interval="false"
              data-bs-touch="false"
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
                    draggable="false"
                  >
                </div>
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
  max-height: 80dvh;
}

.zoom-wrapper {
  touch-action: none;
  user-select: none;
  transform-origin: center center;
  will-change: transform;

  &.is-zoomed {
    cursor: grab;
  }

  &.is-dragging {
    cursor: grabbing;
  }
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
