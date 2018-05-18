<template>
    <div>
        <span v-for="pageNumber in pageNumbers"
              :key="pageNumber"
              :class="{currentPage: pageNumber === page}"
              @click="requestPage(pageNumber)">
            {{pageNumber}}
        </span>
    </div>
</template>

<script>
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
  },
  computed: {
    numberOfPages() {
      return Math.ceil(this.totalCount / this.take) || 0;
    },
    pageNumbers() {
      return Array.from(Array(this.numberOfPages).keys());
    },
  },
  methods: {
    requestPage(pageNumber) {
      this.$emit('requestPage', pageNumber);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/variables";

.currentPage {
  background-color: $color-focus;
  background-color: mix($color-ternary, $color-secondary, 60%);
  box-shadow: $highlight-border;
}

div {
  display: flex;
  justify-content: center;
}

span {
  padding: 0.5em 1em;
  border: $border;
  cursor: pointer;
  font-weight: bold;
}
</style>
