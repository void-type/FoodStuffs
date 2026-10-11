<script lang="ts" setup>
defineProps({
  text: {
    type: String,
    required: false,
    default: 'No image',
  },
});
</script>

<template>
  <div class="img-placeholder d-print-none m-auto">
    <div class="text-muted">
      {{ text }}
    </div>
  </div>
</template>

<style lang="scss" scoped>
.img-placeholder {
  padding-top: calc((75% / 2) - (1.5rem / 2));
  padding-bottom: calc((75% / 2) - (1.5rem / 2));
  width: 100%;
  max-width: 100%;
  align-items: center;
  justify-content: center;
  background-color: rgba(var(--bs-body-color-rgb), 0.05);
  display: flex;
}
</style>
