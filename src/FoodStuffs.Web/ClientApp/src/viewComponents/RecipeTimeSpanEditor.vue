<template>
  <div class="text-center">
    <b-input-group>
      <b-form-input
        :id="`${id}-hours`"
        :value="timeSpan.toHours()"
        :class="{'is-invalid': isInvalid, 'text-center': true}"
        type="number"
        min="0"
        @change="onHoursInput"
      />
      <b-form-input
        :id="`${id}-minutes`"
        :value="timeSpan.toMinutes()"
        :class="{'is-invalid': isInvalid, 'text-center': true}"
        type="number"
        min="-1"
        @change="onMinutesInput"
      />
    </b-input-group>
    <small
      :class="{'hidden': !(showPreview && timeSpan.totalMinutes > 0)}"
    >
      {{ timeSpan.toString() }}
    </small>
  </div>
</template>

<script>
import RecipeTimeSpan from '../models/RecipeTimeSpan';

export default {
  props: {
    id: {
      type: String,
      required: false,
      default: null,
    },
    value: {
      type: Number,
      required: false,
      default: 0,
    },
    isInvalid: {
      type: Boolean,
      required: false,
      default: false,
    },
    showPreview: {
      type: Boolean,
      required: false,
      default: false,
    },
  },
  data() {
    return {
      timeSpan: new RecipeTimeSpan(),
    };
  },
  watch: {
    value(newValue) {
      this.timeSpan = new RecipeTimeSpan(newValue);
    },
  },
  methods: {
    onHoursInput(value) {
      const timeSpan = new RecipeTimeSpan(this.timeSpan.toMinutes(), value);
      this.$emit('input', timeSpan.totalMinutes);
    },
    onMinutesInput(value) {
      const timeSpan = new RecipeTimeSpan(value, this.timeSpan.toHours());
      this.$emit('input', timeSpan.totalMinutes);
    },
  },
};
</script>

<style lang="scss" scoped>
.hidden {
  visibility: hidden;
}
</style>
