<template>
  <div>
    <div>
      <b-form-group
        :label="label"
        :label-for="fieldName"
      >
        <b-input-group>
          <b-form-input
            :id="fieldName"
            v-model="newTag"
            :disabled="false"
          />
          <b-input-group-append>
            <b-button
              class="ml-2"
              @click.prevent="addTagClick()"
            >
              Add
            </b-button>
          </b-input-group-append>
        </b-input-group>
      </b-form-group>
    </div>

    <div class="tags">
      <span
        v-for="tag in tags"
        :key="tag"
      >
        {{ tag }}<span
          class="remove-tag"
          @click="removeTagClick(tag)"
        >
          ✖</span>
      </span>
    </div>
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

.tags {
  display: flex;
  flex-flow: wrap;

  & > span {
    margin-top: 0.5rem;
    background-color: $color-secondary;
    box-sizing: border-box;
    padding: 0.3em 0.6rem;
    box-shadow: $shadow;

    &:not(last) {
      margin-right: 0.5rem;
    }

    &.remove-tag {
      margin-left: 0.5rem;
    }

    & > span:hover {
      cursor: pointer;
    }
  }
}
</style>
