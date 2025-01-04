<script lang="ts" setup>
import { onMounted } from 'vue';
import { RouterView, useRoute } from 'vue-router';
import useAppStore from '@/stores/appStore';
import AppHeader from '@/components/AppHeader.vue';
import AppNav from '@/components/AppNav.vue';
import AppFooter from '@/components/AppFooter.vue';
import AppMessageCenter from '@/components/AppMessageCenter.vue';
import RouterHelper from '@/models/RouterHelper';
import AppModal from '@/components/AppModal.vue';
import ApiHelper from '@/models/ApiHelper';
import useRecipeStore from '@/stores/recipeStore';
import useMealPlanStore from '@/stores/mealPlanStore';
import DarkModeHelper from '@/models/DarkModeHelper';
import RecipeStoreHelper from '@/models/RecipeStoreHelper';
import useMessageStore from '@/stores/messageStore';
import { getCurrentMealPlanFromStorage } from './models/MealPlanStoreHelper';

const appStore = useAppStore();
const messageStore = useMessageStore();
const recipeStore = useRecipeStore();
const mealPlanStore = useMealPlanStore();
const route = useRoute();
const api = ApiHelper.client;

onMounted(() => {
  appStore.setDarkMode(DarkModeHelper.getInitialDarkModeSetting());

  recipeStore.addToRecent(RecipeStoreHelper.getQueuedRecent());

  mealPlanStore.setCurrentMealPlan(getCurrentMealPlanFromStorage());

  api()
    .appGetInfo()
    .then((response) => {
      appStore.setApplicationInfo(response.data);
      RouterHelper.setTitle(route);

      if (response.data.antiforgeryToken) {
        ApiHelper.setHeader(
          response.data.antiforgeryTokenHeaderName || 'X-Csrf-Token',
          response.data.antiforgeryToken
        );
      }
    })
    .catch((response) => messageStore.setApiFailureMessages(response));

  api()
    .appGetVersion()
    .then((response) => appStore.setVersionInfo(response.data));
});
</script>

<template>
  <div id="skip-nav" class="container-xxl visually-hidden-focusable">
    <router-link class="d-inline-flex p-2 m-1" :to="{ hash: '#main', query: route.query }"
      >Skip to main content</router-link
    >
  </div>
  <AppHeader>
    <template #navItems>
      <AppNav />
    </template>
  </AppHeader>
  <main id="main" tabindex="-1">
    <RouterView />
  </main>
  <AppMessageCenter />
  <AppModal />
  <AppFooter />
</template>

<style lang="scss" scoped></style>
