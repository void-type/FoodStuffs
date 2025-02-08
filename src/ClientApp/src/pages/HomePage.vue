<script lang="ts" setup>
import useDiscoveryStore from '@/stores/discoveryStore';
import { storeToRefs } from 'pinia';
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import RecipeCard from '@/components/RecipeCard.vue';

const discoveryStore = useDiscoveryStore();

const { list, take, isFetchingRecipes } = storeToRefs(discoveryStore);

const loadMoreTriggerElement: Ref<Element | undefined> = ref();
let loadMoreObserver: IntersectionObserver | null = null;

function setupLoadMoreObserver() {
  if (!loadMoreTriggerElement.value) {
    return;
  }

  loadMoreObserver = new IntersectionObserver(
    async (entries) => {
      if (entries[0].isIntersecting) {
        await discoveryStore.fetchNext();
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

onMounted(async () => {
  setupLoadMoreObserver();
  if (list.value.length < take.value) {
    await discoveryStore.fetchNext();
  }
});

onUnmounted(() => {
  tearDownLoadMoreObserver();
});
</script>

<template>
  <div class="container-xxl">
    <div class="grid mt-4">
      <RecipeCard
        v-for="(recipe, i) in list"
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
  </div>
</template>

<style lang="scss" scoped></style>
