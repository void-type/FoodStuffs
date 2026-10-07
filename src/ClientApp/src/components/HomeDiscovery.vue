<script lang="ts" setup>
import type { SearchRecipesResultItem } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import { onMounted, ref } from 'vue';
import RecipeCard from '@/components/RecipeCard.vue';
import ApiHelper from '@/models/ApiHelper';
import RecipesSearchRequest from '@/models/RecipesSearchRequest';
import useMessageStore from '@/stores/messageStore';

const api = ApiHelper.client;
const messageStore = useMessageStore();

const list = ref<SearchRecipesResultItem[]>([]);
const isFetchingRecipes = ref(false);

async function fetchDiscoveryRecipes() {
  isFetchingRecipes.value = true;

  try {
    const response = await api().recipesSearch({
      ...new RecipesSearchRequest(),
      page: 1,
      take: 4,
      sortBy: 'random',
      randomSortSeed: Math.random().toString(36).substring(2),
    });

    list.value = response.data.results?.items || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  } finally {
    isFetchingRecipes.value = false;
  }
}

onMounted(fetchDiscoveryRecipes);
</script>

<template>
  <div v-if="list.length > 0" class="mt-4">
    <h2>Discover</h2>
    <div class="grid recipe-grid-container">
      <RecipeCard
        v-for="recipe in list"
        :key="recipe.id"
        :recipe="recipe"
        class="recipe-grid-item"
      />
    </div>
  </div>
  <div v-else-if="isFetchingRecipes" class="m-0 mt-4 text-center">
    <div class="spinner-border m-0" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
