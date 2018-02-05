<template>
    <div>
        <div v-if="recentRecipes.length > 0">
            <h3 class="text-center">Recent</h3>
            <SelectSidebarList v-bind:recipes="recentRecipes" />
        </div>
        <div>
            <button v-on:click="newRecipe()">New</button>
            <SelectSidebarList v-bind:recipes="recipes" />
        </div>
    </div>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";
    import SelectSidebarList from "./SelectSidebarList";

    export default {
        computed: {
            ...mapGetters(["recipes", "recentRecipes"])
        },
        methods: {
            ...mapActions(["selectRecipe"]),
            newRecipe() {
                this.selectRecipe();
                this.$router.push({ name: "edit" });
            }
        },
        components: {
            SelectSidebarList
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SelectSidebar";
</style>