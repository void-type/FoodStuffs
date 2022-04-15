<script setup lang="ts">
defineProps({
  title: String,
  ingredients: Array,
  onClear: Function,
  onIngredientClick: Function
});
</script>

<template>
  <div class="list">
    <h2>{{ title }}</h2>
    <button class="btn btn-success" @click="onClear">Clear</button>
    <ul>
      <li
          v-for="[ingredient, quantity] in ingredients"
          :key="ingredient"
          @click="onIngredientClick(ingredient)">{{ quantity }}x {{ ingredient }}</li>
    </ul>
  </div>
</template>

<style scoped lang="scss">
@import "@/App";

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
