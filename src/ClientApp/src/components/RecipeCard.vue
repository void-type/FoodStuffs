<script lang="ts" setup>
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useImageLightboxStore from '@/stores/imageLightboxStore';
import AppSortHandle from './AppSortHandle.vue';
import ImagePlaceholder from './ImagePlaceholder.vue';
import RecipeCurrentMealPlanButton from './RecipeCurrentMealPlanButton.vue';
import TagBadge from './TagBadge.vue';

// Minimal shape shared by search results, meal plan recipes, and recently viewed recipes.
interface RecipeCardRecipe {
  id?: number;
  name?: string;
  image?: string | null;
  mealPlanningSidesCount?: number;
  categories?: { id?: number; name?: string; color?: string; showOnCards?: boolean }[];
}

const props = defineProps({
  recipe: { type: Object as PropType<RecipeCardRecipe>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
  // Extra class(es) applied to the image/placeholder, e.g. for a greyscale completed state.
  imgClass: {
    type: [String, Array, Object] as PropType<string | string[] | Record<string, boolean | undefined>>,
    required: false,
    default: '',
  },
  // Suppresses the image enlarge button, e.g. when a meal plan recipe is complete.
  hideImageExpand: { type: Boolean, required: false, default: false },
});

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);
const cardCategories = computed(() => (props.recipe.categories || []).filter(c => c.showOnCards));
const sidesCount = computed(() => props.recipe.mealPlanningSidesCount || 0);
const imageLightboxStore = useImageLightboxStore();
</script>

<template>
  <div :id="recipeCardId" class="card">
    <div class="card-title-row d-flex align-items-center gap-2 min-w-0">
      <AppSortHandle v-if="props.showSortHandle" />
      <router-link class="fw-semibold mb-0 card-title-text card-title-link" :to="RouterHelper.viewRecipe(recipe)">
        {{ recipe.name }}
      </router-link>
    </div>
    <div class="d-flex flex-row">
      <div class="image-container flex-shrink-0 position-relative">
        <router-link class="card-link card-hover" :to="RouterHelper.viewRecipe(recipe)">
          <img
            v-if="recipe.image != null"
            class="img-fluid"
            :class="imgClass"
            :src="ApiHelper.imageUrl(recipe.image)"
            :alt="`Image of ${recipe.name}`"
            :loading="imgLazy ? 'lazy' : 'eager'"
            width="1600"
            height="1200"
          >
          <ImagePlaceholder
            v-else
            class="img-fluid position-absolute top-0 left-0 bottom-0 end-0"
            :class="imgClass"
          />
        </router-link>
        <slot name="image-overlay" />
        <button
          v-if="recipe.image != null && !hideImageExpand"
          type="button"
          class="btn btn-dark btn-sm position-absolute bottom-0 end-0 m-1 opacity-75 d-print-none"
          aria-label="Enlarge image"
          @click.stop.prevent="imageLightboxStore.open([recipe.image!])"
        >
          <FontAwesomeIcon icon="fa-expand" />
        </button>
      </div>
      <div class="card-body flex-grow-1 d-flex flex-column gap-2 min-w-0 p-2">
        <div v-if="sidesCount > 0" class="small text-muted">
          {{ sidesCount }} side{{ sidesCount !== 1 ? 's' : '' }}
        </div>
        <div v-if="cardCategories.length > 0" class="d-flex flex-wrap gap-1">
          <TagBadge v-for="tag in cardCategories" :key="tag.id" :tag="tag" />
        </div>
        <slot />
        <div class="d-flex flex-wrap align-items-center justify-content-between gap-2 mt-auto">
          <div class="d-flex flex-wrap gap-3">
            <router-link
              type="button"
              class="btn btn-outline-secondary btn-sm d-print-none"
              aria-label="Edit recipe"
              :to="RouterHelper.editRecipe(recipe)"
              @click.stop
            >
              <FontAwesomeIcon icon="fa-pen" />
            </router-link>
            <RecipeCurrentMealPlanButton class="btn-sm d-print-none" invert :recipe-id="recipe.id" />
            <slot name="actions" />
          </div>
          <slot name="trailing" />
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.image-container {
  position: relative;
  width: 165px;
  aspect-ratio: 1600 / 1200;
  flex-shrink: 0;
  align-self: flex-start;
  overflow: hidden;
  border-radius: var(--bs-card-inner-border-radius);

  img {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
}

.card-title-row {
  padding: calc(var(--bs-card-spacer-y) / 2) calc(var(--bs-card-spacer-x) / 2);
  border: none;
}

.card-title-text {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 2;
  overflow: hidden;
}

.card-title-link {
  color: var(--bs-link-color) !important;
  text-decoration: none !important;

  &:hover,
  &:focus {
    color: var(--bs-link-hover-color) !important;
    text-decoration: underline !important;
  }
}

.min-w-0 {
  min-width: 0;
}
</style>
