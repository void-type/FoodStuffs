<template>
    <table>
        <thead v-if="title !== null">
            <tr>
                <th>{{title}}</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="recipe in recipes"
                :key="recipe.id"
                @click="selectClick(recipe)">

                <td>{{recipe.name}}</td>
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
    title: {
      type: String,
      required: false,
    },
  },
  methods: {
    selectClick(recipe) {
      this.$emit('selectRecipe', recipe);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/variables";
@import "../style/inputs";

table {
  width: 15em;
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

@media #{$medium-screen} {
  table {
    width: 100%;
  }
}
</style>
