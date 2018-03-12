<template>
    <div v-if="currentRecipe.name">
        <h1>{{currentRecipe.name}}</h1>
        <h3>Ingredients</h3>
        <pre>{{currentRecipe.ingredients}}</pre>
        <h3>Directions</h3>
        <pre>{{currentRecipe.directions}}</pre>
        <hr />
        <p>
            <span v-if="currentRecipe.prepTimeMinutes !== null"><strong>Prep Time: </strong>{{currentRecipe.prepTimeMinutes}} minutes. </span>
            <span v-if="currentRecipe.cookTimeMinutes !== null"><strong>Cook Time: </strong>{{currentRecipe.cookTimeMinutes}} minutes.</span>
            <br v-if="currentRecipe.prepTimeMinutes !== null || currentRecipe.cookTimeMinutes !== null"/>
            <strong>Categories: </strong>
            <span class="categories">
                <span v-for="category in currentRecipe.categories"
                      :key="category">
                    {{category}}
                </span>
            </span><br />
            <strong>Created By: </strong>{{currentRecipe.createdBy}}. <strong>Created On: </strong>{{currentRecipe.createdOn | dateString}}<br />
            <strong>Modified By: </strong>{{currentRecipe.modifiedBy}}. <strong>Modified On: </strong>{{currentRecipe.modifiedOn | dateString}}
        </p>
    </div>
</template>

<script>
import dateString from "../../filters/dateString";

export default {
  props: {
    currentRecipe: {
      type: Object,
      required: true
    }
  },
  filters: {
    dateString: dateString
  }
};
</script>

<style lang="scss" scoped>
@import "../variables";
@import "../inputs";

.categories > span:not(:last-child):after {
  content: ", ";
}

p {
  margin-bottom: 1.5em;
}

hr {
  margin: 1em 0em;
}

@media screen {
  h1 {
    margin-top: 0;
  }
}
</style>