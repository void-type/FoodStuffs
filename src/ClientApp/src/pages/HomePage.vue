<script lang="ts" setup>
import useDiscoveryStore from '@/stores/discoveryStore';
import { storeToRefs } from 'pinia';
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
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

async function randomize() {
  await discoveryStore.rollRandomSortSeed();
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
    <div class="btn-toolbar mt-2">
      <button
        class="btn btn-outline-primary py-0 px-2"
        aria-label="Randomize"
        title="Randomize"
        @click="randomize"
      >
        <font-awesome-icon icon="fa-dice-three" />
      </button>
    </div>
    <div class="grid mt-2">
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
