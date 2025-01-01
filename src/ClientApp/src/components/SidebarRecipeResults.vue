<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import { onMounted, computed } from 'vue';
import { storeToRefs } from 'pinia';
import SidebarRecipeBase from '@/components/SidebarRecipeBase.vue';

defineProps({
  routeName: {
    type: String,
    required: true,
  },
});

const recipeStore = useRecipeStore();

const { listResponse, recentRecipes } = storeToRefs(recipeStore);

const searchedRecipes = computed(() => {
  const recentIds = recentRecipes.value.map((r) => r.id);
  return (listResponse.value.items || []).filter((r) => !recentIds.includes(r.id));
});

onMounted(async () => {
  if (listResponse.value.count === 0) {
    await recipeStore.fetchRecipesList();
  }
});
</script>

<template>
  <div>
    <SidebarRecipeBase :recipes="searchedRecipes" title="Search results" :route-name="routeName" />
  </div>
</template>

<style lang="scss" scoped></style>
