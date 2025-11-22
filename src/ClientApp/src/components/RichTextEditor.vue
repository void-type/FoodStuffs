<script lang="ts" setup>
import Typography from '@tiptap/extension-typography';
import StarterKit from '@tiptap/starter-kit';
import { EditorContent, Extension, textInputRule, useEditor } from '@tiptap/vue-3';
import { ref, watch } from 'vue';

defineProps({
  id: {
    type: String,
    required: true,
  },
  disabledButtons: {
    type: Array<string>,
    required: false,
    default: () => ['h1', 'h2', 'h6', 'clearMarks', 'clearNodes', 'blockquote', 'codeBlock'],
  },
});

const model = defineModel<string | null | undefined>();

// We'll do our own fractions.
Typography.options.oneHalf = false;
Typography.options.oneQuarter = false;
Typography.options.threeQuarters = false;

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
  extensions: [StarterKit, Typography, CustomReplacer],
  content: model.value,
  onUpdate: () => {
    model.value = editor.value?.getHTML() || '';
  },
});

const showSource = ref(false);

watch(
  () => model.value,
  (value) => {
    const isSame = editor.value?.getHTML() === value;

    if (isSame) {
      return;
    }

    editor.value?.commands?.setContent(value || '', false);
  },
);

const toolbarButtonClass = 'btn btn-outline-secondary rounded-0';
</script>

<template>
  <!-- TODO: toolbar and collapse button. -->
  <div v-if="editor && !showSource">
    <div class="editor-toolbars pb-2">
      <div class="btn-toolbar">
        <button
          v-if="!disabledButtons.includes('source')"
          :class="toolbarButtonClass"
          @click="showSource = !showSource"
        >
          Source
        </button>
        <button
          v-if="!disabledButtons.includes('undo')"
          :class="toolbarButtonClass"
          :disabled="!editor.can().chain().focus().undo().run()"
          @click="editor.chain().focus().undo().run()"
        >
          Undo
        </button>
        <button
          v-if="!disabledButtons.includes('redo')"
          :class="toolbarButtonClass"
          :disabled="!editor.can().chain().focus().redo().run()"
          @click="editor.chain().focus().redo().run()"
        >
          Redo
        </button>
        <button
          v-if="!disabledButtons.includes('bold')"
          :disabled="!editor.can().chain().focus().toggleBold().run()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('bold') }"
          @click="editor.chain().focus().toggleBold().run()"
        >
          Bold
        </button>
        <button
          v-if="!disabledButtons.includes('italic')"
          :disabled="!editor.can().chain().focus().toggleItalic().run()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('italic') }"
          @click="editor.chain().focus().toggleItalic().run()"
        >
          Italic
        </button>
        <button
          v-if="!disabledButtons.includes('underline')"
          :disabled="!editor.can().chain().focus().toggleStrike().run()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('strike') }"
          @click="editor.chain().focus().toggleStrike().run()"
        >
          Strike
        </button>
        <button
          v-if="!disabledButtons.includes('code')"
          :disabled="!editor.can().chain().focus().toggleCode().run()"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('code') }"
          @click="editor.chain().focus().toggleCode().run()"
        >
          Code
        </button>
        <button
          v-if="!disabledButtons.includes('horizontalRule')"
          :class="toolbarButtonClass"
          @click="editor.chain().focus().setHorizontalRule().run()"
        >
          Rule
        </button>
        <button
          v-if="!disabledButtons.includes('hardBreak')"
          :class="toolbarButtonClass"
          @click="editor.chain().focus().setHardBreak().run()"
        >
          Break
        </button>
        <button
          v-if="!disabledButtons.includes('clearMarks')"
          :class="toolbarButtonClass"
          @click="editor.chain().focus().unsetAllMarks().run()"
        >
          Clear marks
        </button>
        <button
          v-if="!disabledButtons.includes('clearNodes')"
          :class="toolbarButtonClass"
          @click="editor.chain().focus().clearNodes().run()"
        >
          Clear nodes
        </button>
        <button
          v-if="!disabledButtons.includes('paragraph')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('paragraph') }"
          @click="editor.chain().focus().setParagraph().run()"
        >
          Paragraph
        </button>
        <button
          v-if="!disabledButtons.includes('h1')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 1 }),
          }"
          @click="editor.chain().focus().toggleHeading({ level: 1 }).run()"
        >
          H1
        </button>
        <button
          v-if="!disabledButtons.includes('h2')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 2 }),
          }"
          @click="editor.chain().focus().toggleHeading({ level: 2 }).run()"
        >
          H2
        </button>
        <button
          v-if="!disabledButtons.includes('h3')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 3 }),
          }"
          @click="editor.chain().focus().toggleHeading({ level: 3 }).run()"
        >
          H3
        </button>
        <button
          v-if="!disabledButtons.includes('h4')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 4 }),
          }"
          @click="editor.chain().focus().toggleHeading({ level: 4 }).run()"
        >
          H4
        </button>
        <button
          v-if="!disabledButtons.includes('h5')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 5 }),
          }"
          @click="editor.chain().focus().toggleHeading({ level: 5 }).run()"
        >
          H5
        </button>
        <button
          v-if="!disabledButtons.includes('h6')"
          :class="{
            [toolbarButtonClass]: true,
            'is-active': editor.isActive('heading', { level: 6 }),
          }"
          @click="editor.chain().focus().toggleHeading({ level: 6 }).run()"
        >
          H6
        </button>
        <button
          v-if="!disabledButtons.includes('bullet')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('bulletList') }"
          @click="editor.chain().focus().toggleBulletList().run()"
        >
          Bullet
        </button>
        <button
          v-if="!disabledButtons.includes('ordered')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('orderedList') }"
          @click="editor.chain().focus().toggleOrderedList().run()"
        >
          Number
        </button>
        <button
          v-if="!disabledButtons.includes('codeBlock')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('codeBlock') }"
          @click="editor.chain().focus().toggleCodeBlock().run()"
        >
          Code block
        </button>
        <button
          v-if="!disabledButtons.includes('blockquote')"
          :class="{ [toolbarButtonClass]: true, 'is-active': editor.isActive('blockquote') }"
          @click="editor.chain().focus().toggleBlockquote().run()"
        >
          Blockquote
        </button>
      </div>
    </div>
    <EditorContent :id="id" class="rich-text" :editor="editor" />
  </div>
  <div v-else>
    <div class="editor-toolbars pb-2">
      <div class="btn-toolbar">
        <button
          v-if="!disabledButtons.includes('source')"
          :class="toolbarButtonClass"
          @click="showSource = !showSource"
        >
          HTML
        </button>
      </div>
    </div>
    <div class="grow-wrap">
      <textarea
        :id="id"
        v-model="model"
        class="tiptap"
        @input="
          (e) => editor?.commands?.setContent((e.target as HTMLInputElement)?.value || '', false)
        "
      />
    </div>
  </div>
</template>

<style lang="scss">
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
</style>
