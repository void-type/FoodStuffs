<script lang="ts" setup>
import { onMounted, watch, reactive } from 'vue';
import { onBeforeRouteLeave, onBeforeRouteUpdate } from 'vue-router';
import type { GetRecipeResponse } from '@/api/data-contracts';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import SelectSidebar from '@/components/SelectSidebar.vue';
import RecipeViewer from '@/components/RecipeViewer.vue';
import GetRecipeResponseClass from '@/models/GetRecipeResponseClass';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const appStore = useAppStore();
const recipeStore = useRecipeStore();

const data = reactive({
  sourceRecipe: new GetRecipeResponseClass() as GetRecipeResponse | null,
});

function fetchRecipe(id: number) {
  new Api()
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
  (newValue) => {
    fetchRecipe(newValue);
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
    <div class="row">
      <h1 class="mt-4 mb-0">{{ data.sourceRecipe?.name || 'Loading...' }}</h1>
      <div class="col-md-12 col-lg-9">
        <RecipeViewer v-if="data.sourceRecipe !== null" :recipe="data.sourceRecipe" />
      </div>
      <div class="col-md-12 col-lg-3 d-print-none mt-4">
        <SelectSidebar :route-name="'view'" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
