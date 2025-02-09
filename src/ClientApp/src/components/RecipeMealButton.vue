<script lang="ts" setup>
import { type PropType } from 'vue';
import useMealPlanStore from '@/stores/mealPlanStore';
import { storeToRefs } from 'pinia';

const props = defineProps({
  recipeId: {
    type: Number as PropType<number | null | undefined>,
    required: true,
  },
});

const mealPlanStore = useMealPlanStore();
const { currentMealPlan } = storeToRefs(mealPlanStore);
</script>

<template>
  <button
    v-if="mealPlanStore.currentRecipesContains(props.recipeId)"
    class="btn btn-secondary"
    :aria-label="`Remove recipe from current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.removeCurrentRecipe(props.recipeId)"
  >
    Plan: Remove
  </button>
  <button
    v-else
    class="btn btn-secondary"
    :aria-label="`Add recipe to current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.addCurrentRecipe(props.recipeId)"
  >
    Plan: Add
  </button>
</template>

<style lang="scss" scoped>
div.form-control-plaintext {
  white-space: pre-wrap;
}

div.carousel-item img,
.img-placeholder-wrapper {
  max-width: 27rem;
}
</style>
