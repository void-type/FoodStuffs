<script lang="ts" setup>
import Choices from '@/models/Choices';
import { toInt } from '@/models/FormatHelpers';
import type IPaginatedRequest from '@/models/IPaginatedRequest';
import { computed, type PropType } from 'vue';

const props = defineProps({
  listRequest: {
    type: Object as PropType<IPaginatedRequest>,
    required: true,
  },
  totalCount: {
    type: Number,
    required: true,
  },
  limit: {
    type: Number,
    required: false,
    default: () => 7,
  },
  onChangePage: {
    // eslint-disable-next-line no-unused-vars
    type: Function as PropType<(page: number) => void>,
    required: true,
  },
  onChangeTake: {
    // eslint-disable-next-line no-unused-vars
    type: Function as PropType<(take: number) => void>,
    required: true,
  },
});

function range(size: number, startAt = 0) {
  return [...Array(size).keys()].map((i) => i + startAt);
}

function changeTakeLocal(event: Event) {
  const { value } = event.target as HTMLSelectElement;
  props.onChangeTake(Number(value));
}

const currentPage = computed(() => toInt(props.listRequest.page));

const pageCount = computed(() => {
  const total = toInt(props.totalCount, 0);
  const take = toInt(props.listRequest.take, 0);

  if (total < 1 || take < 1) {
    return 1;
  }

  return Math.ceil(total / take);
});

const isPrevDisabled = computed(() => currentPage.value < 2);

const isNextDisabled = computed(() => currentPage.value > pageCount.value - 1);

const pageRange = computed(() => {
  const { limit } = props;
  const limitHalf = Math.floor(limit / 2);
  const totalPages = pageCount.value;
  const current = currentPage.value;

  // If under the limit, return all the pages
  if (totalPages <= limit) {
    return range(totalPages, 1);
  }

  const startOffset = Math.max(current - limitHalf, 1);

  // Near the start
  if (startOffset === 1) {
    return [...range(limit - 1, startOffset), '…'];
  }

  const offsetLimit = totalPages - limit + 1;

  // In the middle
  if (startOffset < offsetLimit) {
    return ['…', ...range(limit - 2, startOffset + 1), '…'];
  }

  // Near the end
  return ['…', ...range(limit - 1, offsetLimit + 1)];
});
</script>

<template>
  <nav aria-label="Table pagination" class="d-print-none">
    <ul class="pagination justify-content-center">
      <li :class="{ 'page-item': true, disabled: isPrevDisabled }">
        <button
          class="page-link"
          role="menuitemradio"
          aria-checked="false"
          type="button"
          aria-label="First page"
          :aria-disabled="isPrevDisabled"
          :tabindex="isPrevDisabled ? -1 : undefined"
          @click="onChangePage(1)"
        >
          <span aria-hidden="true">&laquo;</span>
        </button>
      </li>
      <li :class="{ 'page-item': true, disabled: isPrevDisabled }">
        <button
          class="page-link"
          role="menuitemradio"
          aria-checked="false"
          type="button"
          aria-label="Previous page"
          :aria-disabled="isPrevDisabled"
          :tabindex="isPrevDisabled ? -1 : undefined"
          @click="onChangePage(currentPage - 1)"
        >
          <span aria-hidden="true">&lsaquo;</span>
        </button>
      </li>
      <li
        v-for="n in pageRange"
        :key="n"
        :class="{ 'page-item': true, active: currentPage === n, disabled: typeof n === 'string' }"
        :role="typeof n === 'string' ? 'separator' : undefined"
      >
        <span v-if="typeof n === 'string'" class="page-link">
          <span>{{ n }}</span>
        </span>
        <button
          v-else
          class="page-link"
          role="menuitemradio"
          :aria-checked="currentPage === n"
          type="button"
          :aria-label="`Go to page ${n}`"
          @click="onChangePage(n)"
        >
          <span aria-hidden="true">{{ n }}</span>
        </button>
      </li>
      <li :class="{ 'page-item': true, disabled: isNextDisabled }">
        <button
          class="page-link"
          role="menuitemradio"
          aria-checked="false"
          type="button"
          aria-label="Next page"
          :aria-disabled="isNextDisabled"
          :tabindex="isNextDisabled ? -1 : undefined"
          @click="onChangePage(currentPage + 1)"
        >
          <span aria-hidden="true">&rsaquo;</span>
        </button>
      </li>
      <li :class="{ 'page-item': true, disabled: isNextDisabled }">
        <button
          class="page-link"
          role="menuitemradio"
          aria-checked="false"
          type="button"
          aria-label="Last page"
          :aria-disabled="isNextDisabled"
          :tabindex="isNextDisabled ? -1 : undefined"
          @click="onChangePage(pageCount)"
        >
          <span aria-hidden="true">&raquo;</span>
        </button>
      </li>
    </ul>
    <select
      id="pagination-take"
      name="pagination-take"
      class="form-select mx-auto"
      :value="String(listRequest.take)"
      aria-label="Page size"
      @change="changeTakeLocal"
    >
      <option
        v-for="takeOption in Choices.paginationTake"
        :key="String(takeOption.value)"
        :value="String(takeOption.value)"
      >
        {{ takeOption.text }}
      </option>
    </select>
  </nav>
</template>

<style lang="scss" scoped>
#pagination-take {
  max-width: 5em;
}
</style>
