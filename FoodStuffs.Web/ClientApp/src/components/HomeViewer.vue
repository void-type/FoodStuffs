<template>
    <div class="viewer" v-if="currentRecipe.name">
        <h1>{{currentRecipe.name}}</h1>
        <h3>Ingredients</h3>
        <pre>{{currentRecipe.ingredients}}</pre>
        <h3>Directions</h3>
        <pre>{{currentRecipe.directions}}</pre>

        <h3>Stats</h3>

        <div v-if="currentRecipe.prepTimeMinutes !== null">
          <span>Prep Time: {{currentRecipe.prepTimeMinutes}} minutes</span>
          <br />
        </div>
        <div v-if="currentRecipe.cookTimeMinutes !== null">
          <span>Cook Time: {{currentRecipe.cookTimeMinutes}} minutes</span>
          <br />
        </div>
        <br v-if="currentRecipe.prepTimeMinutes !== null
          || currentRecipe.cookTimeMinutes !== null" />
        Created By: {{currentRecipe.createdBy}}<br />
        Created On: {{currentRecipe.createdOnUtc | utcToLocalDateString}}<br />
        Modified By: {{currentRecipe.modifiedBy}}<br />
        Modified On: {{currentRecipe.modifiedOnUtc | utcToLocalDateString}}
        <br /><br />
        <div class="no-print">
            Categories:
            <span class="categories">
                <span v-for="category in currentRecipe.categories"
                      :key="category">
                    {{category}}
                </span>
            </span>
        </div>
    </div>
</template>

<script>
import utcToLocalDateString from '../filters/utcToLocalDateString';

export default {
  props: {
    currentRecipe: {
      type: Object,
      required: true,
    },
  },
  filters: {
    utcToLocalDateString,
  },
};
</script>

<style lang="scss" scoped>
@import "../style/variables";
@import "../style/inputs";

div.viewer {
  width: 100%;
}

.categories > span:not(:last-child):after {
  content: ", ";
}

hr {
  clear: both;
  visibility: hidden;
}

@media screen {
  h1 {
    margin-top: 0;
  }
}
</style>
