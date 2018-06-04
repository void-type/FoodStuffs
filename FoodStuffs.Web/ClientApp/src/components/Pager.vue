<template>
  <div>
    <div class="pages">
      <span
        v-for="pageNumber in numberOfPages"
        :key="pageNumber"
        :class="{currentPage: pageNumber === page}"
        @click="requestPage(pageNumber)">
        {{ pageNumber }}
      </span>
    </div>
    <div>
      <input
        id="take"
        v-model="takeEditor"
        type="number"
        min="1"
        name="take" >
    </div>
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
      return Math.ceil(this.totalCount / this.take) || 1;
    },
    takeEditor: {
      get() {
        return this.take;
      },
      set(value) {
        this.$emit('updateTake', parseInt(value, 10));
        this.requestPage(this.page);
      },
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

div>div {
  display: flex;
  justify-content: center;

  span {
    padding: 0.5em 1em;
    border: $border;
    cursor: pointer;
    font-weight: bold;
  }

  input {
    padding: 0.5em 1em;
    max-width: 4em;
  }
}
</style>
