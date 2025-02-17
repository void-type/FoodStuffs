<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
</script>

<template>
  <span class="sort-handle" @click.stop.prevent>
    <div class="visually-hidden">Drag to sort</div>
    <font-awesome-icon icon="fa-grip-vertical" class="text-muted" />
  </span>
</template>

<style lang="scss" scoped>
html[data-bs-theme='dark'] .text-muted {
  color: rgba(var(--bs-secondary-color-rgb), 0.4) !important;
}

.sort-handle {
  cursor: grab;
}
</style>
