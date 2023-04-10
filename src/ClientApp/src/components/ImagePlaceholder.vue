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
  <div class="card img-placeholder d-print-none">
    <div>{{ text }}</div>
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
}
</style>
