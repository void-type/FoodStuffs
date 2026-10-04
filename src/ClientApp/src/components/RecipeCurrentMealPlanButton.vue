<script lang="ts" setup>
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { storeToRefs } from 'pinia';
import useMealPlanStore from '@/stores/mealPlanStore';

const props = defineProps({
  recipeId: {
    type: Number as PropType<number | null | undefined>,
    required: true,
  },
  invert: {
    type: Boolean,
    required: false,
    default: false,
  },
});

const mealPlanStore = useMealPlanStore();
const { currentMealPlan } = storeToRefs(mealPlanStore);
</script>

<template>
  <button
    v-if="mealPlanStore.currentRecipesContains(props.recipeId)"
    type="button"
    class="btn position-relative"
    :class="props.invert ? 'btn-dark' : 'btn-secondary'"
    :aria-label="`Remove recipe from current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.removeCurrentRecipe(props.recipeId)"
  >
    <FontAwesomeIcon icon="fa-calendar-days" />
    <span class="badge rounded-pill text-bg-danger position-absolute top-0 start-100 translate-middle">
      <FontAwesomeIcon icon="fa-minus" />
    </span>
  </button>
  <button
    v-else
    type="button"
    class="btn position-relative"
    :class="props.invert ? 'btn-dark' : 'btn-secondary'"
    :aria-label="`Add recipe to current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.addCurrentRecipe(props.recipeId)"
  >
    <FontAwesomeIcon icon="fa-calendar-days" />
    <span class="badge rounded-pill text-bg-secondary position-absolute top-0 start-100 translate-middle">
      <FontAwesomeIcon icon="fa-plus" />
    </span>
  </button>
</template>

<style lang="scss" scoped></style>
