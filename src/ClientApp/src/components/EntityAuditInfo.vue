<script lang="ts" setup>
import DateHelpers from '@/models/DateHelpers';

defineProps({
  entity: {
    type: Object,
    required: false,
    default: null,
  },
});
</script>

<template>
  <div v-if="entity !== null" class="grid">
    <div class="g-col-12 g-col-md-6">
      <small
        >Created: {{ DateHelpers.dateTimeForView(entity.createdOn) }} by
        {{ entity.createdBy }}</small
      >
    </div>
    <div class="g-col-12 g-col-md-6">
      <small
        >Modified: {{ DateHelpers.dateTimeForView(entity.modifiedOn) }} by
        {{ entity.modifiedBy }}</small
      >
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
