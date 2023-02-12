<script lang="ts" setup>
import {
  clampHours,
  clampMinutes,
  getTotalMinutes,
  toTimeSpanString,
} from '@/models/TimeSpanHelpers';
import { computed, type PropType } from 'vue';

const props = defineProps({
  id: {
    type: String,
    required: false,
    default: null,
  },
  modelValue: {
    type: Number as PropType<number | null>,
    required: true,
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

const hours = computed(() => clampHours(props.modelValue || 0));
const minutes = computed(() => clampMinutes(props.modelValue || 0));

function onHoursInput(event: Event) {
  const newHours = Number((event.target as HTMLInputElement).value);
  const totalMinutes = getTotalMinutes(minutes.value, newHours);
  emit('update:modelValue', totalMinutes);
}

function onMinutesInput(event: Event) {
  const newMinutes = Number((event.target as HTMLInputElement).value);
  const totalMinutes = getTotalMinutes(newMinutes, hours.value);
  emit('update:modelValue', totalMinutes);
}
</script>

<template>
  <div>
    <label class="visually-hidden" :for="`hours-${id}`">Hours</label>
    <label class="visually-hidden" :for="`minutes-${id}`">Minutes</label>
    <div class="input-group">
      <input
        :id="`hours-${id}`"
        :value="hours"
        :class="{ 'is-invalid': isInvalid, 'text-center': true, 'form-control': true }"
        type="number"
        min="0"
        @change="onHoursInput"
      />
      <input
        :id="`minutes-${id}`"
        :value="minutes"
        :class="{ 'is-invalid': isInvalid, 'text-center': true, 'form-control': true }"
        type="number"
        min="-1"
        @change="onMinutesInput"
      />
    </div>
    <small :class="{ invisible: !(showPreview && (modelValue || 0) > 0) }">
      {{ toTimeSpanString(modelValue || 0) }}
    </small>
  </div>
</template>

<style lang="scss" scoped></style>
