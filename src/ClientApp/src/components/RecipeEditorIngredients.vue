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
    <div class="accordion" id="ingredient-accordion">
      <div class="accordion-item" v-for="(ingredient, id) in formattedIngredients" :key="id">
        <h2 class="accordion-header" :id="`ingredient-${id}-accordion-header`">
          <!-- TODO: --bs-accordion-btn-icon should be white for dark mode -->
          <button
            class="accordion-button collapsed"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="`#ingredient-${id}-accordion-collapse`"
            aria-expanded="false"
            :aria-controls="`ingredient-${id}-accordion-collapse`"
          >
            <span v-if="ingredient.isCategory" class="fw-bold">{{ ingredient.name }}</span>
            <span v-else>{{ ingredient.quantity }}x {{ ingredient.name }}</span>
          </button>
        </h2>
        <div
          :id="`ingredient-${id}-accordion-collapse`"
          class="accordion-collapse collapse"
          :aria-labelledby="`ingredient-${id}-accordion-header`"
          data-bs-parent="#ingredient-accordion"
        >
          <div class="grid p-3">
            <div class="g-col-12 g-col-md-12">
              <label :for="`ingredient-${id}-name`" class="form-label">Name</label>
              <input
                :id="`ingredient-${id}-name`"
                v-model="ingredient.name"
                required
                type="text"
                :class="{
                  'form-control': true,
                  'is-invalid': isFieldInError('ingredients'),
                }"
              />
            </div>
            <div class="g-col-12 g-col-md-4">
              <label :for="`ingredient-${id}-quantity`" class="form-label">Quantity</label>
              <input
                :id="`ingredient-${id}-quantity`"
                v-model="ingredient.quantity"
                required
                type="number"
                min="1"
                :class="{
                  'form-control': true,
                  'is-invalid': isFieldInError('ingredients'),
                }"
              />
            </div>
            <div class="g-col-12 g-col-md-4">
              <label :for="`ingredient-${id}-order`" class="form-label">Order</label>
              <input
                :id="`ingredient-${id}-order`"
                v-model="ingredient.order"
                required
                type="number"
                min="1"
                :class="{
                  'form-control': true,
                  'is-invalid': isFieldInError('ingredients'),
                }"
              />
            </div>
            <div class="g-col-12">
              <div class="form-check">
                <input
                  :id="`ingredient-${id}-isCategory`"
                  v-model="ingredient.isCategory"
                  class="form-check-input"
                  type="checkbox"
                  :class="{ 'is-invalid': isFieldInError('ingredients') }"
                />
                <label :for="`ingredient-${id}-isCategory`" class="form-check-label"
                  >Is Category</label
                >
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.accordion-button {
  padding: 0.75em;
}
</style>
