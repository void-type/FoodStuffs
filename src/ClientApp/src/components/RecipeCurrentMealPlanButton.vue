<script lang="ts" setup>
import { type PropType } from 'vue';
import useMealPlanStore from '@/stores/mealPlanStore';
import { storeToRefs } from 'pinia';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

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
    <font-awesome-icon icon="fa-minus" /> Current Plan
  </button>
  <button
    v-else
    class="btn btn-secondary"
    :aria-label="`Add recipe to current meal plan (${currentMealPlan.name})`"
    @click.stop.prevent="mealPlanStore.addCurrentRecipe(props.recipeId)"
  >
    <font-awesome-icon icon="fa-plus" /> Current Plan
  </button>
</template>

<style lang="scss" scoped></style>
