<script lang="ts" setup>
import { onMounted, watch, computed } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate } from 'vue-router';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();

let sourceRecipeBase: GetRecipeResponse | null = null;
const sourceRecipe = computed(() => sourceRecipeBase);

function fetchRecipe() {
  new Api()
    .recipesDetail(props.id)
    .then((response) => {
      sourceRecipeBase = response.data;
    })
    .catch((response) => {
      appStore.setApiFailureMessages(response);
      sourceRecipeBase = null;
    });
}

watch(
  () => props.id,
  () => {
    fetchRecipe();
  }
);

onMounted(() => {
  fetchRecipe();
});

onBeforeRouteUpdate((to, from, next) => {
  recipeStore.addToRecent(sourceRecipe.value);
  next();
});

onBeforeRouteLeave((to, from, next) => {
  recipeStore.addToRecent(sourceRecipe.value);
  next();
});
</script>

<template>
  <div class="container-lg">
    <div class="row">
      <div class="col-md-12 col-lg-3 d-print-none mt-4">
        <SelectSidebar :route-name="'view'" />
      </div>
      <div v-if="sourceRecipe !== null" class="col mt-4">
        <h1>{{ sourceRecipe.name }}</h1>
        <RecipeViewer class="mt-4" :recipe="sourceRecipe" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
