<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import useShoppingItemStore from '@/stores/shoppingItemStore';
import useCategoryStore from '@/stores/categoryStore';
import usePantryLocationStore from '@/stores/pantryLocationStore';
import useGroceryDepartmentStore from '@/stores/groceryDepartmentStore';
import RouterHelper from '@/models/RouterHelper';
import { isNil } from '@/models/FormatHelper';
import { computed } from 'vue';
import { storeToRefs } from 'pinia';

const recipeStore = useRecipeStore();
const mealPlanStore = useMealPlanStore();
const shoppingItemStore = useShoppingItemStore();
const categoryStore = useCategoryStore();
const pantryLocationStore = usePantryLocationStore();
const groceryDepartmentStore = useGroceryDepartmentStore();

const planName = computed(() => {
  const name = mealPlanStore.currentMealPlan?.name;

  if (isNil(name)) {
    return '';
  }

  const recipeCount = mealPlanStore.currentRecipes.length;

  return `${name} (${recipeCount})`;
});

const { recentRecipes } = storeToRefs(recipeStore);
</script>

<template>
  <ul class="navbar-nav">
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
            :to="{ name: 'recipeList', query: recipeStore.currentQueryParams }"
            class="dropdown-item"
            >Recipes</router-link
          >
        </li>
        <li><hr class="dropdown-divider" /></li>
        <li>
          <router-link
            :to="{ name: 'mealPlanList', query: mealPlanStore.currentQueryParams }"
            class="dropdown-item"
            >Meal Plans</router-link
          >
        </li>
        <li>
          <router-link
            v-if="(mealPlanStore.currentMealPlan.id || 0) > 0"
            :to="{ name: 'mealPlanEdit' }"
            class="dropdown-item"
            >Edit Current Meal Plan<br /><small>{{ planName }}</small></router-link
          >
        </li>
        <li><hr class="dropdown-divider" /></li>
        <li>
          <router-link
            :to="{ name: 'shoppingItemList', query: shoppingItemStore.currentQueryParams }"
            class="dropdown-item"
            >Grocery Items</router-link
          >
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
        Admin
      </a>
      <ul class="dropdown-menu">
        <li>
          <router-link
            :to="{ name: 'categoryList', query: categoryStore.currentQueryParams }"
            class="dropdown-item"
            >Categories</router-link
          >
        </li>
        <li><hr class="dropdown-divider" /></li>
        <li>
          <router-link
            :to="{
              name: 'groceryDepartmentList',
              query: groceryDepartmentStore.currentQueryParams,
            }"
            class="dropdown-item"
            >Grocery Aisles</router-link
          >
        </li>
        <li><hr class="dropdown-divider" /></li>
        <li>
          <router-link
            :to="{ name: 'pantryLocationList', query: pantryLocationStore.currentQueryParams }"
            class="dropdown-item"
            >Storage Locations</router-link
          >
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
        Recent
      </a>
      <ul class="dropdown-menu">
        <li v-for="recipe in recentRecipes" :key="recipe.id">
          <router-link :to="RouterHelper.viewRecipe(recipe)" class="dropdown-item">{{
            recipe.name
          }}</router-link>
        </li>
      </ul>
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
