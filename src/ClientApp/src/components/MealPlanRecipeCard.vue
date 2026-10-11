<script lang="ts" setup>
import type { PropType } from 'vue';
import type { GetMealPlanResponseRecipe } from '@/api/data-contracts';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';
import RecipeCard from './RecipeCard.vue';
import TagBadge from './TagBadge.vue';

const props = defineProps({
  recipe: { type: Object as PropType<GetMealPlanResponseRecipe>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
  isCurrentPlan: { type: Boolean, required: false, default: false },
});

const emit = defineEmits(['recipeCompleted', 'recipeRemoved']);

const hasCategories = computed(() => (props.recipe.categories?.length || 0) > 0);
const hasSides = computed(() => (props.recipe.mealPlanningSidesCount || 0) > 0);
</script>

<template>
  <RecipeCard
    :recipe="recipe"
    :img-lazy="imgLazy"
    :show-sort-handle="showSortHandle"
    :img-class="{ greyscale: recipe.isComplete }"
    :hide-image-expand="recipe.isComplete"
  >
    <template #image-overlay>
      <div v-if="recipe.isComplete" class="completion-overlay">
        <FontAwesomeIcon icon="fa-check" />
      </div>
    </template>

    <div v-if="hasSides" class="small text-muted">
      {{ recipe.mealPlanningSidesCount }} side{{ recipe.mealPlanningSidesCount !== 1 ? 's' : '' }} needed.
    </div>
    <div v-if="hasCategories" class="d-flex flex-wrap gap-1">
      <TagBadge v-for="tag in recipe.categories" :key="tag.name" :tag="tag" />
    </div>

    <template #actions>
      <button
        v-if="!isCurrentPlan"
        type="button"
        class="btn btn-outline-secondary btn-sm d-print-none"
        aria-label="Remove recipe from this plan"
        @click.prevent.stop="() => emit('recipeRemoved', recipe)"
      >
        <FontAwesomeIcon icon="fa-times" />
      </button>
    </template>

    <template #trailing>
      <div class="form-check my-auto">
        <input
          :id="`recipe-complete-${recipe.id}`"
          type="checkbox"
          class="form-check-input"
          :checked="recipe.isComplete"
          @change.stop.prevent="() => emit('recipeCompleted', recipe)"
        >
        <label class="form-check-label" :for="`recipe-complete-${recipe.id}`">Complete</label>
      </div>
    </template>
  </RecipeCard>
</template>

<style lang="scss" scoped>
:deep(.greyscale) {
  filter: grayscale(100%);
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
</style>
