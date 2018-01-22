<template>
    <section>
        <RecipeTable></RecipeTable>
        <RecipeForm></RecipeForm>
    </section>
</template>

<script>
    import RecipeTable from "../components/RecipeTable";
    import RecipeForm from "../components/RecipeForm";

    export default {
        components: {
            RecipeTable,
            RecipeForm
        }
    };
</script>

<style lang="scss">
</style>