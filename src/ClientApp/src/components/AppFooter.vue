<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import { useRoute } from 'vue-router';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const route = useRoute();
const { version } = storeToRefs(appStore);
</script>

<template>
  <footer id="footer" class="mt-4 d-print-none border-top text-center py-3 px-0">
    <div class="m-0 mb-4 text-center">
      <router-link class="btn btn-secondary" :to="{ hash: '#main', query: route.query }"
        >Back to top</router-link
      >
    </div>
    <div>
      <a href="https://github.com/void-type/foodstuffs">
        FoodStuffs {{ version }} is open source!</a
      >
    </div>
    <div>
      Logo icon made by <a href="http://www.freepik.com/" title="Freepik">Freepik</a> from
      <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a>
    </div>
  </footer>
</template>

<style lang="scss" scoped></style>
