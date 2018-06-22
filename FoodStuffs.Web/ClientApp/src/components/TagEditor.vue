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
        v-model="newTagName"
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
    fieldName: {
      type: String,
      required: true,
    },
    label: {
      type: String,
      required: true,
    },
    tags: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      newTagName: '',
    };
  },
  methods: {
    addTagClick() {
      this.$emit('addTag', this.newTagName);
      this.newTagName = '';
    },

    removeTagClick(tagToRemove) {
      this.$emit('removeTag', tagToRemove);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/variables";
@import "../style/inputs";

.form-group > div {
  display: flex;
}

.tags {
  display: flex;
  flex-flow: wrap;

  & > span {
    background-color: $color-neutral-medium;
    box-sizing: border-box;
    padding: 0.3em 0.6em;
    border: $border;
    box-shadow: $shadow;

    & > span:hover {
      cursor: pointer;
      color: $color-danger;
    }
  }
}
</style>
