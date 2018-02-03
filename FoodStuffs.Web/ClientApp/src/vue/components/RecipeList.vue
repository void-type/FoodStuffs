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
            <h3 class="text-center">Recent</h3>
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
    import { mapActions, mapGetters } from "vuex";

    export default {
        computed:
        {
            ...mapGetters([
                "recipes",
                "recentRecipes"
            ])
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
    @import "./RecipeList";
</style>