<template>
  <div>
    Created: {{ entity.createdOn | dateFilter }}
    by {{ entity.createdBy }}<br>
    Modified: {{ entity.modifiedOn | dateFilter }}
    by {{ entity.modifiedBy }}<br>
  </div>
</template>

<script>
import dateHelper from '../util/dateHelper';

export default {
  filters: {
    dateFilter: dateHelper.dateTimeForView,
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
