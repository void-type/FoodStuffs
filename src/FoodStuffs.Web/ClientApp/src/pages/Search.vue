<script setup lang="ts">
import useRecipeStore from '@/stores/recipeStore';
import EntityTablePager from '../components/EntityTablePager.vue';

const recipeStore = useRecipeStore();

function changePage(page: number | null) {
  console.log(page);
}

function changeTake(take: number | null) {
  console.log(take);
}
</script>

<template>
  <EntityTablePager
    :list-response="recipeStore.listResponse"
    :list-request="recipeStore.listRequest"
    :on-change-page="changePage"
    :on-change-take="changeTake"
  />
</template>

<style scoped lang="scss">
.area {
  display: flex;
  flex-wrap: wrap;
  padding: 0.5rem;
}
</style>
