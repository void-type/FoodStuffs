<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import { onMounted, computed } from 'vue';
import useAppStore from '@/stores/appStore';
import { storeToRefs } from 'pinia';
import SelectSidebarList from '@/components/SidebarRecipeBase.vue';
import ApiHelpers from '@/models/ApiHelpers';

defineProps({
  routeName: {
    type: String,
    required: true,
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const api = ApiHelpers.client;

const { listResponse, listRequest, recentRecipes } = storeToRefs(recipeStore);

const searchedRecipes = computed(() => {
  const recentIds = recentRecipes.value.map((r) => r.id);
  return (listResponse.value.items || []).filter((r) => !recentIds.includes(r.id));
});

onMounted(() => {
  if (listResponse.value.count === 0) {
    api()
      .recipesList(listRequest.value)
      .then((response) => recipeStore.setListResponse(response.data))
      .catch((response) => appStore.setApiFailureMessages(response));
  }
});
</script>

<template>
  <div>
    <SelectSidebarList :recipes="searchedRecipes" title="Recipes" :route-name="routeName" />
  </div>
</template>

<style lang="scss" scoped></style>
