<template>
    <section>
        <SearchControls />
        <SearchTable v-bind:recipes="recipes" />
    </section>
</template>

<script>
    import SearchControls from "../components/SearchControls";
    import SearchTable from "../components/SearchTable";

    import { mapGetters } from "vuex";

    export default {
        computed: {
            ...mapGetters(["recipes"])
        },
        components: {
            SearchControls,
            SearchTable
        }
    };
</script>

<style lang="scss" scoped>
    @import "./Search.vue";
</style>