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
import dateHelpers from '../util/dateHelpers';

export default {
  filters: {
    dateFilter: dateHelpers.dateTimeForView,
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
