<template>
    <table class="hover">
        <tbody>
            <tr v-for="recipe in recipes"
                v-bind:key="recipe.id"
                v-on:click="selectRecipe(recipe)">

                <td>{{recipe.name}}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import { mapActions } from "vuex";

    export default {
        props: {
            recipes: {
                type: Array,
                required: true
            }
        },
        methods: {
            ...mapActions(["selectRecipe"])
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SelectSidebarList";
</style>