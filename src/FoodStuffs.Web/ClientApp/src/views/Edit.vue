<template>
  <section>
    <SelectSidebar :route-name="'view'" />
    <RecipeEditor
      :source-recipe="sourceRecipe"
      :is-field-in-error="isFieldInError"
      :on-save="saveRecipe"
      :on-delete="deleteRecipe" />
  </section>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import webApi from '../webApi';
import recipeModels from '../models/RecipeApiModels';
import SelectSidebar from '../viewComponents/SelectSidebar.vue';
import RecipeEditor from '../viewComponents/RecipeEditor.vue';

export default {
  components: {
    SelectSidebar,
    RecipeEditor,
  },
  props: {
    id: {
      type: Number,
      required: false,
      default: 0,
    },
  },
  data() {
    return {
      sourceRecipe: new recipeModels.GetResponse(),
    };
  },
  computed: {
    ...mapGetters({
      isFieldInError: 'app/isFieldInError',
    }),
  },
  watch: {
    id() {
      this.fetchRecipe(this.id);
    },
  },
  created() {
    this.fetchRecipe(this.id);
  },
  methods: {
    ...mapActions({
      setApiFailureMessage: 'app/setApiFailureMessage',
      saveRecipe: 'recipes/save',
      deleteRecipe: 'recipes/delete',
      addToRecent: 'recipes/addToRecent',
    }),
    fetchRecipe(id) {
      if (this.id === 0) {
        this.sourceRecipe = new recipeModels.GetResponse();
        return;
      }
      webApi.recipes.get(
        id,
        (data) => { this.sourceRecipe = data; },
        response => this.setApiFailureMessage(response),
      );
    },
  },
  beforeRouteUpdate(to, from, next) {
    this.addToRecent(this.sourceRecipe);
    next();
  },
  beforeRouteLeave(to, from, next) {
    // TODO: find out if the child is dirty
    // const dirty = JSON.stringify(this.sourceRecipe) !== JSON.stringify(this.workingRecipe);

    // if (dirty) {
    //   const answer = window.confirm('Do you really want to leave? you have unsaved changes!');

    //   if (!answer) {
    //     next(false);
    //   }
    // }

    this.addToRecent(this.sourceRecipe);
    next();
  },
};
</script>

<style lang="scss" scoped>
</style>
