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
  <div class="list">
    <h2>{{ title }}</h2>
    <button class="btn btn-primary" @click="onClear()">Clear</button>
    <ul>
      <li
        v-for="[ingredient, quantity] in ingredients"
        :key="ingredient"
        @keydown.enter="onIngredientClick(ingredient)"
        @click="onIngredientClick(ingredient)"
      >
        {{ quantity }}x {{ ingredient }}
      </li>
    </ul>
  </div>
</template>

<style lang="scss" scoped>
@import '@/styles/cards';

.list {
  flex: 1;
  flex-wrap: wrap;
  margin: 0.5rem;

  ul {
    padding-left: 0;
    margin-top: 1rem;

    li {
      text-decoration: none;
      list-style: none;
      outline: $outline;
      padding: 0.25rem;
      border-radius: 0.2rem;
      margin-bottom: 0.5rem;

      &:hover {
        background-color: $color-hover-bg;
      }
    }
  }
}
</style>
