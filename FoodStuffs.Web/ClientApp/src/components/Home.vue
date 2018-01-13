<template>
  <section>
    <recipe-table :recipes="recipes"
                  @recipe-selected="recipeSelected">
    </recipe-table>
    <recipe-form v-if="currentRecipe !== null"
                 :current-recipe="currentRecipe"
                 :fields-in-error="fieldsInError"
                 @create-recipe="createRecipe"
                 @update-recipe="updateRecipe"
                 @delete-recipe="deleteRecipe">
    </recipe-form>
  </section>
</template>

<script>
  export default {
    name: "home"
  };
</script>

<style lang="scss">
  @import '../variables';
</style>
