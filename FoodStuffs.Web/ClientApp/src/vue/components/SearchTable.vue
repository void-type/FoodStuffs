<template>
    <table class="hover">
        <tbody>
            <tr v-for="recipe in recipes"
                v-bind:key="recipe.id"
                v-on:click="selectClick(recipe)">

                <td>{{recipe.name}}</td>
                <td>{{recipe.categories.join(", ")}}</td>
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
            ...mapActions(["selectRecipe"]),
            selectClick(recipe) {
                this.selectRecipe(recipe);
                this.$router.push({ name: "home" });
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SearchTable";
</style>