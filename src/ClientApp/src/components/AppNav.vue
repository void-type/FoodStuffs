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

  const recipeCount = mealPlanStore.currentRecipes.length;

  return `${name} (${recipeCount})`;
});
</script>

<template>
  <ul class="navbar-nav">
    <li class="nav-item">
      <router-link :to="{ name: 'home' }" class="nav-link">Home</router-link>
    </li>
    <li class="nav-item dropdown">
      <a
        class="nav-link dropdown-toggle"
        href="#"
        role="button"
        data-bs-toggle="dropdown"
        aria-expanded="false"
      >
        Recipes
      </a>
      <ul class="dropdown-menu">
        <li>
          <router-link
            :to="{ name: 'recipeSearch', query: recipeStore.currentQueryParams }"
            class="dropdown-item"
            >Search Recipes</router-link
          >
        </li>
        <li>
          <router-link :to="{ name: 'recipeNew' }" class="dropdown-item">New Recipe</router-link>
        </li>
      </ul>
    </li>
    <li class="nav-item dropdown">
      <a
        class="nav-link dropdown-toggle"
        href="#"
        role="button"
        data-bs-toggle="dropdown"
        aria-expanded="false"
      >
        Meal Plans
      </a>
      <ul class="dropdown-menu">
        <li>
          <router-link
            :to="{ name: 'planList', query: mealPlanStore.currentQueryParams }"
            class="dropdown-item"
            >List Meal Plans</router-link
          >
        </li>
        <li>
          <router-link :to="{ name: 'planEdit' }" class="dropdown-item"
            >Current Plan {{ planName }}</router-link
          >
        </li>
      </ul>
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
