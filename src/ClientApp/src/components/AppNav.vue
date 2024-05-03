<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import useMealSetStore from '@/stores/mealSetStore';
import { isNil } from '@/models/FormatHelpers';
import { computed } from 'vue';

const recipeStore = useRecipeStore();
const mealSetStore = useMealSetStore();

const planName = computed(() => {
  const name = mealSetStore.currentMealSet?.name;

  if (isNil(name)) {
    return '';
  }

  return ` (${name})`;
});
</script>

<template>
  <ul class="navbar-nav">
    <li class="nav-item">
      <router-link :to="{ name: 'home' }" class="nav-link">Home</router-link>
    </li>
    <li class="nav-item">
      <router-link
        :to="{ name: 'recipeSearch', query: recipeStore.currentQueryParams }"
        class="nav-link"
        >Search Recipes</router-link
      >
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'recipeNew' }" class="nav-link">New Recipe</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'planSearch' }" class="nav-link">Plans</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'planEdit' }" class="nav-link">Edit {{ planName }}</router-link>
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
