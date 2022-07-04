<script lang="ts" setup>
import {
  clampHours,
  clampMinutes,
  getTotalMinutes,
  toTimeSpanString,
} from '@/models/TimeSpanHelpers';
import { computed, getCurrentInstance } from 'vue';

// Expose emit function
const emit =
  getCurrentInstance()?.emit ||
  (() => {
    /* do nothing */
  });

const props = defineProps({
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
});

defineEmits<{
  // eslint-disable-next-line no-unused-vars
  (e: 'input', totalMinutes: number): void;
}>();

const hours = computed(() => clampHours(props.value));
const minutes = computed(() => clampMinutes(props.value));

function onHoursInput(event: Event) {
  const newHours = Number((event.target as HTMLInputElement).value);
  const totalMinutes = getTotalMinutes(minutes.value, newHours);
  emit('input', totalMinutes);
}

function onMinutesInput(event: Event) {
  const newMinutes = Number((event.target as HTMLInputElement).value);
  const totalMinutes = getTotalMinutes(newMinutes, hours.value);
  emit('input', totalMinutes);
}
</script>

<template>
  <div class="text-center">
    <div class="input-group">
      <label class="visually-hidden" for="`${id}-hours`">Hours</label>
      <input
        :id="`${id}-hours`"
        :value="hours"
        :class="{ 'is-invalid': isInvalid, 'text-center': true, 'form-control': true }"
        type="number"
        min="0"
        @change="onHoursInput"
      />
      <label class="visually-hidden" for="`${id}-minutes`">Minutes</label>
      <input
        :id="`${id}-minutes`"
        :value="minutes"
        :class="{ 'is-invalid': isInvalid, 'text-center': true, 'form-control': true }"
        type="number"
        min="-1"
        @change="onMinutesInput"
      />
    </div>
    <small :class="{ invisible: !(showPreview && value > 0) }">
      {{ toTimeSpanString(value) }}
    </small>
  </div>
</template>

<style lang="scss" scoped></style>
