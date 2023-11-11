<script lang="ts" setup>
import { ref, computed, type PropType, onMounted, watch } from 'vue';
import type bootstrap from 'bootstrap';
import type { GetRecipeResponse } from '@/api/data-contracts';
import ApiHelpers from '@/models/ApiHelpers';
import { isNil } from '@/models/FormatHelpers';
import { toTimeSpanString } from '@/models/TimeSpanHelpers';
import EntityAuditInfo from '@/components/EntityAuditInfo.vue';
import RecipeViewerIngredients from '@/components/RecipeViewerIngredients.vue';
import useAppStore from '@/stores/appStore';
import { storeToRefs } from 'pinia';
import ImagePlaceholder from './ImagePlaceholder.vue';

const props = defineProps({
  recipe: {
    type: Object as PropType<GetRecipeResponse>,
    required: true,
  },
});

const appStore = useAppStore();
const { useDarkMode } = storeToRefs(appStore);

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
    <div class="row">
      <div class="col-12 d-print-none">
        <div class="btn-toolbar">
          <router-link
            :to="{ name: 'edit', params: { id: recipe.id } }"
            class="btn btn-primary me-3"
            type="button"
          >
            Edit
          </router-link>
          <div class="form-check form-switch mt-2">
            <label class="form-check-label" for="showImage">Show image</label>
            <input id="showImage" v-model="showImage" class="form-check-input" type="checkbox" />
          </div>
        </div>
      </div>
    </div>
    <div v-if="showImage" class="row mb-5">
      <div
        :class="{
          'col-12': true,
          'text-center': true,
          'mt-4': true,
          'd-print-none': !(images.length > 0),
        }"
      >
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
                class="img-fluid rounded"
                :src="ApiHelpers.imageUrl(imageName)"
                :alt="`image ${i} of ${recipe.name}`"
                :loading="i > 0 ? 'lazy' : 'eager'"
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
        <ImagePlaceholder v-else class="rounded" />
      </div>
    </div>
    <h3 v-if="(recipe.ingredients?.length || 0) > 0" class="mt-4">Ingredients</h3>
    <RecipeViewerIngredients
      v-if="(recipe.ingredients?.length || 0) > 0"
      :ingredients="recipe.ingredients || []"
    />
    <h3 v-if="!isNil(recipe.directions)" class="mt-3">Directions</h3>
    <div
      v-if="!isNil(recipe.directions)"
      :class="{ 'form-control-plaintext': true, 'p-0': true, 'text-light': useDarkMode }"
    >
      {{ recipe.directions }}
    </div>
    <h3
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
    </h3>
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

div.carousel-item img {
  max-height: 20rem;
}
</style>
