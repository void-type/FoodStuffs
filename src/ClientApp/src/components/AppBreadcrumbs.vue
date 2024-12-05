<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import useShoppingItemStore from '@/stores/shoppingItemStore';
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const recipeStore = useRecipeStore();
const mealPlanStore = useMealPlanStore();
const shoppingItemStore = useShoppingItemStore();

const breadcrumbs = computed(() => {
  const matched = route.matched
    .filter((r) => r.meta.title)
    .map((r) => {
      const name = r.name || r.children.find((c) => c.path === '')?.name;

      const routeData = {
        name,
        params: { ...route.params },
        query: {},
      };

      if (name === 'recipeSearch') {
        routeData.query = recipeStore.currentQueryParams;
      } else if (name === 'mealPlanList') {
        routeData.query = mealPlanStore.currentQueryParams;
      } else if (name === 'shoppingItemList') {
        routeData.query = shoppingItemStore.currentQueryParams;
      }

      return {
        routeData,
        title: r.meta.title,
      };
    });

  return matched;
});
</script>

<template>
  <div class="mt-2">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li
          v-for="(segment, index) in breadcrumbs"
          :key="index"
          class="breadcrumb-item"
          :aria-current="index === breadcrumbs.length - 1 ? 'page' : false"
        >
          <router-link v-if="index !== breadcrumbs.length - 1" :to="segment.routeData">{{
            segment.title
          }}</router-link>
          <span v-else>{{ segment.title }}</span>
        </li>
      </ol>
    </nav>
  </div>
</template>

<style lang="scss" scoped></style>
