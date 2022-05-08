<script setup lang="ts">
import { toInt } from '@/models/FormatHelpers';
import useRecipeStore from '@/stores/recipeStore';
import EntityTablePager from '../components/EntityTablePager.vue';

const recipeStore = useRecipeStore();

function changePage(page: number) {
  recipeStore.setListRequest({ ...recipeStore.listRequest, page });
  // fetch
}

function changeTake(take: number) {
  recipeStore.setListRequest({
    ...recipeStore.listRequest,
    take,
    page: 1,
    isPagingEnabled: toInt(take) > 1,
  });
  // fetch
}
</script>

<template>
  <EntityTablePager
    :list-request="recipeStore.listRequest"
    :total-count="toInt(recipeStore.listResponse.totalCount)"
    :on-change-page="changePage"
    :on-change-take="changeTake"
    class="mt-4"
  />
</template>

<style scoped lang="scss"></style>
