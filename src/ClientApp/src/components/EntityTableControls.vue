<script lang="ts" setup>
defineProps({
  clearSearch: {
    type: Function,
    required: true,
  },
  initSearch: {
    type: Function,
    required: true,
  },
});
</script>

<template>
  <form>
    <slot name="searchForm" />
    <div class="btn-toolbar">
      <button class="btn btn-primary me-2" type="button" @click.stop.prevent="initSearch()">
        Search
      </button>
      <button class="btn btn-secondary" type="button" @click.stop.prevent="clearSearch()">
        Clear
      </button>
    </div>
  </form>
</template>

<style lang="scss" scoped></style>
