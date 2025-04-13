<script lang="ts" setup>
import type { GetMealPlanResponseRecipe } from '@/api/data-contracts';
import { computed, type PropType } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ImagePlaceholder from './ImagePlaceholder.vue';
import RecipeMealButton from './RecipeMealButton.vue';
import AppSortHandle from './AppSortHandle.vue';

const props = defineProps({
  recipe: { type: Object as PropType<GetMealPlanResponseRecipe>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
});

const emit = defineEmits(['recipeChecked']);

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);
</script>

<template>
  <div :id="recipeCardId" class="card">
    <div class="card-header position-relative d-md-none">
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
              class="img-fluid"
              :class="{ greyscale: recipe.isComplete }"
              :src="ApiHelper.imageUrl(recipe.image)"
              :alt="`Image of ${recipe.name}`"
              :loading="imgLazy ? 'lazy' : 'eager'"
              width="1600"
              height="1200"
            />
            <ImagePlaceholder
              v-else
              class="img-fluid position-absolute top-0 left-0"
              :class="{ greyscale: recipe.isComplete }"
            />
            <div v-if="recipe.isComplete" class="completion-overlay">
              <font-awesome-icon icon="fa-check" />
            </div>
          </div>
        </router-link>
      </div>
      <div class="g-col-9">
        <div class="card-header position-relative d-none d-md-block">
          <AppSortHandle v-if="props.showSortHandle" class="card-header-sort-handle" />
          <router-link :to="RouterHelper.viewRecipe(recipe)">
            {{ recipe.name }}
          </router-link>
        </div>
        <div class="card-body">
          <div class="btn-toolbar mt-2">
            <router-link
              type="button"
              class="btn btn-sm btn-secondary me-2"
              aria-label="edit recipe"
              :to="RouterHelper.editRecipe(recipe)"
              @click.stop
              >Edit</router-link
            >
            <RecipeMealButton class="btn-sm" :recipe-id="recipe.id" />
          </div>
          <div class="mt-3">
            <div class="form-check my-auto">
              <input
                :id="`recipe-complete-${recipe.id}`"
                type="checkbox"
                class="form-check-input"
                :checked="recipe.isComplete"
                @change.stop.prevent="() => emit('recipeChecked', recipe)"
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

  .greyscale {
    filter: grayscale(100%);
  }

  img {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
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
