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
        <SearchTable v-bind:recipes="recipes" />
    </div>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";
    import ListSearch from "../../models/recipeListSearch";
    import SearchTable from "./SearchTable";

    export default {
        data: function () {
            return {
                search: new ListSearch()
            }
        },
        computed: {
            ...mapGetters(["recipes"])
        },
        methods: {
            ...mapActions(["fetchRecipes"]),
            clearSearch() {
                this.search = new ListSearch();
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