<script lang="ts" setup>
import ApiHelper from '@/models/ApiHelper';
import RecipesListRequest from '@/models/RecipesListRequest';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import RecipeCard from '@/components/RecipeCard.vue';
import type { HttpResponse } from '@/api/http-client';

const recipeStore = useRecipeStore();
const messageStore = useMessageStore();
const api = ApiHelper.client;

const { discoverList, discoverPage } = storeToRefs(recipeStore);

const isFetchingRecipes = ref(false);

const randomSortSeed = crypto.randomUUID();

async function fetchRecipes() {
  if (isFetchingRecipes.value) {
    return;
  }

  isFetchingRecipes.value = true;

  try {
    const response = await api().recipesSearch({
      ...new RecipesListRequest(),
      page: discoverPage.value + 1,
      take: 12,
      sortBy: 'random',
      randomSortSeed,
    });

    recipeStore.setDiscoverListResponse(response.data);
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  } finally {
    isFetchingRecipes.value = false;
  }
}

const loadMoreTriggerElement: Ref<Element | undefined> = ref();
let loadMoreObserver: IntersectionObserver | null = null;

function setupLoadMoreObserver() {
  if (!loadMoreTriggerElement.value) {
    return;
  }

  loadMoreObserver = new IntersectionObserver(
    (entries) => {
      if (entries[0].isIntersecting) {
        fetchRecipes();
      }
    },
    {
      threshold: 1,
    }
  );

  loadMoreObserver.observe(loadMoreTriggerElement.value);
}

function tearDownLoadMoreObserver() {
  if (loadMoreObserver) {
    loadMoreObserver.disconnect();
    loadMoreObserver = null;
  }
}

onMounted(() => {
  setupLoadMoreObserver();
});

onUnmounted(() => {
  tearDownLoadMoreObserver();
});
</script>

<template>
  <div class="container-xxl">
    <div class="grid mt-4">
      <RecipeCard
        v-for="(recipe, i) in discoverList"
        :key="recipe.id"
        :recipe="recipe"
        :lazy="i > 6"
        class="g-col-12 g-col-sm-6 g-col-lg-4"
      />
    </div>
    <div ref="loadMoreTriggerElement" class="m-0"></div>
    <div v-if="isFetchingRecipes" class="m-0 mt-4 text-center">
      <div class="spinner-border m-0" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
    <p v-if="discoverList.length > 0" class="m-0 mt-4 text-center">
      <a href="#main">Back to top</a>
    </p>
  </div>
</template>

<style lang="scss" scoped></style>
