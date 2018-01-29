<template>
    <section>
        <RecipeList></RecipeList>
        <RecipeEditor></RecipeEditor>
    </section>
</template>

<script>
    import RecipeList from "../components/RecipeList";
    import RecipeEditor from "../components/RecipeEditor";

    export default {
        components: {
            RecipeList,
            RecipeEditor
        }
    };
</script>

<style lang="scss" scoped>
</style>