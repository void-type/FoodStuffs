<template>
    <form>
        <div class="form-row">
            <div class="form-group">
                <input type="text"
                       id="nameSearch"
                       name="nameSearch"
                       :value="nameSearch"
                       @input="updateNameSearch($event.target.value)" />
                <label for="nameSearch">Name Contains</label>
            </div>
            <div class="form-group">
                <input type="text"
                       id="categorySearch"
                       name="categorySearch"
                       :value="categorySearch"
                       @input="updateCategorySearch($event.target.value)" />
                <label for="categorySearch">Categories Contain</label>
            </div>
            <div class="form-group button-group">
                <div>
                    <button @click.prevent="search(nameSearch, categorySearch)">
                        Search
                    </button>
                    <button @click.prevent="clear()">
                        Clear All
                    </button>
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
      required: true
    },
    categorySearch: {
      type: String,
      required: true
    }
  },
  methods: {
    search(nameSearch, categorySearch) {
      this.$emit("search", nameSearch, categorySearch);
    },
    clear(nameSearch, categorySearch) {
      this.$emit("clear");
    },
    updateNameSearch(value) {
      this.$emit("updateNameSearch", value);
    },
    updateCategorySearch(value) {
      this.$emit("updateCategorySearch", value);
    }
  }
};
</script>

<style lang="scss" scoped>
@import "../variables";
@import "../inputs";

form .form-group.button-group {
  margin-top: 1em;
  flex-grow: 0;
}
</style>