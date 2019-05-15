<template>
  <section>
    <SelectSidebar :route-name="'view'" />
    <RecipeViewer :recipe="sourceRecipe" />
  </section>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
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
  computed: {
    ...mapGetters({
      listResponse: 'recipes/listResponse',
    }),
  },
  watch: {
    id() {
      this.fetchRecipe(this.id);
    },
  },
  created() {
    this.fetchRecipe(this.id);

    if (this.listResponse.count === 0) {
      this.fetchRecipesList();
    }
  },
  methods: {
    ...mapActions({
      setApiFailureMessages: 'app/setApiFailureMessages',
      addToRecent: 'recipes/addToRecent',
      setListResponse: 'recipes/setListResponse',
    }),
    fetchRecipe(id) {
      webApi.recipes.get(
        id,
        (data) => { this.sourceRecipe = data; },
        response => this.setApiFailureMessages(response),
      );
    },
    fetchRecipesList() {
      webApi.recipes.list(
        this.listRequest,
        data => this.setListResponse(data),
        response => this.setApiFailureMessages(response),
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
