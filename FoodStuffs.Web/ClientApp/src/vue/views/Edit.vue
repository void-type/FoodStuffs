<template>
    <section>
        <RecipeTable></RecipeTable>
        <RecipeEditor></RecipeEditor>
    </section>
</template>

<script>
    import RecipeTable from "../components/RecipeTable";
    import RecipeEditor from "../components/RecipeEditor";

    export default {
        components: {
            RecipeTable,
            RecipeEditor
        }
    };
</script>

<style lang="scss" scoped>
</style>