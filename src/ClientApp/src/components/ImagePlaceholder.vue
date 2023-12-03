<script lang="ts" setup>
defineProps({
  text: {
    type: String,
    required: false,
    default: 'No image',
  },
  height: {
    type: String,
    required: false,
    default: '16rem',
  },
  width: {
    type: String,
    required: false,
    default: '20rem',
  },
});
</script>

<template>
  <div class="img-placeholder d-print-none">
    <div class="text-muted">{{ text }}</div>
  </div>
</template>

<style lang="scss" scoped>
.img-placeholder {
  height: v-bind(height);
  width: 100%;
  max-width: v-bind(width);
  margin-left: auto;
  margin-right: auto;
  align-items: center;
  justify-content: center;
  background-color: inherit;
  display: flex;
  outline: var(--bs-border-width) solid var(--bs-border-color);
}

html[data-bs-theme='dark'] .img-placeholder {
  outline: var(--bs-gray-800) 1px solid;
}
</style>
