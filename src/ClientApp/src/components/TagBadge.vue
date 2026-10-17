<script setup lang="ts">
import type { PropType } from 'vue';
import type { RouteLocationRaw } from 'vue-router';
import type { Tag } from '@/models/Tag';
import { computed } from 'vue';
import { RouterLink } from 'vue-router';
import { DEFAULT_TAG_COLOR } from '@/models/Tag';

const props = defineProps({
  tag: {
    type: Object as PropType<Tag>,
    required: false,
    default: () => ({ color: DEFAULT_TAG_COLOR }),
  },
  // Renders the badge as a link.
  to: {
    type: [String, Object] as PropType<RouteLocationRaw | undefined>,
    required: false,
    default: undefined,
  },
  // Renders the badge as a button that emits "remove". Takes precedence over "to".
  removable: { type: Boolean, required: false, default: false },
});

const emit = defineEmits(['remove']);

const rootComponent = computed(() => {
  if (props.removable) {
    return 'button';
  }

  return props.to ? RouterLink : 'span';
});

const isInteractive = computed(() => props.removable || !!props.to);

function onClick() {
  if (props.removable) {
    emit('remove');
  }
}

const badgeStyleComputed = computed(() => {
  const { color } = props.tag;
  if (!color || color === DEFAULT_TAG_COLOR) {
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
  const textColor = luminance > 0.5 ? '#000' : '#fff';

  return {
    '--badge-bg-color': color,
    'background-color': 'var(--badge-bg-color, var(--bs-secondary-bg, #6c757d))',
    'color': textColor,
  };
});
</script>

<template>
  <component
    :is="rootComponent"
    class="badge rounded-pill"
    :class="{ 'badge-interactive': isInteractive, 'border-0': removable }"
    :to="removable ? undefined : to"
    :type="removable ? 'button' : undefined"
    :aria-label="removable ? `Click to remove ${tag.name}.` : undefined"
    :style="badgeStyleComputed"
    @click="onClick"
  >
    {{ props.tag.name }}
  </component>
</template>

<style lang="scss" scoped>
.badge-interactive {
  text-decoration: none;

  &:hover,
  &:focus-visible {
    filter: brightness(0.9);
  }
}
</style>
