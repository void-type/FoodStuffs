<template>
  <div
    class="text-center no-print"
  >
    <b-pagination
      :value="listResponse.page"
      :total-rows="listResponse.totalCount"
      :per-page="listResponse.take"
      align="center"
      @change="changePage"
    />
    <label
      class="sr-only"
    >Pagination Take Amount</label>
    <b-form-select
      :value="listRequest.take"
      :options="takeOptions"
      class="prpa-small-input"
      type="number"
      @change="changeTake"
    />
  </div>
</template>

<script>
import options from '../util/options';

export default {
  props: {
    listResponse: {
      type: Object,
      required: true,
    },
    listRequest: {
      type: Object,
      required: true,
    },
    changePage: {
      type: Function,
      required: true,
    },
    changeTake: {
      type: Function,
      required: true,
    },
  },
  computed: {
    takeOptions() {
      return options.paginationTakeOptions;
    },
  },
};
</script>

<style lang="scss" scoped>
.prpa-small-input {
  max-width: 5em;
}
</style>
