<template>
  <b-form
    @submit.stop.prevent
    @keydown.enter.stop.prevent="initSearch()"
  >
    <slot
      name="searchControls"
    />
    <b-form-row>
      <b-col
        cols="12"
      >
        <b-button-toolbar>
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
        </b-button-toolbar>
      </b-col>
    </b-form-row>
  </b-form>
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
</style>
