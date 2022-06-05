<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const { version } = storeToRefs(appStore);
</script>

<template>
  <footer class="no-print mt-4">
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

<style lang="scss" scoped>
footer {
  border-top: 1px solid;
  padding: 1em 0rem;
  text-align: center;
}
</style>
