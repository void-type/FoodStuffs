<template>
  <b-form-row
    v-if="entity !== null"
  >
    <b-col
      sm="12"
      md="6"
    >
      <small>Created: {{ entity.createdOn | dateFilter }}
        by {{ entity.createdBy }}</small>
    </b-col>
    <b-col
      sm="12"
      md="6"
    >
      <small>Modified: {{ entity.modifiedOn | dateFilter }}
        by {{ entity.modifiedBy }}</small>
    </b-col>
  </b-form-row>
</template>

<script>
import { dateTimeForView } from '@/models/dateHelpers';

export default {
  filters: {
    dateFilter: dateTimeForView,
  },
  props: {
    entity: {
      type: Object,
      required: false,
      default: null,
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
