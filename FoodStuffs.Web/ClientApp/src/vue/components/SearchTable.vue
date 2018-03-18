<template>
    <table>
        <thead>
            <tr>
                <th class="sortable-header"
                    @click="sortByNameClick()">
                    Name &nbsp;
                    <span v-html="selectedNameSortType.symbol"></span>
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
    selectedNameSortType: {
      type: Object,
      required: true
    }
  },
  methods: {
    selectClick(recipe) {
      this.$emit("selectRecipe", recipe);
      this.$router.push({ name: "home" });
    },

    sortByNameClick() {
      this.$emit("cycleSelectedNameSortType");
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

.sortable-header {
  cursor: pointer;
}

@media #{$medium-screen} {
  table {
    width: 100%;
  }
}
</style>