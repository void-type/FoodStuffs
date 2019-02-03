<template>
  <div>
    <div class="pages">
      <span
        v-for="pageNumber in numberOfPages"
        :key="pageNumber"
        :class="{currentPage: pageNumber === page}"
        @click="changePage(pageNumber)">
        {{ pageNumber }}</span>
    </div>
    <div>
      <select
        id="take"
        :value="take"
        name="take"
        @change="selectTakeOption" >
        <option
          v-for="opt in takeOptions"
          :key="opt.id"
          :value="opt.value">{{ opt.text }}</option>
      </select>
    </div>
  </div>
</template>

<script>
import defaults from '../util/options';

export default {
  props: {
    totalCount: {
      type: Number,
      required: true,
    },
    page: {
      type: Number,
      required: true,
    },
    take: {
      type: Number,
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
    numberOfPages() {
      return Math.ceil(this.totalCount / this.take) || 1;
    },
    takeOptions() {
      return defaults.paginationTakeOptions;
    },
  },
  methods: {
    selectTakeOption(event) {
      this.changeTake(event.target.value);
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
    padding: 0.5em 1em;
    border: $border;
    cursor: pointer;
    font-weight: bold;
  }

  select,
  input {
    margin-top: 0.5em;
    max-width: 4em;
  }
}
</style>
