<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { storeToRefs } from 'pinia';
import RouterHelper from '@/models/RouterHelper';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const { version } = storeToRefs(appStore);
</script>

<template>
  <footer id="footer" class="mt-5 d-print-none border-top text-center py-3 px-0">
    <div class="m-0 mb-4 text-center">
      <button class="btn btn-outline-secondary" type="button" @click="RouterHelper.scrollToTop()">
        Back to top
        <FontAwesomeIcon icon="fa-arrow-up" class="ms-1" />
      </button>
    </div>
    <div>
      <a href="https://github.com/void-type/foodstuffs">
        FoodStuffs {{ version }} is open source!</a>
    </div>
    <div>
      Logo icon made by <a href="http://www.freepik.com/" title="Freepik">Freepik</a> from
      <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a>
    </div>
  </footer>
</template>

<style lang="scss" scoped></style>
