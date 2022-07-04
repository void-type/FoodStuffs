<script lang="ts" setup>
import { ref, computed, onMounted, onUnmounted, type PropType } from 'vue';
import { storeToRefs } from 'pinia';
import useSidebarStore from '@/stores/sidebarStore';
import type { ListRecipesResponse } from '@/api/data-contracts';

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
  <div class="card">
    <div
      class="card-header h5 mb-0 hover"
      @click="toggleSidebarVisible"
      @keydown.enter="toggleSidebarVisible"
    >
      {{ title }}
    </div>
    <div :class="{ 'vt-collapsed': !isSidebarVisible }">
      <div class="list-group list-group-flush">
        <router-link
          v-for="recipe in recipes"
          :key="recipe.id"
          class="list-group-item"
          :to="{ name: props.routeName, params: { id: recipe.id } }"
        >
          {{ recipe.name }}
        </router-link>
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

.vt-collapsed {
  height: 0;
  overflow: hidden;
  transition: height 0.55s ease;
}
</style>
