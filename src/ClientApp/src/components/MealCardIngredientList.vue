<script lang="ts" setup>
import type { PropType } from 'vue';

defineProps({
  title: { type: String, required: true },
  ingredients: { type: Object as PropType<[string, number][]>, required: true },
  onClear: { type: Function, required: true },
  onIngredientClick: { type: Function, required: true },
});
</script>

<template>
  <div>
    <div class="card">
      <h5 class="card-header d-flex justify-content-between align-items-center">
        {{ title }}
        <button class="btn btn-primary" @click="onClear()">Clear</button>
      </h5>
      <ul class="list-group list-group-flush">
        <li
          v-for="[ingredient, quantity] in ingredients"
          :key="ingredient"
          tabindex="0"
          role="button"
          class="list-group-item card-hover"
          @keydown.stop.prevent.enter="onIngredientClick(ingredient)"
          @click="onIngredientClick(ingredient)"
        >
          {{ quantity }}x {{ ingredient }}
        </li>
        <li v-if="ingredients.length < 1" class="list-group-item">-- Empty --</li>
      </ul>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
