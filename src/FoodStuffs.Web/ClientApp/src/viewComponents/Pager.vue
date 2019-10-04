<template>
  <div>
    <div class="pages">
      <span
        v-for="pageNumber in numberOfPages"
        :key="pageNumber"
        :class="{currentPage: pageNumber === page}"
        @click="onPageChange(pageNumber)"
      >
        {{ pageNumber }}</span>
    </div>
    <div>
      <select
        id="take"
        :value="take"
        name="take"
        @change="selectTakeOption"
      >
        <option
          v-for="opt in takeOptions"
          :key="opt.id"
          :value="opt.value"
        >
          {{ opt.text }}
        </option>
      </select>
    </div>
  </div>
</template>

<script>
import defaults from '../util/options';

export default {
  props: {
    page: {
      type: Number,
      required: true,
    },
    take: {
      type: Number,
      required: false,
      default: 0,
    },
    totalCount: {
      type: Number,
      required: true,
    },
    onPageChange: {
      type: Function,
      required: true,
    },
    onTakeChange: {
      type: Function,
      required: true,
    },
  },
  computed: {
    numberOfPages() {
      const computed = Math.ceil(this.totalCount / this.take);
      return computed > 1 && Number.isFinite(computed) ? computed : 1;
    },
    takeOptions() {
      return defaults.paginationTakeOptions;
    },
  },
  methods: {
    selectTakeOption(event) {
      // Select only returns strings
      const { value } = event.target;
      this.onTakeChange(value === '' ? null : +value);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

.pages {
  & span {
    &.currentPage {
      background-color: $color-focus;
      background-color: mix($color-ternary, $color-secondary, 90%);
      box-shadow: $highlight-border;
    }

    &:hover,
    &:active {
      background-color: mix($color-ternary, $color-secondary, 60%);
      box-shadow: $highlight-border;
    }
  }
}

div > div {
  display: flex;
  justify-content: center;

  span {
    padding: 0.5em 1rem;
    border: $border;
    cursor: pointer;
    font-weight: 600;
  }

  select,
  input {
    margin-top: 0.5rem;
    max-width: 4rem;
  }
}
</style>
