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
  <div class="card-outer">
    <div
      :class="{ 'card-inner': true, active: card.active }"
      @keydown.enter="onCardClick()"
      @click="onCardClick(card.id)"
    >
      <h3>{{ card.name }}</h3>
      <ul v-if="showIngredients">
        <li v-for="ingredient in card.ingredients" :key="ingredient.name">
          {{ ingredient.quantity }}x {{ ingredient.name }}
        </li>
      </ul>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import '@/styles/cards';

.card-outer {
  padding: 0.5rem;
  width: 33.33%;
}

@media only screen and (max-width: 768px) {
  .card-outer {
    width: 100%;
  }
}

.card-inner {
  outline: $outline;
  padding: 1rem;
  border-radius: 0.2rem;
  height: 100%;

  &.active {
    background-color: $color-active-bg;
  }

  &:hover {
    background-color: $color-hover-bg;
  }

  h3 {
    margin: 0;
    text-align: center;
  }

  ul {
    padding: 0;
    list-style-position: inside;
  }
}
</style>
