<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { Extension, textInputRule } from '@tiptap/core';
import Link from '@tiptap/extension-link';
import Typography from '@tiptap/extension-typography';
import StarterKit from '@tiptap/starter-kit';
import { EditorContent, useEditor } from '@tiptap/vue-3';
import { html as beautifyHtml } from 'js-beautify';
import { nextTick, ref, watch } from 'vue';

defineProps({
  id: {
    type: String,
    required: true,
  },
  disabledButtons: {
    type: Array<string>,
    required: false,
    default: () => ['h1', 'h2', 'h6', 'code', 'clearMarks', 'clearNodes', 'hardBreak', 'blockquote', 'codeBlock'],
  },
});

const model = defineModel<string | null | undefined>();

const CustomReplacer = Extension.create({
  name: 'replacer',
  addInputRules() {
    return [
      // Fractions
      textInputRule({ find: /(?:^|\s)(1\/2)\s$/, replace: '½' }),
      textInputRule({ find: /(?:^|\s)(1\/3)\s$/, replace: '⅓' }),
      textInputRule({ find: /(?:^|\s)(2\/3)\s$/, replace: '⅔' }),
      textInputRule({ find: /(?:^|\s)(1\/4)\s$/, replace: '¼' }),
      textInputRule({ find: /(?:^|\s)(3\/4)\s$/, replace: '¾' }),
      textInputRule({ find: /(?:^|\s)(1\/8)\s$/, replace: '⅛' }),
      textInputRule({ find: /(?:^|\s)(3\/8)\s$/, replace: '⅜' }),
      textInputRule({ find: /(?:^|\s)(5\/8)\s$/, replace: '⅝' }),
      textInputRule({ find: /(?:^|\s)(7\/8)\s$/, replace: '⅞' }),

      // Degrees
      textInputRule({ find: /(?:^|\s)\d+(deg)\s$/, replace: '°' }),
    ];
  },
});

const editor = useEditor({
  injectCSS: false,
  extensions: [
    StarterKit,
    Link.configure({
      openOnClick: false,
      autolink: true,
      HTMLAttributes: {
        rel: 'noopener noreferrer',
        target: '_blank',
      },
    }),
    Typography.configure({
      oneHalf: false,
      oneQuarter: false,
      threeQuarters: false,
    }),
    CustomReplacer,
  ],
  content: model.value,
  onUpdate: () => {
    model.value = editor.value?.getHTML() || '';
  },
});

const showSource = ref(false);

function toggleSource() {
  if (!showSource.value && model.value) {
    model.value = beautifyHtml(model.value, {
      indent_size: 2,
      wrap_line_length: 0,
      end_with_newline: false,
    });
  }
  showSource.value = !showSource.value;
}

watch(
  () => model.value,
  (value) => {
    const isSame = editor.value?.getHTML() === value;

    if (isSame) {
      return;
    }

    editor.value?.commands?.setContent(value || '', { emitUpdate: false });
  },
);

const showLinkPanel = ref(false);
const linkUrl = ref('');
const linkInputRef = ref<HTMLInputElement | null>(null);

function openLinkPanel() {
  const existingHref = editor.value?.getAttributes('link').href ?? '';
  linkUrl.value = existingHref;
  showLinkPanel.value = true;
  nextTick(() => linkInputRef.value?.focus());
}

function applyLink() {
  if (linkUrl.value) {
    editor.value
      ?.chain()
      .focus()
      .setLink({ href: linkUrl.value })
      .run();
  } else {
    editor.value?.chain().focus().unsetLink().run();
  }
  showLinkPanel.value = false;
}

function cancelLink() {
  showLinkPanel.value = false;
  editor.value?.chain().focus().run();
}

const toolbarButtonClass = 'btn btn-sm btn-outline-secondary rounded-0';
</script>

