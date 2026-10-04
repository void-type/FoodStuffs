<script lang="ts" setup>
import AppScrollToTop from '@/components/AppScrollToTop.vue';
import HomeDiscovery from '@/components/HomeDiscovery.vue';
import HomeMealPlanGlance from '@/components/HomeMealPlanGlance.vue';
import HomeRecentlyViewed from '@/components/HomeRecentlyViewed.vue';
</script>

<template>
  <div class="container-xxl">
    <HomeMealPlanGlance />
    <HomeRecentlyViewed />
    <HomeDiscovery />
  </div>
  <AppScrollToTop />
</template>

<style lang="scss" scoped></style>
