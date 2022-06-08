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
    <div v-for="(ing, id) in formattedIngredients" :key="id">
      <h4 v-if="ing.isCategory">
        {{ ing.name }}
      </h4>
      <div v-else>{{ ing.quantity }} {{ ing.name }}</div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