<template>
  <div v-if="editor && !showSource">
    <div class="editor-toolbars pt-1">
      <div class="btn-toolbar flex-wrap" role="toolbar" aria-label="Rich text editor toolbar">
        <!-- History -->
        <button
          v-if="!disabledButtons.includes('source')"
          :class="toolbarButtonClass"
          title="View HTML source"
          @click="toggleSource"
        >
          <FontAwesomeIcon icon="fa-terminal" />
        </button>
        <button
          v-if="!disabledButtons.includes('undo')"
          :class="toolbarButtonClass"
          :disabled="!editor.can().undo()"
          title="Undo"
          @click="editor.chain().focus().undo().run()"
        >
          <FontAwesomeIcon icon="fa-rotate-left" />
        </button>
        <button
          v-if="!disabledButtons.includes('redo')"
          :class="toolbarButtonClass"
          :disabled="!editor.can().redo()"
          title="Redo"
          @click="editor.chain().focus().redo().run()"
        >
          <FontAwesomeIcon icon="fa-rotate-right" />
        </button>

        <!-- Inline marks -->
        <button
          v-if="!disabledButtons.includes('bold')"
          :disabled="!editor.can().toggleBold()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('bold') }"
          title="Bold"
          @click="editor.chain().focus().toggleBold().run()"
        >
          <FontAwesomeIcon icon="fa-bold" />
        </button>
        <button
          v-if="!disabledButtons.includes('italic')"
          :disabled="!editor.can().toggleItalic()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('italic') }"
          title="Italic"
          @click="editor.chain().focus().toggleItalic().run()"
        >
          <FontAwesomeIcon icon="fa-italic" />
        </button>
        <button
          v-if="!disabledButtons.includes('strike')"
          :disabled="!editor.can().toggleStrike()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('strike') }"
          title="Strikethrough"
          @click="editor.chain().focus().toggleStrike().run()"
        >
          <FontAwesomeIcon icon="fa-strikethrough" />
        </button>
        <button
          v-if="!disabledButtons.includes('code')"
          :disabled="!editor.can().toggleCode()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('code') }"
          title="Inline code"
          @click="editor.chain().focus().toggleCode().run()"
        >
          <FontAwesomeIcon icon="fa-code" />
        </button>

        <!-- Block type -->
        <button
          v-if="!disabledButtons.includes('paragraph')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('paragraph') }"
          title="Paragraph"
          @click="editor.chain().focus().setParagraph().run()"
        >
          <FontAwesomeIcon icon="fa-paragraph" />
        </button>
        <button
          v-if="!disabledButtons.includes('h1')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 1 }),
          }"
          title="Heading 1"
          @click="editor.chain().focus().toggleHeading({ level: 1 }).run()"
        >
          <FontAwesomeIcon icon="fa-heading" /><small>1</small>
        </button>
        <button
          v-if="!disabledButtons.includes('h2')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 2 }),
          }"
          title="Heading 2"
          @click="editor.chain().focus().toggleHeading({ level: 2 }).run()"
        >
          <FontAwesomeIcon icon="fa-heading" /><small>2</small>
        </button>
        <button
          v-if="!disabledButtons.includes('h3')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 3 }),
          }"
          title="Heading 3"
          @click="editor.chain().focus().toggleHeading({ level: 3 }).run()"
        >
          <FontAwesomeIcon icon="fa-heading" /><small>3</small>
        </button>
        <button
          v-if="!disabledButtons.includes('h4')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 4 }),
          }"
          title="Heading 4"
          @click="editor.chain().focus().toggleHeading({ level: 4 }).run()"
        >
          <FontAwesomeIcon icon="fa-heading" /><small>4</small>
        </button>
        <button
          v-if="!disabledButtons.includes('h5')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 5 }),
          }"
          title="Heading 5"
          @click="editor.chain().focus().toggleHeading({ level: 5 }).run()"
        >
          <FontAwesomeIcon icon="fa-heading" /><small>5</small>
        </button>
        <button
          v-if="!disabledButtons.includes('h6')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 6 }),
          }"
          title="Heading 6"
          @click="editor.chain().focus().toggleHeading({ level: 6 }).run()"
        >
          <FontAwesomeIcon icon="fa-heading" /><small>6</small>
        </button>

        <!-- Lists -->
        <button
          v-if="!disabledButtons.includes('bullet')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('bulletList') }"
          title="Bullet list"
          @click="editor.chain().focus().toggleBulletList().run()"
        >
          <FontAwesomeIcon icon="fa-list-ul" />
        </button>
        <button
          v-if="!disabledButtons.includes('ordered')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('orderedList') }"
          title="Numbered list"
          @click="editor.chain().focus().toggleOrderedList().run()"
        >
          <FontAwesomeIcon icon="fa-list-ol" />
        </button>

        <!-- Insert -->
        <button
          v-if="!disabledButtons.includes('horizontalRule')"
          :class="toolbarButtonClass"
          title="Horizontal rule"
          @click="editor.chain().focus().setHorizontalRule().run()"
        >
          <FontAwesomeIcon icon="fa-ruler-horizontal" />
        </button>
        <button
          v-if="!disabledButtons.includes('hardBreak')"
          :class="toolbarButtonClass"
          title="Hard break"
          @click="editor.chain().focus().setHardBreak().run()"
        >
          ↵
        </button>

        <!-- Link -->
        <button
          v-if="!disabledButtons.includes('link')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('link') }"
          title="Set link"
          @click="openLinkPanel"
        >
          <FontAwesomeIcon icon="fa-link" />
        </button>
        <button
          v-if="!disabledButtons.includes('unlink')"
          :class="toolbarButtonClass"
          :disabled="!editor.isActive('link')"
          title="Remove link"
          @click="editor.chain().focus().unsetLink().run()"
        >
          <FontAwesomeIcon icon="fa-link-slash" />
        </button>

        <!-- Utility -->
        <button
          v-if="!disabledButtons.includes('clearMarks')"
          :class="toolbarButtonClass"
          title="Clear marks"
          @click="editor.chain().focus().unsetAllMarks().run()"
        >
          Clear marks
        </button>
        <button
          v-if="!disabledButtons.includes('clearNodes')"
          :class="toolbarButtonClass"
          title="Clear nodes"
          @click="editor.chain().focus().clearNodes().run()"
        >
          Clear nodes
        </button>
        <button
          v-if="!disabledButtons.includes('codeBlock')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('codeBlock') }"
          title="Code block"
          @click="editor.chain().focus().toggleCodeBlock().run()"
        >
          Code block
        </button>
        <button
          v-if="!disabledButtons.includes('blockquote')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('blockquote') }"
          title="Blockquote"
          @click="editor.chain().focus().toggleBlockquote().run()"
        >
          Blockquote
        </button>
      </div>

      <!-- Link input panel -->
      <div v-if="showLinkPanel" class="input-group mt-1">
        <input
          ref="linkInputRef"
          v-model="linkUrl"
          type="url"
          class="form-control form-control-sm"
          placeholder="https://..."
          @keyup.enter="applyLink"
          @keyup.escape="cancelLink"
        >
        <button class="btn btn-sm btn-outline-secondary" title="Clear" @click="linkUrl = ''">
          <FontAwesomeIcon icon="fa-times" />
        </button>
        <button class="btn btn-sm btn-outline-secondary" @click="applyLink">
          Apply
        </button>
        <button class="btn btn-sm btn-outline-secondary" @click="cancelLink">
          Cancel
        </button>
      </div>
    </div>
    <EditorContent :id="id" class="rich-text" :editor="editor" />
  </div>
  <div v-if="!editor || showSource">
    <div class="editor-toolbars pt-1">
      <div class="btn-toolbar" role="toolbar">
        <div class="btn-group me-1" role="group">
          <button
            v-if="!disabledButtons.includes('source')"
            :class="toolbarButtonClass"
            title="Visual editor"
            @click="toggleSource"
          >
            <FontAwesomeIcon icon="fa-terminal" />
            HTML
          </button>
        </div>
      </div>
    </div>
    <div class="grow-wrap" :data-replicated-value="model">
      <textarea :id="id" v-model="model" class="tiptap" />
    </div>
  </div>
