<template>
    <div class="viewer" v-if="currentRecipe.name">
        <h1>{{currentRecipe.name}}</h1>
        <h3>Ingredients</h3>
        <pre>{{currentRecipe.ingredients}}</pre>
        <h3>Directions</h3>
        <pre>{{currentRecipe.directions}}</pre>

        <h3>Stats</h3>

        <div v-if="currentRecipe.prepTimeMinutes !== null"><span>Prep Time: {{currentRecipe.prepTimeMinutes}} minutes</span><br /></div>
        <div v-if="currentRecipe.cookTimeMinutes !== null"><span>Cook Time: {{currentRecipe.cookTimeMinutes}} minutes<br /></span></div>
        <br v-if="currentRecipe.prepTimeMinutes !== null || currentRecipe.cookTimeMinutes !== null"/>
        Created By: {{currentRecipe.createdBy}}<br />
        Created On: {{currentRecipe.createdOnUtc | localDateString}}<br />
        Modified By: {{currentRecipe.modifiedBy}}<br />
        Modified On: {{currentRecipe.modifiedOnUtc | localDateString}}
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
import localDateString from "../../filters/localDateString";

export default {
  props: {
    currentRecipe: {
      type: Object,
      required: true
    }
  },
  filters: {
    localDateString: localDateString
  }
};
</script>

<style lang="scss" scoped>
@import "../variables";
@import "../inputs";

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