<script setup lang="ts">
import type { PropType } from 'vue';
import type { Tag } from '@/models/Tag';
import { computed } from 'vue';

const props = defineProps({
  tag: {
    type: Object as PropType<Tag>,
    required: false,
    default: () => ({ color: '#000000' }),
  },
});

const badgeStyleComputed = computed(() => {
  const { color } = props.tag;
  if (!color || color === '#000000') {
    // .text-bg-secondary
    return {
      color: '#000 !important',
      backgroundColor: 'RGBA(var(--bs-secondary-rgb),var(--bs-bg-opacity, 1)) !important',
    };
  }

  const hex = color.replace('#', '');
  const r = Number.parseInt(hex.substr(0, 2), 16);
  const g = Number.parseInt(hex.substr(2, 2), 16);
  const b = Number.parseInt(hex.substr(4, 2), 16);

  const luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
  const textColor = luminance > 0.5 ? 'text-dark' : 'text-light';

  return {
    '--badge-bg-color': color,
    'background-color': 'var(--badge-bg-color, var(--bs-secondary-bg, #6c757d))',
    'color': `var(--${textColor}-color, #fff)`,
  };
});
</script>

<template>
  <span class="badge rounded-pill" :style="badgeStyleComputed">
    {{ props.tag.name }}
  </span>
</template>