</template>

<style lang="scss">
.editor-toolbars {
  position: sticky;
  top: calc(var(--navbar-height, 0px) + var(--save-bar-height, 0px));
  z-index: 10;
  background-color: var(--bs-body-bg);
}

.tiptap {
  margin: 0;
  min-height: 10rem;

  // Form-control
  display: block;
  width: 100%;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  font-weight: 400;
  line-height: 1.5;
  color: var(--bs-body-color);
  appearance: none;
  background-color: var(--bs-body-bg);
  background-clip: padding-box;
  border: var(--bs-border-width) solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
  transition:
    border-color 0.15s ease-in-out,
    box-shadow 0.15s ease-in-out;

  &:focus {
    color: var(--bs-body-color);
    background-color: var(--bs-body-bg);
    border-color: rgb(170, 184.5, 161.5);
    outline: 0;
    box-shadow: 0 0 0 0.25rem rgba(85, 114, 68, 0.25);
  }

  :first-child {
    margin-top: 0;
  }

  :last-child {
    margin-bottom: 0;
  }
}

.is-invalid .tiptap {
  border-color: var(--bs-form-invalid-border-color);
  padding-right: calc(1.5em + 0.75rem);
  background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 12 12' width='12' height='12' fill='none' stroke='%23b85454'%3e%3ccircle cx='6' cy='6' r='4.5'/%3e%3cpath stroke-linejoin='round' d='M5.8 3.6h.4L6 6.5z'/%3e%3ccircle cx='6' cy='8.2' r='.6' fill='%23b85454' stroke='none'/%3e%3c/svg%3e");
  background-repeat: no-repeat;
  background-position: right calc(0.375em + 0.1875rem) center;
  background-size: calc(0.75em + 0.375rem) calc(0.75em + 0.375rem);
}

.btn.is-active {
  background-color: var(--bs-secondary-bg);
  border-color: var(--bs-border-color);
  color: var(--bs-body-color);
}

.grow-wrap {
  display: grid;

  &::after {
    content: attr(data-replicated-value) ' ';
    white-space: pre-wrap;
    visibility: hidden;
    grid-area: 1 / 1 / 2 / 2;

    // Match .tiptap styles so the height calculation is accurate
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    min-height: 10rem;
    border: var(--bs-border-width) solid transparent;
    word-break: break-word;
  }

  > textarea {
    resize: none;
    overflow: hidden;
    grid-area: 1 / 1 / 2 / 2;
  }
}
</style>
