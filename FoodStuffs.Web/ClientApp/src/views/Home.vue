<template>
    <section>
        <SelectSidebar />
        <RecipeViewer :currentRecipe="currentRecipe" />
    </section>
</template>

<script>
import { mapGetters } from 'vuex';
import SelectSidebar from '../components/SelectSidebar.vue';
import RecipeViewer from '../components/RecipeViewer.vue';

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
