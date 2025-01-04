<script lang="ts" setup>
import DateHelper from '@/models/DateHelper';

defineProps({
  entity: {
    type: Object,
    required: false,
    default: null,
  },
});
</script>

<template>
  <div v-if="entity !== null">
    <div>
      <small
        >Created: {{ DateHelper.dateTimeForView(entity.createdOn) }} by
        {{ entity.createdBy }}</small
      >
    </div>
    <div>
      <small
        >Modified: {{ DateHelper.dateTimeForView(entity.modifiedOn) }} by
        {{ entity.modifiedBy }}</small
      >
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
