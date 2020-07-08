<template>
  <div
    class="text-center no-print"
  >
    <b-pagination
      id="pagination-page"
      :value="page"
      :total-rows="totalCount"
      name="pagination-page"
      :per-page="take"
      align="center"
      @change="changePage"
    />
    <label
      class="sr-only"
      for="pagination-take"
    >Pagination Take Amount</label>
    <b-form-select
      id="pagination-take"
      :value="listRequest.take"
      :options="takeOptions"
      name="pagination-take"
      class="small-input"
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
  data() {
    return {
      page: 1,
      take: options.paginationTakeOptions[0].value,
      totalCount: 0,
    };
  },
  computed: {
    takeOptions() {
      return options.paginationTakeOptions;
    },
  },
  watch: {
    listResponse(response) {
      this.take = response.take;
      this.totalCount = response.totalCount;
      this.$nextTick(() => { this.page = response.page; });
    },
  },
};
</script>

<style lang="scss" scoped>
.small-input {
  max-width: 5em;
}
</style>
