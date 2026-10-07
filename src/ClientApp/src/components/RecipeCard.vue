<script lang="ts" setup>
import type { PropType } from 'vue';
import type { SearchRecipesResultItem } from '@/api/data-contracts';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useImageLightboxStore from '@/stores/imageLightboxStore';
import AppSortHandle from './AppSortHandle.vue';
import ImagePlaceholder from './ImagePlaceholder.vue';
import RecipeCurrentMealPlanButton from './RecipeCurrentMealPlanButton.vue';

const props = defineProps({
  recipe: { type: Object as PropType<SearchRecipesResultItem>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
});

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);
const imageLightboxStore = useImageLightboxStore();
</script>

<template>
  <div :id="recipeCardId" class="card flex-row">
    <div class="image-container flex-shrink-0 position-relative">
      <router-link class="card-link card-hover" :to="RouterHelper.viewRecipe(recipe)">
        <img
          v-if="recipe.image != null"
          class="img-fluid"
          :src="ApiHelper.imageUrl(recipe.image)"
          :alt="`Image of ${recipe.name}`"
          :loading="imgLazy ? 'lazy' : 'eager'"
          width="1600"
          height="1200"
        >
        <ImagePlaceholder v-else class="img-fluid position-absolute top-0 left-0 bottom-0 end-0" />
      </router-link>
      <button
        v-if="recipe.image != null"
        type="button"
        class="btn btn-dark btn-sm position-absolute bottom-0 end-0 m-1 opacity-75 d-print-none"
        aria-label="Enlarge image"
        @click.stop.prevent="imageLightboxStore.open([recipe.image!])"
      >
        <FontAwesomeIcon icon="fa-expand" />
      </button>
    </div>
    <div class="card-body flex-grow-1 d-flex flex-column gap-2 min-w-0">
      <div class="d-flex align-items-center gap-2 min-w-0">
        <AppSortHandle v-if="props.showSortHandle" />
        <router-link class="h4 mb-0 text-truncate card-title-link" :to="RouterHelper.viewRecipe(recipe)">
          {{ recipe.name }}
        </router-link>
      </div>
      <div class="d-flex flex-wrap gap-2 mt-auto">
        <router-link
          type="button"
          class="btn btn-dark btn-sm d-print-none"
          aria-label="Edit recipe"
          :to="RouterHelper.editRecipe(recipe)"
          @click.stop
        >
          <FontAwesomeIcon icon="fa-pen" />
        </router-link>
        <RecipeCurrentMealPlanButton class="btn-sm d-print-none" invert :recipe-id="recipe.id" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.image-container {
  position: relative;
  width: 175px;
  aspect-ratio: 1600 / 1200;
  flex-shrink: 0;
  align-self: center;
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

  :deep(.img-placeholder) {
    outline: none;
    border-right: var(--bs-border-width) solid var(--bs-card-border-color);
  }
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
