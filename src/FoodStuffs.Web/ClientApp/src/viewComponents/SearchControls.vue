<template>
  <form>
    <div class="form-row">
      <div class="form-group">
        <input
          id="nameSearch"
          :value="nameSearch"
          type="text"
          name="nameSearch"
          @input="changeNameSearch($event.target.value)" >
        <label for="nameSearch">Name Contains</label>
      </div>
      <div class="form-group">
        <input
          id="categorySearch"
          :value="categorySearch"
          type="text"
          name="categorySearch"
          @input="changeCategorySearch($event.target.value)" >
        <label for="categorySearch">Categories Contain</label>
      </div>
    </div>
    <div class="form-row">
      <div class="button-group">
        <div>
          <button @click.prevent="initSearch()">
            Search</button>
          <button @click.prevent="clearSearch()">
            Clear All</button>
        </div>
      </div>
    </div>
  </form>
</template>

<script>
export default {
  props: {
    categorySearch: {
      type: String,
      required: true,
    },
    nameSearch: {
      type: String,
      required: true,
    },
    changeCategorySearch: {
      type: Function,
      required: true,
    },
    changeNameSearch: {
      type: Function,
      required: true,
    },
    initSearch: {
      type: Function,
      required: true,
    },
    clearSearch: {
      type: Function,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

form .form-group.button-group {
  margin-top: 1em;
  flex-grow: 0;
}
</style>
