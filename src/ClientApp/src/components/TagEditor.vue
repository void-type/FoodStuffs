<script lang="ts" setup>
import type { PropType } from 'vue';
import type { Tag } from '@/models/Tag';
import { onClickOutside } from '@vueuse/core';
import { computed, nextTick, ref, useTemplateRef, watch } from 'vue';
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
const inputElement = useTemplateRef<HTMLInputElement>('inputElement');

// Index of the suggestion highlighted with the arrow keys, or -1 to use the typed text.
const activeIndex = ref(-1);

const listId = computed(() => `${props.fieldName}-suggestions`);

function optionId(index: number) {
  return `${props.fieldName}-suggestion-${index}`;
}

function addTag(name: string) {
  props.onAddTag(name);
  newTag.value = '';
  activeIndex.value = -1;
  // Keep typing: return focus to the field, wherever the add came from.
  inputElement.value?.focus();
}

function removeTagClick(tag: Tag) {
  props.onRemoveTag(tag);
}

const showSuggestions = ref(true);

const availableSuggestions = computed(() => {
  const taken = new Set(props.tags.map(x => x.name?.toLowerCase()));

  return props.suggestions
    .filter(x => !taken.has(x.name?.toLowerCase()))
    .filter(x => newTag.value && x.name?.toLowerCase()?.includes(newTag.value.toLowerCase()));
});

const isListOpen = computed(() => availableSuggestions.value.length > 0 && showSuggestions.value);

// Adds the highlighted suggestion if there is one, otherwise whatever was typed.
function submit() {
  const suggestion = isListOpen.value ? availableSuggestions.value[activeIndex.value] : undefined;

  addTag(suggestion?.name ?? newTag.value);
}

// Cycles through the suggestions, with the typed text (-1) as one stop so it's reachable again.
function moveHighlight(step: 1 | -1) {
  const count = availableSuggestions.value.length;

  if (count === 0) {
    return;
  }

  showSuggestions.value = true;

  let next = activeIndex.value + step;

  if (next >= count) {
    next = -1;
  } else if (next < -1) {
    next = count - 1;
  }

  activeIndex.value = next;

  nextTick(() => {
    document.getElementById(optionId(next))?.scrollIntoView({ block: 'nearest' });
  });
}

function onInputKeydown(event: KeyboardEvent) {
  switch (event.key) {
    case 'ArrowDown':
      event.preventDefault();
      moveHighlight(1);
      break;
    case 'ArrowUp':
      event.preventDefault();
      moveHighlight(-1);
      break;
    case 'Enter':
      event.preventDefault();
      event.stopPropagation();
      submit();
      break;
    case 'Escape':
      if (isListOpen.value) {
        event.preventDefault();
        event.stopPropagation();
        showSuggestions.value = false;
      }
      break;
    default:
      break;
  }
}

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
    // Typing starts a fresh search, so drop any highlight.
    activeIndex.value = -1;

    if (newValue.length > 0) {
      showSuggestions.value = true;
    } else {
      showSuggestions.value = false;
    }
  },
);

watch(isListOpen, (open) => {
  if (!open) {
    activeIndex.value = -1;
  }
});
</script>

<template>
  <div ref="tagEditorRef" @focusout="handleFocusOut">
    <label :for="fieldName" class="form-label">{{ label }}</label>
    <div class="input-group">
      <input
        :id="fieldName"
        ref="inputElement"
        v-model="newTag"
        :name="fieldName"
        class="form-control"
        :disabled="false"
        autocomplete="off"
        role="combobox"
        aria-autocomplete="list"
        :aria-expanded="isListOpen"
        :aria-controls="listId"
        :aria-activedescendant="isListOpen && activeIndex > -1 ? optionId(activeIndex) : undefined"
        @keydown="onInputKeydown"
      >
      <button class="btn btn-secondary" type="button" @click.stop.prevent="submit">
        Add
      </button>
    </div>
    <ul
      v-if="isListOpen"
      :id="listId"
      class="dropdown-menu show"
      role="listbox"
      :aria-labelledby="fieldName"
    >
      <li
        v-for="(suggestion, i) in availableSuggestions"
        :id="optionId(i)"
        :key="suggestion.name"
        role="option"
        :aria-selected="i === activeIndex"
      >
        <button
          type="button"
          class="dropdown-item suggestion"
          :class="{ active: i === activeIndex }"
          tabindex="-1"
          @click.prevent.stop="addTag(suggestion.name || '')"
        >
          {{ suggestion.name }}
        </button>
      </li>
    </ul>
    <div>
      <TagBadge
        v-for="tag in tags"
        :key="tag.name"
        removable
        class="me-2 mt-2"
        :tag="tag"
        @remove="removeTagClick(tag)"
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
