<template>
    <section>
        <SelectSidebar />
        <HomeViewer :currentRecipe="currentRecipe" />
    </section>
</template>

<script>
import { mapGetters } from 'vuex';
import SelectSidebar from '../components/SelectSidebar.vue';
import HomeViewer from '../components/HomeViewer.vue';

export default {
  components: {
    SelectSidebar,
    HomeViewer,
  },
  computed: {
    ...mapGetters(['currentRecipe']),
  },
};
</script>

<style lang="scss" scoped>

</style>
