<script lang="ts" setup>
import useDiscoveryStore from '@/stores/discoveryStore';
import { storeToRefs } from 'pinia';
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import RecipeCard from '@/components/RecipeCard.vue';
import RouterHelper from '@/models/RouterHelper';

const discoveryStore = useDiscoveryStore();

const { list, take, isFetchingRecipes } = storeToRefs(discoveryStore);

const loadMoreTriggerElement: Ref<Element | undefined> = ref();
let loadMoreObserver: IntersectionObserver | null = null;

const showScrollToTop = ref(true);

function handleScroll() {
  const scrollTop = window.scrollY || document.documentElement.scrollTop;
  const { scrollHeight, clientHeight } = document.documentElement;

  // Hide button when within 200px of the top or 100px of the bottom
  showScrollToTop.value = scrollTop > 300 && scrollTop + clientHeight < scrollHeight - 300;
}

function setupLoadMoreObserver() {
  if (!loadMoreTriggerElement.value) {
    return;
  }

  loadMoreObserver = new IntersectionObserver(
    async (entries) => {
      if (entries.length > 0 && entries[0]?.isIntersecting) {
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
  window.addEventListener('scroll', handleScroll);
  handleScroll(); // Initial check
  if (list.value.length < take.value) {
    await discoveryStore.fetchNext();
  }
});

onUnmounted(() => {
  tearDownLoadMoreObserver();
  window.removeEventListener('scroll', handleScroll);
});
</script>

<template>
  <div class="container-xxl">
    <div class="btn-toolbar mt-2">
      <button
        class="btn btn-outline-secondary py-0 px-2 ms-auto"
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
  <Transition name="fade">
    <div v-if="showScrollToTop" class="scroll-to-top">
      <button
        class="btn btn-outline-secondary opacity-75"
        type="button"
        @click="RouterHelper.scrollToTop()"
      >
        <font-awesome-icon icon="fa-arrow-up" />
      </button>
    </div>
  </Transition>
</template>

<style lang="scss" scoped>
.scroll-to-top {
  position: fixed;
  bottom: 1rem;
  left: 50%;
  transform: translateX(-50%);
  z-index: 1000;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
