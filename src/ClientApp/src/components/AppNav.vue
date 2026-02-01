<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import { computed } from 'vue';
import { isNil } from '@/models/FormatHelper';
import RouterHelper from '@/models/RouterHelper';
import useCategoryStore from '@/stores/categoryStore';
import useGroceryAisleStore from '@/stores/groceryAisleStore';
import useGroceryItemStore from '@/stores/groceryItemStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import useRecipeStore from '@/stores/recipeStore';
import useStorageLocationStore from '@/stores/storageLocationStore';

const recipeStore = useRecipeStore();
const mealPlanStore = useMealPlanStore();
const groceryItemStore = useGroceryItemStore();
const categoryStore = useCategoryStore();
const storageLocationStore = useStorageLocationStore();
const groceryAisleStore = useGroceryAisleStore();

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
          >
            Recipes
          </router-link>
        </li>
        <li><hr class="dropdown-divider"></li>
        <li>
          <router-link
            :to="{ name: 'mealPlanList', query: mealPlanStore.currentQueryParams }"
            class="dropdown-item"
          >
            Meal Plans
          </router-link>
        </li>
        <li>
          <router-link
            v-if="(mealPlanStore.currentMealPlan.id || 0) > 0"
            :to="{
              name: 'mealPlanEdit',
              params: { id: mealPlanStore.currentMealPlan.id },
              query: mealPlanStore.currentQueryParams,
            }"
            class="dropdown-item"
          >
            Edit Current Meal Plan<br><small>{{ planName }}</small>
          </router-link>
        </li>
        <li><hr class="dropdown-divider"></li>
        <li>
          <router-link
            :to="{ name: 'groceryItemList', query: groceryItemStore.currentQueryParams }"
            class="dropdown-item"
          >
            Grocery Items
          </router-link>
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
          >
            Categories
          </router-link>
        </li>
        <li><hr class="dropdown-divider"></li>
        <li>
          <router-link
            :to="{
              name: 'groceryAisleList',
              query: groceryAisleStore.currentQueryParams,
            }"
            class="dropdown-item"
          >
            Grocery Aisles
          </router-link>
        </li>
        <li><hr class="dropdown-divider"></li>
        <li>
          <router-link
            :to="{ name: 'storageLocationList', query: storageLocationStore.currentQueryParams }"
            class="dropdown-item"
          >
            Storage Locations
          </router-link>
        </li>
      </ul>
    </li>
    <li v-if="recentRecipes.length > 0" class="nav-item dropdown">
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
        <template v-for="(recipe, index) in recentRecipes" :key="recipe.id">
          <li>
            <router-link :to="RouterHelper.viewRecipe(recipe)" class="dropdown-item">
              {{
                recipe.name
              }}
            </router-link>
          </li>
          <li v-if="index < recentRecipes.length - 1">
            <hr class="dropdown-divider">
          </li>
        </template>
      </ul>
    </li>
  </ul>
</template>

<style lang="scss" scoped></style>
