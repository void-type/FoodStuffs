<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import useMessageStore from '@/stores/messageStore';
import DateHelpers from '@/models/DateHelpers';
import { storeToRefs } from 'pinia';
import { computed, type PropType } from 'vue';
import type { LocationQuery } from 'vue-router';
import type { ModalParameters } from '@/models/ModalParameters';
import EntityTablePager from '@/components/EntityTablePager.vue';

const props = defineProps({
  query: {
    type: Object as PropType<LocationQuery>,
    required: false,
    default: () => ({}),
  },
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const mealPlanStore = useMealPlanStore();

const { useDarkMode } = storeToRefs(appStore);

const { listResponse, listRequest, currentMealPlan } = storeToRefs(mealPlanStore);

// TODO: make sure to gate deletes behind modal.
// TODO: edit button should disable when current.

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
    return 'No results';
  }

  const base = ((itemSet.page || 0) - 1) * (itemSet.take || 0);
  const start = base + 1;
  const end = base + (itemSet.count || 0);

  return `Showing ${start}-${end} of ${totalCount} results.`;
});

function navigateSearch() {
  router.push({
    query: mealPlanStore.currentQueryParams,
  });
}

function clearSearch() {
  mealPlanStore.setListRequest({
    ...new SearchRecipesRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
  });

  // selectedCategories gets it's new value from query params.

  navigateSearch();
}

function startSearch() {
  mealPlanStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  navigateSearch();
}

function changePage(page: number) {
  mealPlanStore.setListRequest({ ...listRequest.value, page });

  navigateSearch();
}

function changeTake(take: number) {
  mealPlanStore.setListRequest({
    ...listRequest.value,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  });

  navigateSearch();
}

function changeSort(event: Event) {
  const { value } = event.target as HTMLSelectElement;

  mealPlanStore.setListRequest({
    ...listRequest.value,
    sortBy: value,
    page: 1,
  });

  navigateSearch();
}

function setListRequestFromQuery() {
  const categories =
    props.query.categories
      ?.toString()
      ?.split(',')
      .flatMap((x) => {
        const n = toNumberOrNull(x);
        return n ? [n] : [];
      }) || [];

  selectedCategories.value = categories;

  mealPlanStore.setListRequest({
    ...new SearchRecipesRequest(),
    ...props.query,
    categories,
    page: toNumber(Number(props.query.page), 1),
    take: toNumber(Number(props.query.take), Choices.defaultPaginationTake.value),
  });
}
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4">Meal Plans</h1>
    <div>
      <table :class="{ table: true, 'table-dark': useDarkMode }">
        <thead>
          <tr>
            <th>Name</th>
            <th>Created on</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="mealPlan in listResponse.items" :key="mealPlan.id">
            <td>{{ mealPlan.name }}</td>
            <td>{{ DateHelpers.dateTimeForView(mealPlan.createdOn) }}</td>
            <td>
              <button
                class="btn btn-sm btn-primary me-2"
                :disabled="currentMealPlan.id === mealPlan.id"
                @click="() => mealPlanStore.setCurrentMealPlan(mealPlan.id)"
              >
                Edit
              </button>
              <button class="btn btn-sm btn-danger" @click="() => onDeleteMealPlan(mealPlan.id)">
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <div v-if="(listResponse.items || []).length < 1" class="text-center">No meal plans.</div>
    </div>
    <EntityTablePager
      v-if="(listResponse.items?.length || 0) > 0"
      :list-request="listRequest"
      :total-count="toInt(listResponse.totalCount)"
      :on-change-page="changePage"
      :on-change-take="changeTake"
      class="mt-4"
    />
  </div>
</template>

<style lang="scss" scoped></style>
