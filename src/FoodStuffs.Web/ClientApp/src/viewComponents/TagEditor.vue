<template>
  <div>
    <div>
      <div class="tags">
        <span
          v-for="tag in tags"
          :key="tag">
          {{ tag }}&nbsp;&nbsp;<span @click="removeTag(tag)">
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
    addTag: {
      type: Function,
      required: true,
    },
    removeTag: {
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
      this.addTag(this.newTag);
      this.newTag = '';
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
    margin-left: 0.5em;
  }
}

.tags {
  display: flex;
  flex-flow: wrap;

  & > span {
    margin-top: 0.5em;
    background-color: $color-neutral-medium;
    box-sizing: border-box;
    padding: 0.3em 0.6em;
    border: $border;
    box-shadow: $shadow;

    & > span:hover {
      cursor: pointer;
      color: $color-danger-dark;
    }
  }
}
</style>
