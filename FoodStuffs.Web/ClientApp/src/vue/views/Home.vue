<template>
    <section>
        <RecipeList></RecipeList>
        <RecipeViewer></RecipeViewer>
    </section>
</template>

<script>
    import RecipeList from "../components/RecipeList";
    import RecipeViewer from "../components/RecipeViewer";

    export default {
        components: {
            RecipeList,
            RecipeViewer
        }
    };
</script>

<style lang="scss" scoped>
</style>