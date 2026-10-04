<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import RecipeCard from '@/components/RecipeCard.vue';
import useRecipeStore from '@/stores/recipeStore';

const recipeStore = useRecipeStore();
const { recentRecipes } = storeToRefs(recipeStore);
</script>

<template>
  <div v-if="recentRecipes.length > 0" class="mt-4">
    <h2>Recently Viewed</h2>
    <div class="grid">
      <RecipeCard
        v-for="recipe in recentRecipes"
        :key="recipe.id"
        :recipe="recipe"
        class="g-col-6 g-col-md-3"
      />
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
