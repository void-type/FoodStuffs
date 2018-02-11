<template>
    <section>
        <SearchControls />
        <SearchTable v-bind:recipes="recipes" />
        <SearchTablePager v-bind:totalCount="recipesTotalCount" v-bind:page="recipesPage" v-bind:take="recipesTake" />
    </section>
</template>

<script>
    import SearchControls from "../components/SearchControls";
    import SearchTable from "../components/SearchTable";
    import SearchTablePager from "../components/SearchTablePager";

    import { mapGetters } from "vuex";

    export default {
        computed: {
            ...mapGetters(["recipes", "recipesPage", "recipesTake", "recipesTotalCount"])
        },
        components: {
            SearchControls,
            SearchTable,
            SearchTablePager
        }
    };
</script>

<style lang="scss" scoped>
    @import "./Search.vue";
</style>