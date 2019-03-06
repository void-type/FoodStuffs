<template>
  <section>
    <SelectSidebar :route-name="'view'" />
    <RecipeEditor
      :source-recipe="sourceRecipe"
      :is-field-in-error="isFieldInError"
      :on-save="onSave"
      :on-delete="onDelete" />
  </section>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import webApi from '../webApi';
import router from '../router';
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
      sourceRecipe: new webApi.recipes.GetResponse(),
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
      setSuccessMessage: 'app/setSuccessMessage',
      addToRecent: 'recipes/addToRecent',
      removeFromRecent: 'recipes/removeFromRecent',
      fetchList: 'recipes/fetchList',
    }),
    fetchRecipe(id) {
      if (this.id === 0) {
        this.sourceRecipe = new webApi.recipes.GetResponse();
        return;
      }
      webApi.recipes.get(
        id,
        (data) => { this.sourceRecipe = data; },
        response => webApi.setApiFailureMessage(response),
      );
    },
    onSave(recipe) {
      webApi.recipes.save(
        recipe,
        (data) => {
          this.fetchRecipe(this.id);
          router.push({ name: 'edit', params: { id: data.id } });
          this.fetchList();
          this.setSuccessMessage(data.message);
        },
        response => webApi.setApiFailureMessage(response),
      );
    },
    onDelete(id) {
      webApi.recipes.delete(
        id,
        (data) => {
          this.removeFromRecent(this.id);
          this.sourceRecipe = null;
          this.fetchList();
          router.push({ name: 'search' });
          this.setSuccessMessage(data.message);
        },
        response => webApi.setApiFailureMessage(response),
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
    if (this.sourceRecipe !== null) {
      this.addToRecent(this.sourceRecipe);
    }
    next();
  },
};
</script>

<style lang="scss" scoped>
</style>
