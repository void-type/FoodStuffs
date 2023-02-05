<script lang="ts" setup>
import type { GetRecipeResponseIngredient } from '@/api/data-contracts';
import { computed, reactive, type PropType } from 'vue';

const props = defineProps({
  modelValue: {
    type: Object as PropType<Array<GetRecipeResponseIngredient>>,
    required: true,
  },
});

const emit = defineEmits(['update:modelValue']);
// TODO: finish this editor
const data = reactive({
  ingredients: props.modelValue,
});

const formattedIngredients = computed(() => {
  const ingredients = props.modelValue.slice();
  ingredients.sort((a, b) => (a.order || 0) - (b.order || 0));
  return ingredients;
});

function emitInput() {
  emit('update:modelValue', data.ingredients);
}
</script>

<template>
  <div>
    <div v-for="(ingredient, id) in formattedIngredients" :key="id">
      <div v-if="ingredient.isCategory" class="fw-bold mt-1">
        {{ ingredient.name }}
      </div>
      <div v-else>
        <span class="ingredient-quantity">{{ ingredient.quantity }}x</span>{{ ingredient.name }}
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.ingredient-quantity {
  margin-right: 1em;
}
</style>
