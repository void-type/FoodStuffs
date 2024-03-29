<script lang="ts" setup>
import { computed, type PropType } from 'vue';

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
  suggestions: {
    type: Array as PropType<Array<string>>,
    required: false,
    default: () => [],
  },
});

let newTag = '';

const availableSuggestions = computed(() =>
  props.suggestions.filter((x) => !props.tags.includes(x))
);

function addTagClick(tag: string) {
  props.onAddTag(tag);
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
        list="tagSuggestions"
        class="form-control"
        :disabled="false"
        @keydown.stop.prevent.enter="addTagClick(newTag)"
      />
      <datalist id="tagSuggestions">
        <option v-for="suggestion in availableSuggestions" :key="suggestion" :value="suggestion">
          {{ suggestion }}
        </option>
      </datalist>
      <button class="btn btn-outline-light" type="button" @click.stop.prevent="addTagClick(newTag)">
        Add
      </button>
    </div>
    <div class="tags mt-2">
      <button
        v-for="tag in tags"
        :key="tag"
        type="button"
        class="btn btn-sm btn-light me-2 mb-2"
        @click.stop.prevent="removeTagClick(tag)"
      >
        {{ tag }}
        <span class="ms-2">✕</span>
      </button>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
