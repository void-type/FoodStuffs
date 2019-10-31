<template>
  <b-input-group
    slot="searchControls"
    prepend="Search Text"
  >
    <label
      for="searchString"
      class="sr-only"
    >Search Text</label>
    <b-form-input
      id="searchString"
      :value="searchString"
      @input="updateSearchString"
    />
  </b-input-group>
</template>

<script>
export default {
  props: {
    searchString: {
      type: String,
      required: false,
      default: null,
    },
    updateSearchString: {
      type: Function,
      required: true,
    },
  },
};
</script>
