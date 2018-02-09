<template>
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
            <div class="form-group button-group">
                <div>
                    <button v-on:click.prevent="fetchRecipes(search)">
                        Search
                    </button>
                    <button v-on:click.prevent="clearSearch(search)">
                        Clear All
                    </button>
                </div>
            </div>
        </div>
    </form>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";
    import RecipeSearchParameters from "../../models/recipeSearchParameters";
    import SearchTable from "./SearchTable";

    export default {
        data: function () {
            return {
                search: new RecipeSearchParameters(),
            }
        },
        computed: {
            ...mapGetters(["recipes"])
        },
        methods: {
            ...mapActions(["fetchRecipes"]),
            clearSearch() {
                this.search = new RecipeSearchParameters();
                this.fetchRecipes(this.search);
            }
        },
        components: {
            SearchTable
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SearchControls";
</style>