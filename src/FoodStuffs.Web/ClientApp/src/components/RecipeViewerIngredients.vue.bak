<template>
  <div>
    <div
      v-for="(ing, id) in formattedIngredients"
      :key="id"
    >
      <h4 v-if="ing.isCategory">
        {{ ing.name }}
      </h4>
      <div v-else>
        {{ ing.quantity }} {{ ing.name }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    ingredients: {
      type: Array,
      required: true,
    },
  },
  computed: {
    formattedIngredients() {
      const ingredients = this.ingredients.slice();
      ingredients.sort((a, b) => a.order - b.order);
      return ingredients;
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
