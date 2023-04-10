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
      <span v-if="ingredient.isCategory" class="fw-bold">
        {{ ingredient.name }}
      </span>
      <div v-else>
        <span>{{ ingredient.quantity }}x {{ ingredient.name }}</span>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
