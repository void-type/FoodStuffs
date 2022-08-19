<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';

const recipeStore = useRecipeStore();
</script>

<template>
  <ul class="navbar-nav">
    <li class="nav-item">
      <router-link :to="{ name: 'search', query: recipeStore.currentQueryParams }" class="nav-link"
        >Search</router-link
      >
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'new' }" class="nav-link">New</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'meal-cards' }" class="nav-link">Meal Cards</router-link>
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
