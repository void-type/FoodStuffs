<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';
import useCategoryStore from '@/stores/categoryStore';
import useGroceryAisleStore from '@/stores/groceryAisleStore';
import useGroceryItemStore from '@/stores/groceryItemStore';
import useGroceryStoreStore from '@/stores/groceryStoreStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import useRecipeStore from '@/stores/recipeStore';
import useStorageLocationStore from '@/stores/storageLocationStore';

const recipeStore = useRecipeStore();
const mealPlanStore = useMealPlanStore();
const groceryItemStore = useGroceryItemStore();
const categoryStore = useCategoryStore();
const groceryStoreStore = useGroceryStoreStore();
const storageLocationStore = useStorageLocationStore();
const groceryAisleStore = useGroceryAisleStore();

const hasCurrentMealPlan = computed(() => (mealPlanStore.currentMealPlan.id || 0) > 0);
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
            :to="{ name: 'groceryStoreList', query: groceryStoreStore.currentQueryParams }"
            class="dropdown-item"
          >
            Grocery Stores
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
    <li class="nav-item current-meal-plan-nav-item">
      <router-link
        v-if="hasCurrentMealPlan"
        :to="{
          name: 'mealPlanEdit',
          params: { id: mealPlanStore.currentMealPlan.id },
          query: mealPlanStore.currentQueryParams,
        }"
        class="nav-link"
        :title="mealPlanStore.currentMealPlan.name"
      >
        <FontAwesomeIcon icon="fa-calendar-days" class="me-1" /><span class="current-meal-plan-name">{{ mealPlanStore.currentMealPlan.name }}</span>
      </router-link>
      <router-link
        v-else
        :to="{ name: 'mealPlanList', query: mealPlanStore.currentQueryParams }"
        class="nav-link"
      >
        <FontAwesomeIcon icon="fa-calendar-days" class="me-1" />Select Meal Plan
      </router-link>
    </li>
  </ul>
</template>

<style lang="scss" scoped>
// Keep long meal plan names from wrapping the navbar to multiple lines.
.current-meal-plan-nav-item {
  min-width: 0;

  .nav-link {
    display: flex;
    align-items: center;
  }

  .current-meal-plan-name {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    max-width: 12rem;
  }
}
</style>
