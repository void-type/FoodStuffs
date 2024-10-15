<script lang="ts" setup>
import type { SearchRecipesResultItem } from '@/api/data-contracts';
import { computed, type PropType } from 'vue';
import RouterHelpers from '@/models/RouterHelpers';
import RecipeMealButton from './RecipeMealButton.vue';

const props = defineProps({
  recipe: { type: Object as PropType<SearchRecipesResultItem>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
});

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);
</script>

<template>
  <div :id="recipeCardId" class="card card-hover">
    <router-link class="card-link" :to="RouterHelpers.viewRecipe(recipe)">
      <div class="card-body">
        <div class="card-title h5">{{ recipe.name }}</div>
        <div class="btn-toolbar mt-4">
          <router-link
            type="button"
            class="btn btn-sm btn-secondary me-2"
            aria-label="edit recipe"
            :to="RouterHelpers.editRecipe(recipe)"
            @click.stop.prevent
            >Edit</router-link
          >
          <RecipeMealButton class="btn-sm" :recipe-id="recipe.id" />
        </div>
        <div v-if="(recipe.categories?.length || 0) > 0" class="mt-3">
          <span
            v-for="category in recipe.categories"
            :key="category || ''"
            class="badge text-bg-secondary me-2 mt-2"
          >
            {{ category }}</span
          >
        </div>
      </div>
    </router-link>
  </div>
</template>

<style lang="scss" scoped></style>
