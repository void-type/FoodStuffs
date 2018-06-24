<template>
  <div>
    Created by {{ recipe.createdBy }} on
    {{ recipe.createdOnUtc | utcToLocalDateString }}<br >
    Modified by {{ recipe.modifiedBy }} on
    {{ recipe.modifiedOnUtc | utcToLocalDateString }}<br >
  </div>
</template>

<script>
import utcToLocalDateString from '../filters/utcToLocalDateString';

export default {
  filters: {
    utcToLocalDateString,
  },
  props: {
    recipe: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>

</style>
