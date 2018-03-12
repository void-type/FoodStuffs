<template>
    <table>
        <thead>
            <tr>
                <th class="ptr"
                    @click="sortByNameClick()">
                    Name &nbsp;
                    <span v-html="selectedSortSymbol"></span>
                </th>
                <th>Category</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="recipe in recipes"
                :key="recipe.id"
                @click="selectClick(recipe)">
                <td>{{recipe.name}}</td>
                <td>{{recipe.categories.join(", ")}}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
import sortTypes from "../../models/recipeSearchSortTypes";

export default {
  props: {
    recipes: {
      type: Array,
      required: true
    },
    selectedSort: {
      type: String,
      required: true
    }
  },
  computed: {
    // TODO: pass this as a prop?
    selectedSortSymbol() {
      return sortTypes.filter(type => type.name === this.selectedSort)[0]
        .symbol;
    }
  },
  methods: {
    selectClick(recipe) {
      this.$emit("selectRecipe", recipe);
      this.$router.push({ name: "home" });
    },

    sortByNameClick() {
      this.$emit("updateSelectedSort");
    }
  }
};
</script>

<style lang="scss" scoped>
@import "../variables";
@import "../inputs";

table {
  width: 100%;
  padding: 1em;
  border-collapse: collapse;
  border-spacing: 0;

  tr {
    text-align: left;
  }

  th {
    border-bottom: $border;
  }

  td {
    border-bottom: $border-light;
  }

  th,
  td {
    padding: 0.5em 1em;
  }

  tbody tr:hover {
    cursor: pointer;
    background-color: mix($color-ternary, $color-secondary, 60%);
    box-shadow: $highlight-border;
  }
}

.ptr {
  cursor: pointer;
}

@media #{$medium-screen} {
  table {
    width: 100%;
  }
}
</style>