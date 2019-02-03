<template>
  <table>
    <thead>
      <tr>
        <th
          class="sortable-header"
          @click="cycleNameSort()">
          Name&nbsp;&nbsp;<span v-html="nameSort.symbol"/>
        </th>
        <th>Categories</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="recipe in recipes"
        :key="recipe.id"
        :to="{name: 'view', props: {id: recipe.id}}">
        <td>{{ recipe.name }}</td>
        <td>{{ recipe.categories.join(", ") }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
export default {
  props: {
    recipes: {
      type: Array,
      required: true,
    },
    nameSort: {
      type: Object,
      required: true,
    },
    cycleNameSort: {
      type: Function,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

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
