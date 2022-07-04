<script lang="ts" setup>
import type { PropType } from 'vue';

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
    <label :for="fieldName" class="form-label">{{ label }}</label>
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
        class="btn btn-sm btn-secondary me-2 mb-2"
        @click="removeTagClick(tag)"
      >
        {{ tag }}
        <span class="ms-2">✕</span>
      </button>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
