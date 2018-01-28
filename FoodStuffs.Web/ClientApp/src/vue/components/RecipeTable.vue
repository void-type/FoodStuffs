<template>
    <div>
        <div>
            <button v-on:click="newRecipe()">New</button>
            <table class="hover">
                <tbody>
                    <tr v-for="recipe in recipes" v-bind:key="recipe.id" v-on:click="selectRecipe(recipe)">
                        <td>{{recipe.name}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-if="recentRecipes.length > 0">
            <h3>Recent</h3>
            <table class="hover">
                <tbody>
                    <tr v-for="recipe in recentRecipes" v-bind:key="recipe.id" v-on:click="selectRecipe(recipe)">
                        <td>{{recipe.name}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
    import { mapActions } from "vuex";

    export default {
        computed:
        {
            recipes() {
                return this.$store.state.recipes;
            },
            recentRecipes() {
                // TODO: Recents should store by Id. Use PostFeedback to reselect recipe.
                return this.$store.state.recentRecipes;
            }
        },
        methods: {
            ...mapActions({
                selectRecipe: "selectRecipe"
            }),
            newRecipe() {
                this.selectRecipe();
                this.$router.push({ name: "edit" });
            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "./RecipeTable";
</style>