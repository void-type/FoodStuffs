<template>
  <table>
    <thead v-if="title !== null">
      <tr>
        <th>{{ title }}</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="recipe in recipes"
        :key="recipe.id"
        @click="onTableRowClick(recipe)"
      >
        <td>{{ recipe.name }}</td>
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
    title: {
      type: String,
      required: false,
      default: '',
    },
    routeName: {
      type: String,
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

table {
  width: 15rem;
  padding: 1rem;
  border-collapse: collapse;
  border-spacing: 0;

  tr {
    text-align: left;
  }

  th,
  td {
    padding: 0.5em 1rem;
  }
}
</style>
