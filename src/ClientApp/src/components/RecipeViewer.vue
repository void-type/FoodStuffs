<script lang="ts" setup>
import { ref, computed, type PropType, onMounted, watch } from 'vue';
import type bootstrap from 'bootstrap';
import type { GetRecipeResponse } from '@/api/data-contracts';
import ApiHelper from '@/models/ApiHelper';
import { isNil } from '@/models/FormatHelper';
import { toTimeSpanString } from '@/models/TimeSpanHelper';
import EntityAuditInfo from '@/components/EntityAuditInfo.vue';
import RouterHelper from '@/models/RouterHelper';
import ImagePlaceholder from './ImagePlaceholder.vue';
import RecipeMealButton from './RecipeMealButton.vue';

const props = defineProps({
  recipe: {
    type: Object as PropType<GetRecipeResponse>,
    required: true,
  },
});

const showImage = ref(true);
const carouselIndex = ref(0);
const uniqueId = crypto.randomUUID();

const images = computed(() => {
  const { pinnedImage } = props.recipe;
  const recipeImages = props.recipe.images || [];

  if (pinnedImage != null && recipeImages.includes(pinnedImage)) {
    const sortedRecipeImages = recipeImages.filter((i) => i !== pinnedImage);

    sortedRecipeImages.unshift(pinnedImage);

    return sortedRecipeImages;
  }

  return recipeImages;
});

function timeSpanFormat(totalMinutes: number | null | undefined) {
  if (totalMinutes === null || totalMinutes === undefined) {
    return '';
  }

  return toTimeSpanString(totalMinutes);
}

watch(
  () => props.recipe,
  () => {
    carouselIndex.value = 0;
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
    <div class="btn-toolbar sticky-top pt-1">
      <router-link :to="RouterHelper.editRecipe(recipe)" class="btn btn-primary me-2" type="button">
        Edit
      </router-link>
      <RecipeMealButton class="me-2" :recipe-id="recipe.id" />
    </div>
    <div class="btn-toolbar mt-3 d-print-none">
      <div class="form-check form-switch mt-">
        <label class="form-check-label" for="showImage">Show image</label>
        <input id="showImage" v-model="showImage" class="form-check-input" type="checkbox" />
      </div>
    </div>
    <div
      v-if="showImage"
      :class="{
        'g-col-12 text-center mt-4 mb-5': true,
        'd-print-none': !(images.length > 0),
      }"
    >
      <div class="carousel-wrapper mx-auto">
        <div
          v-if="images.length > 0"
          :id="`image-carousel-${uniqueId}`"
          class="carousel slide"
          data-bs-interval="false"
        >
          <div class="carousel-indicators d-print-none">
            <button
              v-for="(imageName, i) in images"
              :key="imageName"
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
              :key="imageName"
              :class="{ 'carousel-item': true, active: i === carouselIndex }"
            >
              <img
                class="img-fluid rounded object-fit-cover"
                :src="ApiHelper.imageUrl(imageName)"
                :alt="`image ${i} of ${recipe.name}`"
                :loading="i > 0 ? 'lazy' : 'eager'"
                width="1600"
                height="1200"
                style="aspect-ratio: 4 / 3"
              />
            </div>
          </div>
          <button
            type="button"
            class="carousel-control-prev d-print-none"
            :data-bs-target="`#image-carousel-${uniqueId}`"
            data-bs-slide="prev"
          >
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous image</span>
          </button>
          <button
            type="button"
            class="carousel-control-next d-print-none"
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
    <h2 v-if="!isNil(recipe.directions)" class="mt-3">Directions</h2>
    <!-- eslint-disable-next-line vue/no-v-html -->
    <div v-if="!isNil(recipe.directions)" class="rich-text" v-html="recipe.directions"></div>
    <h2 v-if="!isNil(recipe.sides)" class="mt-3">Sides</h2>
    <div v-if="!isNil(recipe.sides)" :class="{ 'form-control-plaintext p-0': true }">
      {{ recipe.sides }}
    </div>
    <h2 v-if="(recipe.shoppingItems?.length || 0) > 0" class="mt-3 mb-0 d-print-none">
      Grocery Items
    </h2>
    <div class="badge p-0 mb-2 text-muted">
      <small>Not printed.</small>
    </div>
    <ul>
      <li v-for="item in recipe.shoppingItems || []" :key="item.id">
        {{ item.quantity }}x {{ item.name }}
      </li>
    </ul>
    <h2
      v-if="
        recipe.prepTimeMinutes ||
        0 > 0 ||
        recipe.cookTimeMinutes ||
        0 > 0 ||
        (recipe.categories || []).length > 0
      "
      class="mt-3"
    >
      Stats
    </h2>
    <div v-if="recipe.prepTimeMinutes || 0 > 0">
      Prep Time: {{ timeSpanFormat(recipe.prepTimeMinutes) }}
    </div>
    <div v-if="recipe.cookTimeMinutes || 0 > 0">
      Cook Time: {{ timeSpanFormat(recipe.cookTimeMinutes) }}
    </div>
    <div v-if="(recipe.categories || []).length > 0">
      Categories: {{ (recipe.categories || []).join(', ') }}
    </div>
    <div>
      <EntityAuditInfo class="mt-3" :entity="recipe" />
    </div>
  </div>
</template>

<style lang="scss" scoped>
div.form-control-plaintext {
  white-space: pre-wrap;
}

.carousel-wrapper {
  max-width: 27rem;
}
</style>
