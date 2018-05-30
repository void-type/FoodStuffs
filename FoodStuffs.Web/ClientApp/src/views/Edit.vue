<template>
    <section>
        <SelectSidebar />
        <RecipeEditor />
    </section>
</template>

<script>
import SelectSidebar from '../components/SelectSidebar.vue';
import RecipeEditor from '../components/RecipeEditor.vue';

export default {
  components: {
    SelectSidebar,
    RecipeEditor,
  },
};
</script>

<style lang="scss" scoped>

</style>
