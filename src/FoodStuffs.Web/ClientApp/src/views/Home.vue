<template>
  <section>
    <SelectSidebar />
    <RecipeViewer :recipe="currentRecipe" />
  </section>
</template>

<script>
import { mapGetters } from 'vuex';
import SelectSidebar from '../viewComponents/SelectSidebar.vue';
import RecipeViewer from '../viewComponents/RecipeViewer.vue';

export default {
  components: {
    SelectSidebar,
    RecipeViewer,
  },
  computed: {
    ...mapGetters(['currentRecipe']),
  },
};
</script>

<style lang="scss" scoped>
</style>
