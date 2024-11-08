<script lang="ts" setup>
import { watch, reactive } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate, useRouter } from 'vue-router';
import type { GetRecipeResponse } from '@/api/data-contracts';
import useRecipeStore from '@/stores/recipeStore';
import RecipeViewer from '@/components/RecipeViewer.vue';
import ApiHelpers from '@/models/ApiHelpers';
import RouterHelpers from '@/models/RouterHelpers';
import useMessageStore from '@/stores/messageStore';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const router = useRouter();
const api = ApiHelpers.client;

const data = reactive({
  sourceRecipe: null as GetRecipeResponse | null,
});

function fetchRecipe(id: number) {
  api()
    .recipesGet(id)
    .then((response) => {
      data.sourceRecipe = response.data;
      RouterHelpers.setTitle(router.currentRoute.value, data.sourceRecipe.name);
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
  { immediate: true }
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
    <h1 class="mt-4">{{ data.sourceRecipe?.name }}</h1>
    <div class="grid mt-4">
      <div class="g-col-12">
        <RecipeViewer v-if="data.sourceRecipe !== null" :recipe="data.sourceRecipe" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
