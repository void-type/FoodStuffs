<template>
  <b-input-group
    :prepend="name"
  >
    <label
      class="sr-only"
    >{{ name }}</label>
    <b-form-select
      :value="value"
      :options="booleanOptions"
      @change="change"
    />
  </b-input-group>
</template>

<script>
import options from '../util/options';

export default {
  props: {
    name: {
      type: String,
      required: true,
    },
    value: {
      type: Boolean,
      required: false,
      default: null,
    },
    change: {
      type: Function,
      required: true,
    },
    booleanOptions: {
      type: Array,
      required: false,
      default() { return options.booleanOptions; },
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
