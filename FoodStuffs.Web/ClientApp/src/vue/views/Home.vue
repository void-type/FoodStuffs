<template>
    <section>
        <SelectSidebar />
        <RecipeViewer />
    </section>
</template>

<script>
    import SelectSidebar from "../components/SelectSidebar";
    import RecipeViewer from "../components/RecipeViewer";

    export default {
        components: {
            SelectSidebar,
            RecipeViewer
        }
    };
</script>

<style lang="scss" scoped>
</style>