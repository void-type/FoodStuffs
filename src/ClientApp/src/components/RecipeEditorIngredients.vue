<script lang="ts" setup>
import type { GetRecipeResponseIngredient } from '@/api/data-contracts';
import { computed, reactive, watch, type PropType } from 'vue';

const props = defineProps({
  modelValue: {
    type: Object as PropType<Array<GetRecipeResponseIngredient>>,
    required: true,
  },
  isFieldInError: {
    type: Function,
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

watch(
  () => [data.ingredients],
  () => emit('update:modelValue', data.ingredients)
);
</script>

<template>
  <div>
    <!-- TODO: finish ingredients editor. Need editor, new and delete buttons, sortable UX -->
    <div v-for="(ingredient, id) in formattedIngredients" :key="id">
      <div>
        <label :for="`${id}-quantity`" class="form-label">Quantity</label>
        <input
          :id="`${id}-quantity`"
          v-model="ingredient.quantity"
          required
          type="number"
          min="1"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('ingredients') }"
        />
      </div>
      <div>
        <label :for="`${id}-name`" class="form-label">Name</label>
        <input
          :id="`${id}-name`"
          v-model="ingredient.name"
          required
          type="text"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('ingredients') }"
        />
      </div>
      <div class="form-check">
        <input
          :id="`${id}-isCategory`"
          v-model="ingredient.isCategory"
          class="form-check-input"
          type="checkbox"
          :class="{ 'is-invalid': isFieldInError('ingredients') }"
        />
        <label :for="`${id}-isCategory`" class="form-check-label">Is Category</label>
      </div>
      <div>
        <!-- TODO: sortable UI -->
        <label :for="`${id}-order`" class="form-label">Order</label>
        <input
          :id="`${id}-order`"
          v-model="ingredient.order"
          required
          type="number"
          min="1"
          :class="{ 'form-control': true, 'is-invalid': isFieldInError('ingredients') }"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
