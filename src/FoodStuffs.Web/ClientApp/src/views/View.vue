<template>
  <section>
    <SelectSidebar :route-name="'view'" />
    <RecipeViewer :recipe="sourceRecipe" />
  </section>
</template>

<script>
import { mapActions } from 'vuex';
import webApi from '../webApi';
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
      sourceRecipe: new webApi.recipes.models.GetResponse(),
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
      addToRecent: 'recipes/addToRecent',
    }),
    fetchRecipe(id) {
      webApi.recipes.get(
        id,
        (data) => { this.sourceRecipe = data; },
        response => webApi.setApiFailureMessage(response),
      );
    },
  },
  beforeRouteUpdate(to, from, next) {
    this.addToRecent(this.sourceRecipe);
    next();
  },
  beforeRouteLeave(to, from, next) {
    this.addToRecent(this.sourceRecipe);
    next();
  },
};
</script>

<style lang="scss" scoped>
</style>
