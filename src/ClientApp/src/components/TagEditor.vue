<script lang="ts" setup>
import type { PropType } from 'vue';
import type { Tag } from '@/models/Tag';
import { onClickOutside } from '@vueuse/core';
import { computed, ref, watch } from 'vue';
import TagBadge from './TagBadge.vue';

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
    type: Array as PropType<Array<Tag>>,
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
    type: Array as PropType<Array<Tag>>,
    required: false,
    default: () => [],
  },
});

const newTag = ref('');

function addTagClick(tag: string) {
  props.onAddTag(tag);
  newTag.value = '';
}

function removeTagClick(tag: Tag) {
  props.onRemoveTag(tag);
}

const showSuggestions = ref(true);

const availableSuggestions = computed(() =>
  props.suggestions
    .filter(x => !props.tags.includes(x))
    .filter(x => newTag.value && x.name?.toLowerCase()?.includes(newTag.value.toLowerCase())),
);

const tagEditorRef = ref<HTMLElement | null>(null);

function handleFocusOut(event: FocusEvent) {
  const target = event.relatedTarget as HTMLElement;
  const isChildOfForm
    = tagEditorRef.value?.contains(document.activeElement)
      || (target && tagEditorRef.value?.contains(target));

  if (!isChildOfForm) {
    showSuggestions.value = false;
  }
}

onClickOutside(tagEditorRef, () => {
  showSuggestions.value = false;
});

watch(
  () => newTag.value,
  (newValue) => {
    if (newValue.length > 0) {
      showSuggestions.value = true;
    } else {
      showSuggestions.value = false;
    }
  },
);
</script>

<template>
  <div ref="tagEditorRef" @focusout="handleFocusOut">
    <label :for="fieldName" class="form-label">{{ label }}</label>
    <div class="input-group">
      <input
        :id="fieldName"
        v-model="newTag"
        :name="fieldName"
        class="form-control"
        :disabled="false"
        autocomplete="off"
        @keydown.stop.prevent.enter="addTagClick(newTag)"
      >
      <button class="btn btn-secondary" type="button" @click.stop.prevent="addTagClick(newTag)">
        Add
      </button>
    </div>
    <ul
      v-if="availableSuggestions.length && showSuggestions"
      class="dropdown-menu show"
      role="listbox"
      :aria-labelledby="fieldName"
    >
      <li
        v-for="suggestion in availableSuggestions"
        :key="suggestion.name"
        role="option"
        aria-selected="false"
      >
        <button
          type="button"
          class="dropdown-item suggestion"
          @click.prevent.stop="addTagClick(suggestion.name || '')"
        >
          {{ suggestion.name }}
        </button>
      </li>
    </ul>
    <div>
      <TagBadge
        v-for="tag in tags"
        :key="tag.name"
        type="button"
        :aria-label="`Click to remove ${tag.name}.`"
        class="me-2 mt-2"
        :tag="tag"
        @click.stop.prevent="removeTagClick(tag)"
      />
    </div>
  </div>
</template>

<style lang="scss" scoped>
.suggestion {
  display: block;
  max-width: 15rem;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
