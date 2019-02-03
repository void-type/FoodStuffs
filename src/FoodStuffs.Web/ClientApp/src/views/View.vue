<template>
  <section>
    <SelectSidebar :route-name="'view'" />
    <RecipeViewer :recipe="sourceRecipe" />
  </section>
</template>

<script>
import { mapActions } from 'vuex';
import webApi from '../webApi';
import recipeModels from '../models/RecipeApiModels';
import SelectSidebar from '../viewComponents/SelectSidebar.vue';
import RecipeViewer from '../viewComponents/RecipeViewer.vue';

export default {
  components: {
    SelectSidebar,
    RecipeViewer,
  },
  props: {
    id: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      sourceRecipe: new recipeModels.GetResponse(),
    };
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
    }),
    fetchRecipe(id) {
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
