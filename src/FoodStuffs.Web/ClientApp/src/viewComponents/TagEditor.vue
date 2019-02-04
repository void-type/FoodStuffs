<template>
  <div>
    <div>
      <div class="tags">
        <span
          v-for="tag in tags"
          :key="tag">
          {{ tag }}&nbsp;&nbsp;<span @click="removeTagClick(tag)">
            &#x2716;</span>
        </span>
      </div>
    </div>
    <div>
      <input
        v-model="newTag"
        :id="fieldName"
        :name="fieldName"
        type="text"
        @keydown.enter.prevent="addTagClick()" >
      <button @click.prevent="addTagClick()">
        Add</button>
    </div>
    <label :for="fieldName">{{ label }}</label>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: true,
    },
    fieldName: {
      type: String,
      required: true,
    },
    tags: {
      type: Array,
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
  },
  data() {
    return {
      newTag: '',
    };
  },
  methods: {
    addTagClick() {
      this.onAddTag(this.newTag);
      this.newTag = '';
    },
    removeTagClick(tag) {
      this.onRemoveTag(tag);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

.form-group > div {
  display: flex;

  & button {
    margin-left: 0.5rem;
  }
}

.tags {
  display: flex;
  flex-flow: wrap;

  & > span {
    margin-top: 0.5rem;
    background-color: $color-neutral-medium;
    box-sizing: border-box;
    padding: 0.3em 0.6rem;
    border: $border;
    box-shadow: $shadow;

    & > span:hover {
      cursor: pointer;
      color: $color-danger-dark;
    }
  }
}
</style>
