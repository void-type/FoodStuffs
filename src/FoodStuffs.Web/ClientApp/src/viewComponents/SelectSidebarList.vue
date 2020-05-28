<template>
  <b-card
    no-body
  >
    <template
      v-slot:header
    >
      <h5 class="mb-0">
        {{ title }}
      </h5>
    </template>
    <b-list-group
      flush
    >
      <b-list-group-item
        v-for="recipe in recipes"
        :key="recipe.id"
        button
        @click="viewRecipe(recipe)"
      >
        {{ recipe.name }}
      </b-list-group-item>
    </b-list-group>
  </b-card>
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
    viewRecipe(recipe) {
      router.push({ name: this.routeName, params: { id: recipe.id } }).catch(() => {});
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
