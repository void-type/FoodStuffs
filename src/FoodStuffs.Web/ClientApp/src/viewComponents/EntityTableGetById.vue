<template>
  <div
    inline
    class="no-print"
    @keydown.enter.prevent="showDetail(idToGet)"
    @submit.stop.prevent
  >
    <b-input-group
      prepend="ID"
    >
      <label
        class="sr-only"
      >Get By ID</label>
      <b-form-input
        id="idToGet"
        ref="idToGet"
        v-model="idToGet"
        class="prpa-small-input"
        type="number"
      />
      <b-input-group-append>
        <b-button
          variant="primary"
          @click.stop.prevent="showDetail(idToGet)"
        >
          Go
        </b-button>
      </b-input-group-append>
    </b-input-group>
  </div>
</template>

<script>
export default {
  props: {
    showDetail: {
      type: Function,
      required: true,
    },
  },
  data() {
    return {
      idToGet: null,
    };
  },
  mounted() {
    this.$refs.idToGet.focus();
  },
};
</script>

<style lang="scss" scoped>
.prpa-small-input {
  max-width: 5em;
}
</style>
