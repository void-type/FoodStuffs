<template>
  <form>
    <div class="form-row">
      <div class="form-group">
        <input
          id="nameSearch"
          :value="nameSearch"
          type="text"
          name="nameSearch"
          @input="updateNameSearch($event.target.value)" >
        <label for="nameSearch">Name Contains</label>
      </div>
      <div class="form-group">
        <input
          id="categorySearch"
          :value="categorySearch"
          type="text"
          name="categorySearch"
          @input="updateCategorySearch($event.target.value)" >
        <label for="categorySearch">Categories Contain</label>
      </div>
    </div>
    <div class="form-row">
      <div class="button-group">
        <div>
          <button @click.prevent="search()">
            Search</button>
          <button @click.prevent="clear()">
            Clear All</button>
        </div>
      </div>
    </div>
  </form>
</template>

<script>
export default {
  props: {
    nameSearch: {
      type: String,
      required: true,
    },
    categorySearch: {
      type: String,
      required: true,
    },
    search: {
      type: Function,
      required: true,
    },
    clear: {
      type: Function,
      required: true,
    },
    updateNameSearch: {
      type: Function,
      required: true,
    },
    updateCategorySearch: {
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
