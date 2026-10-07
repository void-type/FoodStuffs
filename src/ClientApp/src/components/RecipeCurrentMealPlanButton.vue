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
    class="btn"
    :class="props.invert ? 'btn-dark' : 'btn-secondary'"
    :aria-label="`Remove recipe from current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.removeCurrentRecipe(props.recipeId)"
  >
    <FontAwesomeIcon icon="fa-calendar-days" />
    <FontAwesomeIcon icon="fa-minus" size="xs" class="text-danger ms-1" />
  </button>
  <button
    v-else
    type="button"
    class="btn"
    :class="props.invert ? 'btn-dark' : 'btn-secondary'"
    :aria-label="`Add recipe to current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.addCurrentRecipe(props.recipeId)"
  >
    <FontAwesomeIcon icon="fa-calendar-days" />
    <FontAwesomeIcon icon="fa-plus" size="xs" class="ms-1" />
  </button>
</template>

<style lang="scss" scoped></style>
