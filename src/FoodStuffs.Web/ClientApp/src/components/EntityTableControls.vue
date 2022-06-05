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
  <form class="mt-4" @submit.stop.prevent @keydown.enter.stop.prevent="initSearch()">
    <slot name="searchControls" />
    <div class="row">
      <div class="col-12">
        <div class="btn-toolbar" role="toolbar">
          <button class="btn btn-primary me-1" type="button" @click.stop.prevent="initSearch()">
            Search
          </button>
          <button class="btn btn-secondary" type="button" @click.stop.prevent="clearSearch()">
            Clear
          </button>
        </div>
      </div>
    </div>
  </form>
</template>

<style lang="scss" scoped></style>
