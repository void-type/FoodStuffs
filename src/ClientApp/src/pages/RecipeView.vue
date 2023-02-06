<script lang="ts" setup>
import { onMounted, watch, reactive } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate } from 'vue-router';
import type { GetRecipeResponse } from '@/api/data-contracts';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import SelectSidebar from '@/components/SelectSidebar.vue';
import RecipeViewer from '@/components/RecipeViewer.vue';
import ApiHelpers from '@/models/ApiHelpers';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const api = ApiHelpers.client;

const data = reactive({
  sourceRecipe: null as GetRecipeResponse | null,
});

function fetchRecipe(id: number) {
  api()
    .recipesDetail(id)
    .then((response) => {
      data.sourceRecipe = response.data;
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
      data.sourceRecipe = null;
    });
}

watch(
  () => props.id,
  () => {
    fetchRecipe(props.id);
  }
);

onMounted(() => {
  fetchRecipe(props.id);
});

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
    <h1 class="mt-4 mb-4">{{ data.sourceRecipe?.name }}</h1>
    <div class="grid">
      <div class="g-col-12 g-col-lg-9 mt-4">
        <RecipeViewer v-if="data.sourceRecipe !== null" :recipe="data.sourceRecipe" />
      </div>
      <div class="g-col-12 g-col-lg-3 d-print-none mt-4">
        <SelectSidebar :route-name="'view'" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
