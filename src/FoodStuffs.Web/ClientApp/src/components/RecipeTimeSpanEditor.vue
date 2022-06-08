<script lang="ts" setup>
import RecipeTimeSpan from '@/models/RecipeTimeSpan';
import { computed, getCurrentInstance } from 'vue';

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

const timeSpan = computed(() => new RecipeTimeSpan(props.value));

// eslint-disable-next-line @typescript-eslint/no-empty-function
const emit = getCurrentInstance()?.emit || (() => {});

function onHoursInput(event: Event) {
  const newValue = Number((event.target as HTMLInputElement).value);
  const ts = new RecipeTimeSpan(timeSpan.value.toMinutes(), newValue);
  emit('input', ts.totalMinutes);
}
function onMinutesInput(event: Event) {
  const newValue = Number((event.target as HTMLInputElement).value);
  const ts = new RecipeTimeSpan(newValue, timeSpan.value.toHours());
  emit('input', ts.totalMinutes);
}
</script>

<template>
  <div class="text-center">
    <div class="input-group">
      <label class="sr-only" for="`${id}-hours`"
        >Hours
        <input
          :id="`${id}-hours`"
          :value="timeSpan.toHours()"
          :class="{ 'is-invalid': isInvalid, 'text-center': true, 'form-control': true }"
          type="number"
          min="0"
          @change="onHoursInput"
        />
      </label>
      <label class="sr-only" for="`${id}-minutes`"
        >Minutes
        <input
          :id="`${id}-minutes`"
          :value="timeSpan.toMinutes()"
          :class="{ 'is-invalid': isInvalid, 'text-center': true, 'form-control': true }"
          type="number"
          min="-1"
          @change="onMinutesInput"
        />
      </label>
    </div>
    <small :class="{ hidden: !(showPreview && timeSpan.totalMinutes > 0) }">
      {{ timeSpan.toString() }}
    </small>
  </div>
</template>

<style lang="scss" scoped>
.hidden {
  visibility: hidden;
}
</style>
