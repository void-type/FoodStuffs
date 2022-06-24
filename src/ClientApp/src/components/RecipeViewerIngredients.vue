<script lang="ts" setup>
import type { GetRecipeResponseIngredient } from '@/api/data-contracts';
import { computed, type PropType } from 'vue';

const props = defineProps({
  ingredients: {
    type: Object as PropType<Array<GetRecipeResponseIngredient>>,
    required: true,
  },
});

const formattedIngredients = computed(() => {
  const ingredients = props.ingredients.slice();
  ingredients.sort((a, b) => (a.order || 0) - (b.order || 0));
  return ingredients;
});
</script>

<template>
  <div>
    <div v-for="(ingredient, id) in formattedIngredients" :key="id">
      <div v-if="ingredient.isCategory" class="fw-bold">
        {{ ingredient.name }}
      </div>
      <div v-else class="ms-2">{{ ingredient.quantity }}x {{ ingredient.name }}</div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
