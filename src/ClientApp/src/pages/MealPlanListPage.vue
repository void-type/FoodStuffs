<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import { storeToRefs } from 'pinia';
import { computed, watch, type PropType } from 'vue';
import type { LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';
import { toInt, toNumber } from '@/models/FormatHelper';
import Choices from '@/models/Choices';
import router from '@/router';
import ListMealPlansRequest from '@/models/MealPlansListRequest';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import RouterHelper from '@/models/RouterHelper';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const mealPlanStore = useMealPlanStore();

const { useDarkMode } = storeToRefs(appStore);

const { listResponse, listRequest, currentMealPlan } = storeToRefs(mealPlanStore);

async function onDeleteMealPlan(id: number | null | undefined) {
  const parameters: ModalParameters = {
    title: 'Delete meal plan',
    description: 'Do you really want to delete this meal plan?',
    okAction: () => mealPlanStore.deleteMealPlan(id),
  };

  appStore.showModal(parameters);
}

const resultCountText = computed(() => {
  const itemSet = listResponse.value;

  const totalCount = itemSet.totalCount || 0;

  // If NaN or less than 0.
  if (!(totalCount > 0)) {
    return 'Found no meal plans.';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} meal plans.`;
});

function navigateSearch() {
  router.push({
    query: mealPlanStore.currentQueryParams,
  });
}

function changePage(page: number) {
  mealPlanStore.listRequest = { ...listRequest.value, page };

  navigateSearch();
}

function changeTake(take: number) {
  mealPlanStore.listRequest = {
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  };

  navigateSearch();
}

function setListRequestFromQuery() {
  mealPlanStore.listRequest = {
    ...new ListMealPlansRequest(),
    ...props.query,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  };
}

async function newMealPlan() {
  router.push({ name: 'mealPlanNew' });
}

watch(
  props,
  () => {
    setListRequestFromQuery();
    mealPlanStore.fetchMealPlanList();
  },
  { immediate: true }
);
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <div class="mt-3">
      <div class="btn-toolbar">
        <button class="btn btn-secondary" @click="newMealPlan">New</button>
      </div>
    </div>
    <div class="mt-3">{{ resultCountText }}</div>
    <table
      v-if="(listResponse.items?.length || 0) > 0"
      :class="{ 'table mt-4': true, 'table-dark': useDarkMode }"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Current</th>
          <th>Recipes</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="mealPlan in listResponse.items" :key="mealPlan.id">
          <td>
            <router-link :to="RouterHelper.editMealPlan(mealPlan)">{{ mealPlan.name }}</router-link>
          </td>
          <td>
            <div class="form-check">
              <input
                type="radio"
                class="form-check-input form-check-input-lg"
                name="currentMealPlan"
                :checked="currentMealPlan.id === mealPlan.id"
                :aria-label="`Set ${mealPlan.name} as current meal plan`"
                @change="() => mealPlanStore.setCurrentMealPlan(mealPlan.id)"
              />
            </div>
          </td>
          <td>{{ mealPlan.recipeCount }}</td>
          <td>
            <button class="btn btn-sm btn-danger" @click="() => onDeleteMealPlan(mealPlan.id)">
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <EntityTablePager
      v-if="(listResponse.items?.length || 0) > 0"
      :list-request="listRequest"
      :total-count="toInt(listResponse.totalCount)"
      :on-change-page="changePage"
      :on-change-take="changeTake"
    />
  </div>
</template>

<style lang="scss" scoped>
.form-check-input-lg {
  transform: scale(1.3);
}
</style>
