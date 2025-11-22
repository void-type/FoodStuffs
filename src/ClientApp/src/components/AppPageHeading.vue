<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const props = defineProps({
  title: {
    type: String,
    required: false,
    default: '',
  },
});

const route = useRoute();

const pageTitle = computed(() => props.title || route.meta.title);
</script>

<template>
  <h1 class="mt-4">
    {{ pageTitle }}
  </h1>
</template>

<style scoped></style>
