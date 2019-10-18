<template>
  <table>
    <thead>
      <tr>
        <th
          class="sortable-header"
          @click="onCycleSort(sort.name)"
        >
          Name<span class="sort-type">{{ sort.symbol }}</span>
        </th>
        <th>Categories</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="recipe in recipes"
        :key="recipe.id"
        @click="onTableRowClick(recipe)"
      >
        <td>{{ recipe.name }}</td>
        <td>{{ recipe.categories.join(", ") }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import router from '../router';

export default {
  props: {
    recipes: {
      type: Array,
      required: true,
    },
    sort: {
      type: Object,
      required: true,
    },
    onCycleSort: {
      type: Function,
      required: true,
    },
  },
  methods: {
    onTableRowClick(recipe) {
      router.push({ name: 'view', params: { id: recipe.id } }).catch(() => {});
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

table {
  width: 100%;
  padding: 1rem;
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
    padding: 0.5em 1rem;
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

.sort-type {
  margin-left: 0.5rem;
}

@media #{$medium-screen} {
  table {
    width: 100%;
  }
}
</style>
