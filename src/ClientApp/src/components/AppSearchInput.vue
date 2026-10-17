<script lang="ts" setup>
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { useTemplateRef } from 'vue';

defineOptions({ inheritAttrs: false });

defineProps({
  id: { type: String, required: true },
  clearLabel: { type: String, required: false, default: 'Clear text' },
});

const emit = defineEmits(['clear']);

const model = defineModel({
  type: String as PropType<string | null | undefined>,
  required: true,
});

const inputElement = useTemplateRef<HTMLInputElement>('inputElement');

function clear() {
  model.value = '';
  emit('clear');
  inputElement.value?.focus();
}
</script>

<template>
  <div class="position-relative">
    <input
      :id="id"
      ref="inputElement"
      v-model="model"
      class="form-control search-input"
      v-bind="$attrs"
    >
    <button
      v-if="model"
      type="button"
      class="btn-clear position-absolute top-0 bottom-0 end-0 border-0 bg-transparent px-3 text-secondary"
      :aria-label="clearLabel"
      @click.stop.prevent="clear"
    >
      <FontAwesomeIcon icon="fa-times" />
    </button>
  </div>
</template>

<style lang="scss" scoped>
// Replaces the browser's native clear button so it looks the same in every browser.
.search-input {
  padding-right: 2.5rem;

  &::-webkit-search-cancel-button {
    appearance: none;
    display: none;
  }
}

.btn-clear:hover,
.btn-clear:focus-visible {
  color: var(--bs-body-color) !important;
}
</style>
