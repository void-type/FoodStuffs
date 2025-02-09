<script lang="ts" setup>
import {
  clampHours,
  clampMinutes,
  getTotalMinutes,
  toTimeSpanString,
} from '@/models/TimeSpanHelper';
import { computed, type PropType } from 'vue';

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
    // eslint-disable-next-line no-use-before-define
    const totalMinutes = getTotalMinutes(minutes.value, newHours);
    emit('update:modelValue', totalMinutes);
  },
});

const minutes = computed({
  get() {
    return clampMinutes(props.modelValue || 0);
  },
  set(value) {
    const newMinutes = Number(value);
    const totalMinutes = getTotalMinutes(newMinutes, hours.value);
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
        :class="{ 'is-invalid': isInvalid, 'form-control text-center': true }"
        type="number"
        min="0"
      />
      <input
        :id="`minutes-${id}`"
        v-model="minutes"
        :class="{ 'is-invalid': isInvalid, 'form-control text-center': true }"
        type="number"
        min="-1"
      />
    </div>
    <small :class="{ invisible: !showPreview }">
      {{ toTimeSpanString(modelValue || 0) }}
    </small>
  </div>
</template>

<style lang="scss" scoped></style>
