<script lang="ts" setup>
import { ref, computed, onMounted, onUnmounted, type PropType } from 'vue';
import { storeToRefs } from 'pinia';
import useSidebarStore from '@/stores/sidebarStore';
import type { ListRecipesResponse } from '@/api/data-contracts';
import { useRouter } from 'vue-router';

const props = defineProps({
  recipes: {
    type: Object as PropType<Array<ListRecipesResponse>>,
    required: true,
  },
  title: {
    type: String,
    required: false,
    default: '',
  },
  routeName: {
    type: String,
    required: true,
  },
});

const sidebarStore = useSidebarStore();
const { isSidebarVisibleSetting } = storeToRefs(sidebarStore);
const isScreenLarge = ref(false);
const isSidebarVisible = computed(() => isSidebarVisibleSetting.value || isScreenLarge.value);

const { setSidebarVisibleSetting } = sidebarStore;

function toggleSidebarVisible() {
  if (isScreenLarge.value) {
    return;
  }

  setSidebarVisibleSetting(!isSidebarVisibleSetting.value);
}

function setIsScreenLarge() {
  const width =
    window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

  isScreenLarge.value = width >= 992;
}

const router = useRouter();

function viewRecipe(id: number | undefined) {
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  router.push({ name: props.routeName, params: { id } }).catch(() => {});
}

onMounted(() => {
  setIsScreenLarge();

  // Initialize sidebars as collapsed on small screens.
  if (isSidebarVisibleSetting.value === null) {
    setSidebarVisibleSetting(isScreenLarge.value);
  }

  window.addEventListener('resize', setIsScreenLarge);
});

onUnmounted(() => {
  window.removeEventListener('resize', setIsScreenLarge);
});
</script>

<template>
  <div class="card mt-2">
    {{ isSidebarVisible === true ? 'true' : '' }}
    <h5
      class="card-title mb-0 hover"
      @click="toggleSidebarVisible"
      @keydown.enter="toggleSidebarVisible"
    >
      {{ title }}
    </h5>
    <div :class="{ 'vt-collapsable': true, 'vt-collapsed': !isSidebarVisible }">
      <div class="list-group list-group-flush">
        <div
          v-for="recipe in recipes"
          :key="recipe.id"
          class="list-group-item"
          @click="viewRecipe(recipe.id)"
          @keydown.enter="viewRecipe(recipe.id)"
        >
          {{ recipe.name }}
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import '@/styles/theme.scss';
@import 'bootstrap/scss/bootstrap';

@include media-breakpoint-down(md) {
  .hover:hover {
    background-color: $gray-200;
    cursor: pointer;
  }
}

// TODO: collapsing elements
.vt-collapsable {
  &.vt-collapsed {
    height: 0;
    overflow: hidden;
    transition: height 0.55s ease;
  }
}
</style>
