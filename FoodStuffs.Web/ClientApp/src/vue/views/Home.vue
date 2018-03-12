<template>
    <section>
        <SelectSidebar />
        <HomeViewer :currentRecipe="currentRecipe" />
    </section>
</template>

<script>
import { mapGetters } from "vuex";
import SelectSidebar from "../components/SelectSidebar";
import HomeViewer from "../components/HomeViewer";

export default {
  components: {
    SelectSidebar,
    HomeViewer
  },
  computed: {
    ...mapGetters(["currentRecipe"])
  }
};
</script>

<style lang="scss" scoped>

</style>