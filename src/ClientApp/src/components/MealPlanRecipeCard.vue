<script lang="ts" setup>
import type { GetMealPlanResponseRecipe } from '@/api/data-contracts';
import { computed, type PropType } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ImagePlaceholder from './ImagePlaceholder.vue';
import RecipeMealButton from './RecipeMealButton.vue';
import AppSortHandle from './AppSortHandle.vue';
import TagBadge from './TagBadge.vue';

const props = defineProps({
  recipe: { type: Object as PropType<GetMealPlanResponseRecipe>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
});

const emit = defineEmits(['recipeCompleted']);

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);
</script>

<template>
  <div :id="recipeCardId" class="card">
    <div class="card-header position-relative d-md-none d-lg-block d-xl-none">
      <AppSortHandle v-if="props.showSortHandle" class="card-header-sort-handle" />
      <router-link :to="RouterHelper.viewRecipe(recipe)">
        {{ recipe.name }}
      </router-link>
    </div>
    <div class="grid gap-none">
      <div class="g-col-3">
        <router-link
          class="card-link card-hover rounded-start"
          :to="RouterHelper.viewRecipe(recipe)"
        >
          <div class="image-container rounded-start w-100 h-100">
            <img
              v-if="recipe.image != null"
              class="img-fluid position-absolute top-0 left-0 bottom-0 right-0 w-100 h-100 object-fit-cover"
              :class="{ greyscale: recipe.isComplete }"
              :src="ApiHelper.imageUrl(recipe.image)"
              :alt="`Image of ${recipe.name}`"
              :loading="imgLazy ? 'lazy' : 'eager'"
              width="1600"
              height="1200"
            />
            <ImagePlaceholder
              v-else
              class="img-fluid position-absolute top-0 left-0 bottom-0 right-0"
              :class="{ greyscale: recipe.isComplete }"
            />
            <div v-if="recipe.isComplete" class="completion-overlay">
              <font-awesome-icon icon="fa-check" />
            </div>
          </div>
        </router-link>
      </div>
      <div class="g-col-9">
        <div class="card-header position-relative d-none d-md-block d-lg-none d-xl-block">
          <AppSortHandle v-if="props.showSortHandle" class="card-header-sort-handle" />
          <router-link :to="RouterHelper.viewRecipe(recipe)">
            {{ recipe.name }}
          </router-link>
        </div>
        <div class="card-body">
          <div class="btn-toolbar mt-2">
            <span>
              <TagBadge
                v-for="tag in recipe.categories"
                :key="tag.name"
                :tag="tag"
                class="mb-1 me-1"
              />
            </span>
            <button
              :id="`overflowMenuButton-recipe-${recipe.id}`"
              class="btn btn-sm btn-secondary dropdown-toggle ms-auto"
              type="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              More
            </button>
            <div
              class="dropdown-menu p-3"
              :aria-labelledby="`overflowMenuButton-recipe-${recipe.id}`"
            >
              <div>
                <RecipeMealButton class="btn-sm mb-2" :recipe-id="recipe.id" />
              </div>
              <div>
                <router-link
                  type="button"
                  class="btn btn-sm btn-secondary"
                  aria-label="edit recipe"
                  :to="RouterHelper.editRecipe(recipe)"
                  @click.stop
                  >Edit Recipe</router-link
                >
              </div>
            </div>
          </div>
          <div class="mt-3 d-flex justify-content-between align-items-center">
            <div>
              <span v-if="(recipe.mealPlanningSidesCount || 0) > 0"
                >{{ recipe.mealPlanningSidesCount }} side{{
                  recipe.mealPlanningSidesCount !== 1 ? 's' : ''
                }}
                needed.</span
              >
            </div>
            <div class="form-check my-auto">
              <input
                :id="`recipe-complete-${recipe.id}`"
                type="checkbox"
                class="form-check-input"
                :checked="recipe.isComplete"
                @change.stop.prevent="() => emit('recipeCompleted', recipe)"
              />
              <label class="form-check-label" :for="`recipe-complete-${recipe.id}`">Complete</label>
            </div>
          </div>
        </div>
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
  border-right: var(--bs-card-border-width) solid var(--bs-card-border-color);

  .greyscale {
    filter: grayscale(100%);
  }
}

.completion-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(0, 0, 0, 0.3);
  transition: background-color 0.3s ease;
  cursor: pointer;

  svg {
    height: 80%;
    color: white;
    filter: drop-shadow(0 0 3px rgba(0, 0, 0, 0.7));
  }
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
</style>
