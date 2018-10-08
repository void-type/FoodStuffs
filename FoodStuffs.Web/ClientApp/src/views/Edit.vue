<template>
  <section>
    <SelectSidebar />
    <RecipeEditor />
  </section>
</template>

<script>
import SelectSidebar from '../viewComponents/SelectSidebar.vue';
import RecipeEditor from '../viewComponents/RecipeEditor.vue';

export default {
  components: {
    SelectSidebar,
    RecipeEditor,
  },
};
</script>

<style lang="scss" scoped>
</style>
