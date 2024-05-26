<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import { isNil } from '@/models/FormatHelpers';
import { computed } from 'vue';

const recipeStore = useRecipeStore();
const mealPlanStore = useMealPlanStore();

const planName = computed(() => {
  const name = mealPlanStore.currentMealPlan?.name;

  if (isNil(name)) {
    return '';
  }

  return `(${name})`;
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
      <router-link :to="{ name: 'planSearch' }" class="nav-link">Meal Plans</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'planEdit' }" class="nav-link"
        >Edit Plan {{ planName }}</router-link
      >
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
