<template>
    <div class="recipe-search">
        <form v-on:keyup.enter.prevent="fetchRecipes(search)">
            <div class="form-row">
                <div class="form-group">
                    <input type="text" id="searchName" name="searchName" v-model="search.name" />
                    <label for="searchName">Name Contains</label>
                </div>
                <div class="form-group">
                        <input type="text" id="searchCategory" name="searchCategory" v-model="search.category" />
                    <label for="searchCategory">Categories Contain</label>
                </div>
            </div>
            <div class="form-row button-row">
                <button v-on:click.prevent="fetchRecipes(search)">
                    Search
                </button>
                <button v-on:click.prevent="clearSearch(search)">
                    Clear All
                </button>
            </div>
        </form>

        <table class="hover">
            <tbody>
                <tr v-for="recipe in recipes" v-bind:key="recipe.id" v-on:click="selectClick(recipe)">
                    <td>{{recipe.name}}</td>
                    <td>{{recipe.categories.join(", ")}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";

    class SearchParameters {
        constructor() {
            this.name = "";
            this.category = "";
            this.take = null;
            this.page = null;
        }
    }

    export default {
        data: function () {
            return {
                search: new SearchParameters()
            }
        },
        computed: {
            ...mapGetters(["recipes"])
        },
        methods: {
            ...mapActions(["selectRecipe", "fetchRecipes"]),
            selectClick(recipe) {
                this.selectRecipe(recipe);
                this.$router.push({ name: "home" });
            },
            clearSearch() {
                this.search = new SearchParameters();
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./RecipeSearch";
</style>