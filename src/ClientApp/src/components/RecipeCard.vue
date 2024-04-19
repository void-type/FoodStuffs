<script lang="ts" setup>
import type { RecipeSearchResultItem } from '@/api/data-contracts';
import { computed, type PropType } from 'vue';
import useMealStore from '@/stores/mealStore';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelpers from '@/models/ApiHelpers';
import RouterHelpers from '@/models/RouterHelpers';
import ImagePlaceholder from './ImagePlaceholder.vue';

const props = defineProps({
  recipe: { type: Object as PropType<RecipeSearchResultItem>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
});

const mealStore = useMealStore();

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);

function flipCard() {
  const card = document.getElementById(recipeCardId.value);

  const front = card?.querySelector('.card-flip-front');
  front?.classList.toggle('invisible');

  const back = card?.querySelector('.card-flip-back');
  back?.classList.toggle('d-none');
}
</script>

<template>
  <div :id="recipeCardId" class="card card-hover">
    <router-link class="card-link" :to="RouterHelpers.viewRecipe(recipe)">
      <div class="card-header">{{ recipe.name }}</div>
      <div class="card-floating-toolbar">
        <a class="btn-card-control" href="#" aria-label="flip card" @click.stop.prevent="flipCard">
          <font-awesome-icon class="me-2" icon="fa-rotate" />
        </a>
      </div>
      <div class="card-flip-container">
        <div class="card-flip-front">
          <div class="image-container">
            <img
              v-if="recipe.image != null"
              class="img-fluid rounded-bottom"
              :src="ApiHelpers.imageUrl(recipe.image)"
              :alt="`Image of ${recipe.name}`"
              :loading="imgLazy ? 'lazy' : 'eager'"
              width="1600"
              height="1200"
            />
            <ImagePlaceholder v-else class="img-fluid rounded-bottom position-absolute" />
          </div>
        </div>
        <div class="card-flip-back p-3 d-none">
          <div class="card-flip-back-inner slim-scroll">
            <div class="btn-toolbar">
              <router-link
                type="button"
                class="btn btn-sm btn-secondary me-2"
                aria-label="edit recipe"
                :to="RouterHelpers.editRecipe(recipe)"
                @click.stop.prevent
                >Edit</router-link
              >
              <button
                class="btn btn-sm btn-secondary"
                aria-label="Add recipe to current meal plan"
                @click.stop.prevent="mealStore.addToMealPlan(recipe.id)"
              >
                Add meal
              </button>
            </div>
            <div v-if="recipe.ingredients?.some((x) => x.isCategory === false)" class="mt-3">
              <div>Ingredients</div>
              <ul>
                <li
                  v-for="ingredient in recipe.ingredients?.filter((x) => x.isCategory === false)"
                  :key="ingredient.name || ''"
                >
                  {{ ingredient.quantity }}x {{ ingredient.name }}
                </li>
              </ul>
            </div>
            <div v-if="(recipe.categories?.length || 0) > 0" class="mt-3">
              <span
                v-for="category in recipe.categories"
                :key="category || ''"
                class="badge text-bg-secondary me-2 mt-2"
              >
                {{ category }}</span
              >
            </div>
          </div>
        </div>
      </div>
    </router-link>
  </div>
</template>

<style lang="scss" scoped>
.image-container {
  position: relative;
  width: 100%;
  height: 0;
  padding-top: calc(1200 / 1600 * 100%);
  overflow: hidden;
}

.image-container img {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card-header {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  padding-right: 2.75rem;
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
