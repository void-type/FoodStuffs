<script lang="ts" setup>
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';

const props = defineProps({
  label: { type: String, required: true },
  // The key the API expects in sortBy for this column.
  sortKey: { type: String, required: true },
  // The list request's current sort.
  sortBy: { type: String as PropType<string | null | undefined>, required: false, default: '' },
  sortDescending: { type: Boolean, required: false, default: false },
});

const emit = defineEmits(['sort']);

const isActive = computed(() => (props.sortBy || '').toLowerCase() === props.sortKey.toLowerCase());

const ariaSort = computed(() => {
  if (!isActive.value) {
    return 'none';
  }

  return props.sortDescending ? 'descending' : 'ascending';
});

const icon = computed(() => {
  if (!isActive.value) {
    return 'fa-sort';
  }

  return props.sortDescending ? 'fa-sort-down' : 'fa-sort-up';
});
</script>

<template>
  <th :aria-sort="ariaSort">
    <button
      type="button"
      class="btn btn-link p-0 border-0 fw-bold text-reset text-decoration-none"
      @click.stop.prevent="emit('sort', sortKey)"
    >
      {{ label }}
      <FontAwesomeIcon :icon="icon" class="ms-1" :class="{ 'opacity-25': !isActive }" />
    </button>
  </th>
</template>
