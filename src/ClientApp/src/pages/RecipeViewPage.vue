<script lang="ts" setup>
import type { GetRecipeResponse } from '@/api/data-contracts';
import { reactive, watch } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate, useRoute } from 'vue-router';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import RecipeViewer from '@/components/RecipeViewer.vue';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const route = useRoute();
const api = ApiHelper.client;

const data = reactive({
  sourceRecipe: null as GetRecipeResponse | null,
});

function fetchRecipe(id: number) {
  api()
    .recipesGet({ id })
    .then((response) => {
      data.sourceRecipe = response.data;
      RouterHelper.setTitle(route, data.sourceRecipe.name);
      recipeStore.queueRecent(response.data);
    })
    .catch((response) => {
      messageStore.setApiFailureMessages(response);
      data.sourceRecipe = null;
    });
}

watch(
  () => props.id,
  () => {
    fetchRecipe(props.id);
  },
  { immediate: true },
);

onBeforeRouteUpdate((to, from, next) => {
  recipeStore.addToRecent(data.sourceRecipe);
  next();
});

onBeforeRouteLeave((to, from, next) => {
  recipeStore.addToRecent(data.sourceRecipe);
  next();
});
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading :title="data.sourceRecipe?.name" />
    <RecipeViewer v-if="data.sourceRecipe !== null" class="mt-3" :recipe="data.sourceRecipe" />
  </div>
</template>

<style lang="scss" scoped></style>
