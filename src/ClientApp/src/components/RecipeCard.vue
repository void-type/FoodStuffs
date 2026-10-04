<script lang="ts" setup>
import type { PropType } from 'vue';
import type { SearchRecipesResultItem } from '@/api/data-contracts';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useImageLightboxStore from '@/stores/imageLightboxStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import AppSortHandle from './AppSortHandle.vue';
import ImagePlaceholder from './ImagePlaceholder.vue';
import TagBadge from './TagBadge.vue';

const props = defineProps({
  recipe: { type: Object as PropType<SearchRecipesResultItem>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
  showCompactView: { type: Boolean, required: false, default: false },
});

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);
const imageLightboxStore = useImageLightboxStore();
const mealPlanStore = useMealPlanStore();

const isInCurrentMealPlan = computed(() => mealPlanStore.currentRecipesContains(props.recipe.id));

const mealPlanButtonLabel = computed(() =>
  isInCurrentMealPlan.value
    ? `Remove recipe from current meal plan (${mealPlanStore.currentMealPlan.name})`
    : `Add recipe to current meal plan (${mealPlanStore.currentMealPlan.name})`);

function toggleCurrentMealPlan() {
  if (isInCurrentMealPlan.value) {
    mealPlanStore.removeCurrentRecipe(props.recipe.id);
  } else {
    mealPlanStore.addCurrentRecipe(props.recipe.id);
  }
}

function flipCard() {
  const card = document.getElementById(recipeCardId.value);

  const front = card?.querySelector('.card-flip-front');
  front?.classList.toggle('invisible');

  const back = card?.querySelector('.card-flip-back');
  back?.classList.toggle('d-none');
}
</script>

<template>
  <div :id="recipeCardId" class="card">
    <div class="card-header position-relative">
      <AppSortHandle v-if="props.showSortHandle" class="card-header-sort-handle" />
      <router-link :to="RouterHelper.viewRecipe(recipe)">
        {{ recipe.name }}
      </router-link>
    </div>
    <div v-if="!props.showCompactView" class="card-flip-container">
      <div class="card-flip-front">
        <div class="image-container">
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
            <ImagePlaceholder v-else class="img-fluid position-absolute top-0 left-0" />
          </router-link>
          <router-link
            type="button"
            class="btn btn-dark btn-sm position-absolute top-0 start-0 m-1 opacity-75 d-print-none"
            aria-label="Edit recipe"
            :to="RouterHelper.editRecipe(recipe)"
            @click.stop
          >
            <FontAwesomeIcon icon="fa-pen" />
          </router-link>
          <button
            type="button"
            class="btn btn-dark btn-sm position-absolute bottom-0 start-0 m-1 opacity-75 d-print-none"
            :aria-label="mealPlanButtonLabel"
            @click.stop.prevent="toggleCurrentMealPlan"
          >
            <FontAwesomeIcon icon="fa-calendar-days" />
            <span
              class="badge rounded-pill position-absolute top-0 start-100 translate-middle"
              :class="isInCurrentMealPlan ? 'text-bg-danger' : 'text-bg-secondary'"
            >
              <FontAwesomeIcon :icon="isInCurrentMealPlan ? 'fa-minus' : 'fa-plus'" />
            </span>
          </button>
          <button
            type="button"
            class="btn btn-dark btn-sm position-absolute top-0 end-0 m-1 opacity-75 d-print-none"
            aria-label="Flip card"
            @click.stop.prevent="flipCard"
          >
            <FontAwesomeIcon icon="fa-rotate" />
          </button>
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
      </div>
      <div class="card-flip-back card-body d-none">
        <button
          type="button"
          class="btn btn-dark btn-sm position-absolute top-0 end-0 m-1 opacity-75 d-print-none"
          aria-label="Flip back"
          @click.stop.prevent="flipCard"
        >
          <FontAwesomeIcon icon="fa-rotate" />
        </button>
        <div class="card-flip-back-inner slim-scroll">
          <div v-if="(recipe.groceryItems?.length || 0) > 0">
            <div>Grocery Items</div>
            <ul>
              <li v-for="groceryItem in recipe.groceryItems" :key="groceryItem.name || ''">
                {{ groceryItem.quantity }}x {{ groceryItem.name }}
              </li>
            </ul>
          </div>
          <div v-if="(recipe.mealPlanningSidesCount || 0) > 0" class="mb-3">
            <div>
              {{ recipe.mealPlanningSidesCount }} side{{
                recipe.mealPlanningSidesCount !== 1 ? 's' : ''
              }}
              needed.
            </div>
          </div>
          <div v-if="(recipe.categories?.length || 0) > 0">
            <TagBadge
              v-for="category in recipe.categories"
              :key="category.name || ''"
              class="m-1"
              :tag="category"
            />
          </div>
        </div>
      </div>
    </div>
    <div v-else class="card-body position-relative">
      <router-link
        type="button"
        class="btn btn-dark btn-sm position-absolute top-0 start-0 m-1 opacity-75 d-print-none"
        aria-label="Edit recipe"
        :to="RouterHelper.editRecipe(recipe)"
        @click.stop
      >
        <FontAwesomeIcon icon="fa-pen" />
      </router-link>
      <button
        type="button"
        class="btn btn-dark btn-sm position-absolute bottom-0 start-0 m-1 opacity-75 d-print-none"
        :aria-label="mealPlanButtonLabel"
        @click.stop.prevent="toggleCurrentMealPlan"
      >
        <FontAwesomeIcon icon="fa-calendar-days" />
        <span
          class="badge rounded-pill position-absolute top-0 start-100 translate-middle"
          :class="isInCurrentMealPlan ? 'text-bg-danger' : 'text-bg-secondary'"
        >
          <FontAwesomeIcon :icon="isInCurrentMealPlan ? 'fa-minus' : 'fa-plus'" />
        </span>
      </button>
      <div v-if="(recipe.categories?.length || 0) > 0" class="compact-categories">
        <TagBadge
          v-for="category in recipe.categories"
          :key="category.name || ''"
          class="mb-1 me-1"
          :tag="category"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.image-container {
  position: relative;
  width: 100%;
  height: 0;
  padding-top: calc(1200 / 1600 * 100%);
  overflow: hidden;

  img {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
}

// Reserve space so the edit/meal-plan corner buttons don't sit on top of the category badges.
.compact-categories {
  padding-left: 2.5rem;
}

.card-header {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;

  &:has(.card-header-sort-handle) {
    padding-left: 2.4rem;
  }

  .card-header-sort-handle {
    padding: var(--bs-card-cap-padding-y) 0.75rem;
    display: inline-block;
    z-index: 2;

    position: absolute;
    top: 0;
    left: 0;
  }
}

.card-flip-container {
  position: relative;
}

.card-flip-back {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
}

.card-flip-back-inner {
  height: 100%;
  width: 100%;
}
</style>
