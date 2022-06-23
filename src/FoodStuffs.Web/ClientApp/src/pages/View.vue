<script lang="ts" setup>
import { onMounted, watch, reactive } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate } from 'vue-router';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import SelectSidebar from '@/components/SelectSidebar.vue';
import RecipeViewer from '@/components/RecipeViewer.vue';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();

const data = reactive({
  sourceRecipe: null as GetRecipeResponse | null,
});

function fetchRecipe() {
  new Api()
    .recipesDetail(props.id)
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
    fetchRecipe();
  }
);

onMounted(() => {
  fetchRecipe();
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
    <div class="row">
      <div v-if="data.sourceRecipe !== null" class="col-md-12 col-lg-9 mt-4">
        <h1>{{ data.sourceRecipe.name }}</h1>
        <RecipeViewer class="mt-4" :recipe="data.sourceRecipe" />
      </div>
      <div class="col-md-12 col-lg-3 d-print-none mt-4">
        <SelectSidebar :route-name="'view'" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
