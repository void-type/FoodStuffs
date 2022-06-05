<script lang="ts" setup>
// import { library } from '@fortawesome/fontawesome-svg-core';
// import { faTimes } from '@fortawesome/free-solid-svg-icons';
// import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

import type { PropType } from 'vue';

// library.add(faTimes);

const props = defineProps({
  label: {
    type: String,
    required: true,
  },
  fieldName: {
    type: String,
    required: true,
  },
  tags: {
    type: Array as PropType<Array<string>>,
    required: true,
  },
  onAddTag: {
    type: Function,
    required: true,
  },
  onRemoveTag: {
    type: Function,
    required: true,
  },
});

let newTag = '';

function addTagClick() {
  props.onAddTag(newTag);
  newTag = '';
}

function removeTagClick(tag: string) {
  props.onRemoveTag(tag);
}
</script>

<template>
  <div>
    <div class="mb-3">
      <label :for="fieldName" class="form-label"
        >{{ label }}
        <div class="input-group">
          <input
            :id="fieldName"
            v-model="newTag"
            :name="fieldName"
            class="form-control"
            :disabled="false"
            @keydown.stop.prevent.enter="addTagClick()"
          />
          <button class="btn btn-secondary" type="button">Add</button>
        </div>
        <div class="tags mt-2">
          <button
            v-for="tag in tags"
            :key="tag"
            class="btn btn-sm btn-outline-secondary me-2 mb-2"
            @click="removeTagClick(tag)"
          >
            {{ tag }}
            <span class="ms-2">✕</span>
          </button>
        </div>
      </label>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import '@/styles/theme.scss';
@import 'bootstrap/scss/bootstrap';

.tags {
  .btn {
    color: $body-color;
    border-color: $gray-400;
  }
}
</style>
