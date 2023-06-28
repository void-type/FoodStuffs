<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import { onMounted, computed } from 'vue';
import { storeToRefs } from 'pinia';
import SelectSidebarList from '@/components/SidebarRecipeBase.vue';
import ApiHelpers from '@/models/ApiHelpers';
import useMessageStore from '@/stores/messageStore';

defineProps({
  routeName: {
    type: String,
    required: true,
  },
});

const messageStore = useMessageStore();
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
      .catch((response) => messageStore.setApiFailureMessages(response));
  }
});
</script>

<template>
  <div>
    <SelectSidebarList :recipes="searchedRecipes" title="Search results" :route-name="routeName" />
  </div>
</template>

<style lang="scss" scoped></style>
