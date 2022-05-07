<script setup lang="ts">
import Choices from '@/models/Choices';
import type IItemSet from '@/models/IItemSet';
import type IPaginatedRequest from '@/models/IPaginatedRequest';
import { nextTick, ref, watch, type PropType } from 'vue';

const props = defineProps({
  listResponse: {
    type: Object as PropType<IItemSet<unknown>>,
    required: true,
  },
  listRequest: {
    type: Object as PropType<IPaginatedRequest>,
    required: true,
  },
  onChangePage: {
    type: Function as PropType<(page: number | null) => void>,
    required: true,
  },
  onChangeTake: {
    type: Function as PropType<(take: number | null) => void>,
    required: true,
  },
});

const takeOptions = Choices.paginationTake;
const page = ref(1);
const take = ref(takeOptions[0].value || 30);
const totalCount = ref(0);

function changeTakeLocal(event: Event) {
  const { value } = event.target as HTMLSelectElement;

  const num = Number(value);

  return Number.isNaN(num) ? null : num;
}

watch(props.listResponse, (response) => {
  take.value = response.take || 10;
  totalCount.value = response.totalCount || 0;
  nextTick(() => {
    page.value = response.page || 1;
  });
});
</script>

<template>
  <div class="text-center no-print">
    <!-- <nav aria-label="Table pagination">
      <ul class="pagination"></ul>
    </nav>
    <b-pagination
      id="pagination-page"
      :value="page"
      :total-rows="totalCount"
      name="pagination-page"
      :per-page="take"
      align="center"
      @change="changePage"
    /> -->
    <!-- <b-form-select
      id="pagination-take"
      :value="listRequest.take"
      :options="takeOptions"
      name="pagination-take"
      class="small-input"
      type="number"
      @change="changeTake"
    /> -->

    <label class="sr-only" for="pagination-take"
      >Page size
      <select
        id="pagination-take"
        name="pagination-take"
        class="form-select"
        :value="listRequest.take + ''"
        @change="changeTakeLocal"
      >
        <option
          v-for="takeOption in takeOptions"
          :key="takeOption.value || undefined"
          :value="takeOption.value"
        >
          {{ takeOption.text }}
        </option>
      </select>
    </label>
  </div>
</template>

<style lang="scss" scoped>
#pagination-take {
  max-width: 5em;
}
</style>
