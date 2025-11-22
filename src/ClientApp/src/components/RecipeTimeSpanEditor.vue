<script lang="ts" setup>
import type { PropType } from 'vue';
import { computed } from 'vue';
import {
  clampHours,
  clampMinutes,
  getTotalMinutes,
  toTimeSpanString,
} from '@/models/TimeSpanHelper';

const props = defineProps({
  id: {
    type: String,
    required: false,
    default: null,
  },
  modelValue: {
    type: Number as PropType<number | null>,
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
    default: true,
  },
});

const emit = defineEmits(['update:modelValue']);

const hours = computed({
  get() {
    return clampHours(props.modelValue || 0);
  },
  set(value) {
    const newHours = Number(value);
    const currentMinutes = clampMinutes(props.modelValue || 0);
    const totalMinutes = getTotalMinutes(currentMinutes, newHours);
    emit('update:modelValue', totalMinutes);
  },
});

const minutes = computed({
  get() {
    return clampMinutes(props.modelValue || 0);
  },
  set(value) {
    const newMinutes = Number(value);
    const currentHours = clampHours(props.modelValue || 0);
    const totalMinutes = getTotalMinutes(newMinutes, currentHours);
    emit('update:modelValue', totalMinutes);
  },
});
</script>

<template>
  <div>
    <label class="visually-hidden" :for="`hours-${id}`">Hours</label>
    <label class="visually-hidden" :for="`minutes-${id}`">Minutes</label>
    <div class="input-group">
      <input
        :id="`hours-${id}`"
        v-model="hours"
        class="form-control text-center" :class="{ 'is-invalid': isInvalid }"
        type="number"
        min="0"
      >
      <input
        :id="`minutes-${id}`"
        v-model="minutes"
        class="form-control text-center" :class="{ 'is-invalid': isInvalid }"
        type="number"
        min="-1"
      >
    </div>
    <small :class="{ invisible: !showPreview }">
      {{ toTimeSpanString(modelValue || 0) }}
    </small>
  </div>
</template>

<style lang="scss" scoped></style>
