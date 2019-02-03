<template>
  <section>
    <SelectSidebar :route-name="'edit'" />
    <RecipeEditor
      :is-field-in-error="isFieldInError"
      :on-save="saveRecipe"
      :source-recipe="sourceRecipe"/>
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
    }),
    fetchRecipe(id) {
      if (this.id === 0) {
        return;
      }
      webApi.recipes.get(
        id,
        (data) => { this.sourceRecipe = data; },
        response => this.setApiFailureMessage(response),
      );
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
