<template>
  <div>
    <b-row>
      <b-col
        md="12"
        lg="3"
        class="no-print"
      >
        <SelectSidebar :route-name="'view'" />
      </b-col>
      <b-col>
        <RecipeViewer :recipe="sourceRecipe" />
      </b-col>
    </b-row>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import webApi from '../webApi';
import recipeModels from '../models/recipesApiModels';
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
