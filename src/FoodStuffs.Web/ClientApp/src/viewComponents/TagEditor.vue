<template>
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
            @click.prevent="addTagClick()"
          >
            Add
          </b-button>
        </b-input-group-append>
      </b-input-group>

      <div class="tags mt-2">
        <b-button
          v-for="tag in tags"
          :key="tag"
          class="mr-2 mb-2"
          size="sm"
          variant="outline-secondary"
          @click="removeTagClick(tag)"
        >
          {{ tag }}
          <span
            class="ml-2"
          >
            ✖</span>
        </b-button>
      </div>
    </b-form-group>
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
@import "../style/theme.scss";

.tags {
  .btn {
    color: $body-color;
    border-color: $gray-400;
  }
}
</style>
