<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import useMealSetStore from '@/stores/mealSetStore';
import { isNil } from '@/models/FormatHelpers';
import { computed } from 'vue';

const recipeStore = useRecipeStore();
const mealSetStore = useMealSetStore();

const mealSetName = computed(() => {
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
      <router-link :to="{ name: 'search', query: recipeStore.currentQueryParams }" class="nav-link"
        >Search</router-link
      >
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'new' }" class="nav-link">New</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'meals' }" class="nav-link">Meals{{ mealSetName }}</router-link>
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
