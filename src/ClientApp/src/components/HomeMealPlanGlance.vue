<script lang="ts" setup>
import type { HttpResponse } from '@/api/http-client';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { storeToRefs } from 'pinia';
import { computed, onMounted, ref } from 'vue';
import RecipeCard from '@/components/RecipeCard.vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useMealPlanStore from '@/stores/mealPlanStore';
import useMessageStore from '@/stores/messageStore';

const api = ApiHelper.client;
const mealPlanStore = useMealPlanStore();
const messageStore = useMessageStore();

const { currentMealPlan, currentRecipes } = storeToRefs(mealPlanStore);

// Mirror MealPlanEditPage's display order: incomplete recipes (in plan order) first, then completed.
const orderedCurrentRecipes = computed(() => {
  const incomplete = currentRecipes.value.filter(recipe => !recipe.isComplete);
  const completed = currentRecipes.value.filter(recipe => recipe.isComplete);
  return [...incomplete, ...completed];
});

const latestMealPlanId = ref<number | null>(null);
const latestMealPlanName = ref('');

const hasCurrentMealPlan = computed(() => (currentMealPlan.value.id || 0) > 0);

const isOnLatestMealPlan = computed(
  () => latestMealPlanId.value === null || currentMealPlan.value.id === latestMealPlanId.value,
);

async function fetchLatestMealPlan() {
  try {
    const response = await api().mealPlansList({ page: 1, take: 1, isPagingEnabled: true });
    const latest = response.data.items?.[0];
    latestMealPlanId.value = latest?.id || null;
    latestMealPlanName.value = latest?.name || '';
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function switchToLatestMealPlan() {
  if (latestMealPlanId.value === null) {
    return;
  }

  await mealPlanStore.setCurrentMealPlan(latestMealPlanId.value);
}

onMounted(fetchLatestMealPlan);
</script>

<template>
  <div v-if="hasCurrentMealPlan || (latestMealPlanId !== null && !isOnLatestMealPlan)" class="mt-4">
    <div
      v-if="latestMealPlanId !== null && !isOnLatestMealPlan"
      class="alert alert-warning d-flex flex-wrap align-items-center justify-content-between gap-2 mb-3"
      role="alert"
    >
      <span>
        <FontAwesomeIcon icon="fa-calendar-days" class="me-1" />
        You're not on the latest meal plan ("{{ latestMealPlanName }}").
      </span>
      <button type="button" class="btn btn-sm btn-warning" @click="switchToLatestMealPlan">
        Switch to Latest
      </button>
    </div>
    <h2 v-if="hasCurrentMealPlan">
      Current Meal Plan
    </h2>
    <p v-if="hasCurrentMealPlan">
      <router-link :to="RouterHelper.editMealPlan(currentMealPlan)">
        <FontAwesomeIcon icon="fa-calendar-days" class="me-2" />
        {{ currentMealPlan.name }}
      </router-link>
    </p>
    <div v-if="currentRecipes.length > 0" class="grid">
      <RecipeCard
        v-for="recipe in orderedCurrentRecipes"
        :key="recipe.id"
        :recipe="recipe"
        class="g-col-6 g-col-md-3"
      />
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
