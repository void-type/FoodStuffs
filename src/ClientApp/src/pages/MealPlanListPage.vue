<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import DateHelper from '@/models/DateHelper';
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
  await mealPlanStore.newCurrentMealPlan();
  router.push({ name: 'mealPlanEdit' });
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
      :class="{ 'table mt-3': true, 'table-dark': useDarkMode }"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Created on</th>
          <th>Recipes</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="mealPlan in listResponse.items" :key="mealPlan.id">
          <td>{{ mealPlan.name }}</td>
          <td>{{ DateHelper.dateTimeForView(mealPlan.createdOn) }}</td>
          <td>{{ mealPlan.recipeCount }}</td>
          <td>
            <router-link
              v-if="currentMealPlan.id === mealPlan.id"
              :to="{ name: 'mealPlanEdit' }"
              class="btn btn-sm btn-secondary me-2"
            >
              Edit current
            </router-link>
            <button
              v-else
              class="btn btn-sm btn-primary me-2"
              @click="() => mealPlanStore.setCurrentMealPlan(mealPlan.id)"
            >
              Make current
            </button>
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

<style lang="scss" scoped></style>
