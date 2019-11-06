<template>
  <div
    v-if="recipe.name"
    class="viewer"
  >
    <b-button
      :to="{name: 'edit', params: {id: recipe.id}}"
      class="float-right"
    >
      Edit
    </b-button>
    <h1>{{ recipe.name }}</h1>
    <h3
      class="mt-3"
    >
      Ingredients
    </h3>
    <b-form-textarea
      plaintext
      rows="1"
      :max-rows="Number.MAX_SAFE_INTEGER"
      :value="recipe.ingredients"
    />
    <h3
      class="mt-3"
    >
      Directions
    </h3>
    <b-form-textarea
      plaintext
      rows="1"
      :max-rows="Number.MAX_SAFE_INTEGER"
      :value="recipe.directions"
    />
    <h3
      class="mt-3"
    >
      Stats
    </h3>
    <div v-if="recipe.prepTimeMinutes !== null">
      Prep Time: {{ recipe.prepTimeMinutes }} minutes
    </div>
    <div v-if="recipe.cookTimeMinutes !== null">
      Cook Time: {{ recipe.cookTimeMinutes }} minutes
    </div>
    <div v-if="recipe.categories.length > 0">
      Categories: {{ recipe.categories.join(', ') }}
    </div>
    <div>
      <EntityDetailsAuditInfo :entity="recipe" />
    </div>
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

h1 {
  margin-top: 0;
}
</style>
