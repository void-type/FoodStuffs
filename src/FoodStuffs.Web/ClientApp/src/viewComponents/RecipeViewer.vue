<template>
  <div
    v-if="recipe.name"
    class="viewer">
    <router-link
      :to="{name: 'edit', params: {id: recipe.id}}"
      class="float-right"
      tag="button"
    >Edit</router-link>
    <h1>{{ recipe.name }}</h1>
    <h3>Ingredients</h3>
    <pre>{{ recipe.ingredients }}</pre>
    <h3>Directions</h3>
    <pre>{{ recipe.directions }}</pre>
    <h3>Stats</h3>
    <div v-if="recipe.prepTimeMinutes !== null">
      <span>Prep Time: {{ recipe.prepTimeMinutes }} minutes</span>
      <br >
    </div>
    <div v-if="recipe.cookTimeMinutes !== null">
      <span>Cook Time: {{ recipe.cookTimeMinutes }} minutes</span>
      <br >
    </div>
    <div>
      Categories:
      <span class="categories">
        <span
          v-for="category in recipe.categories"
          :key="category">
          {{ category }}</span>
      </span>
    </div>
    <br
      v-if="recipe.prepTimeMinutes !== null
      || recipe.cookTimeMinutes !== null" >
    <EntityDetailsAuditInfo :entity="recipe" />
  </div>
</template>

<script>
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';

export default {
  components: {
    EntityDetailsAuditInfo,
  },
  props: {
    recipe: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";
@import "../style/inputs";

.viewer {
  width: 100%;
}

.categories > span:not(:last-child):after {
  content: ", ";
}

hr {
  clear: both;
  visibility: hidden;
}

.float-right {
  float: right;
}

h1 {
  margin-top: 0;
}
</style>
