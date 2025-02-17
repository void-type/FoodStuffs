<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const { version } = storeToRefs(appStore);
</script>

<template>
  <footer id="footer" class="mt-4 d-print-none border-top text-center py-3 px-0">
    <div class="m-0 mb-4 text-center">
      <a class="btn btn-secondary" href="#main">Back to top</a>
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
