<script lang="ts" setup>
import type { PropType } from 'vue';
import type { Card } from '@/stores/cardStore';

defineProps({
  card: { type: Object as PropType<Card>, required: true },
  showIngredients: { type: Boolean, required: true },
  onCardClick: { type: Function, required: true },
});
</script>

<template>
  <div>
    <div
      class="card card-hover"
      tabindex="0"
      role="button"
      @keydown.enter="onCardClick()"
      @click="onCardClick(card.id)"
    >
      <div class="card-body">
        <h5 class="card-title mb-0">{{ card.name }}</h5>
        <ul v-if="showIngredients" class="mt-3 mb-0">
          <li v-for="ingredient in card.ingredients" :key="ingredient.name">
            {{ ingredient.quantity }}x {{ ingredient.name }}
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
