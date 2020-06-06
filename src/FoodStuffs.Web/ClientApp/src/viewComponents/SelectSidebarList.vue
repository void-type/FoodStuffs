<template>
  <b-card
    no-body
  >
    <template
      v-slot:header
    >
      <div
        v-b-toggle="`collapse-${title}`"
      >
        <span
          class="h5 mb-0"
        >
          {{ title }}
        </span>
      </div>
    </template>
    <b-collapse
      :id="`collapse-${title}`"
      visible
    >
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
    </b-collapse>
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
