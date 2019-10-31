<template>
  <b-container>
    <b-row>
      <b-form
        inline
        @submit.stop.prevent
      >
        <div
          class="mr-auto"
        >
          <slot
            name="entityControls"
          />
        </div>
        <slot
          name="auxillaryControls"
        />
      </b-form>
    </b-row>
    <b-row>
      <b-form
        inline
        class="mt-2 d-flex justify-content-end"
        @keydown.enter.stop.prevent="initSearch()"
        @submit.stop.prevent
      >
        <slot
          name="searchControls"
        />
        <b-button
          class="mr-1"
          variant="primary"
          @click.stop.prevent="initSearch()"
        >
          Search
        </b-button>
        <b-button
          @click.stop.prevent="clearSearch()"
        >
          Clear
        </b-button>
      </b-form>
    </b-row>
  </b-container>
</template>

<script>

export default {
  props: {
    clearSearch: {
      type: Function,
      required: true,
    },
    initSearch: {
      type: Function,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
.form-inline {
  width: 100%;
}
</style>
