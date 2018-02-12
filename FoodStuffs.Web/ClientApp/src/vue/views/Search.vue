<template>
    <section>
        <SearchControls :nameSearch="nameSearch"
                        :categorySearch="categorySearch"
                        @updateNameSearch="updateNameSearch"
                        @updateCategorySearch="updateCategorySearch"
                        @search="requestSearch"
                        @clear="clearSearch" />

        <SearchTable :recipes="recipesList"
                     :selectedSort="sort"
                     @updateSelectedSort="updateSelectedSort" />

        <SearchTablePager :page="recipesListPage"
                          :take="recipesListTake"
                          :totalCount="recipesListTotalCount"
                          @requestPage="requestPage" />
    </section>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";
    import SearchControls from "../components/SearchControls";
    import SearchTable from "../components/SearchTable";
    import SearchTablePager from "../components/SearchTablePager";

    export default {
        computed: {
            ...mapGetters(["recipesList", "recipesListPage", "recipesListTake", "recipesListTotalCount"]),
            nameSearch: {
                get() {
                    return this.$store.state.recipesSearchParameters.nameSearch
                },
                set(value) {
                    this.$store.commit('setRecipesSearchParametersNameSearch', value)
                }
            },
            categorySearch: {
                get() {
                    return this.$store.state.recipesSearchParameters.categorySearch
                },
                set(value) {
                    this.$store.commit('setRecipesSearchParametersCategorySearch', value)
                }
            },
            page: {
                get() {
                    return this.$store.state.recipesSearchParameters.page
                },
                set(value) {
                    this.$store.commit('setRecipesSearchParametersPage', value)
                }
            },
            sort: {
                get() {
                    return this.$store.state.recipesSearchParameters.sort
                },
                set(value) {
                    this.$store.commit('setRecipesSearchParametersSort', value)
                }
            }
        },
        components: {
            SearchControls,
            SearchTable,
            SearchTablePager
        },
        methods: {
            ...mapActions(["fetchRecipes"]),
            requestPage(pageNumber) {
                this.page = pageNumber;
                this.fetchRecipes();
            },
            requestSearch(nameSearch, categorySearch) {
                this.nameSearch = nameSearch;
                this.categorySearch = categorySearch;
                this.fetchRecipes();
            },
            clearSearch() {
                this.nameSearch = "";
                this.categorySearch = "";
                this.page = 1;
                this.fetchRecipes();
            },
            updateNameSearch(value) {
                this.nameSearch = value;
            },
            updateCategorySearch(value) {
                this.categorySearch = value;
            },
            updateSelectedSort(value) {
                this.sort = value;
                this.fetchRecipes();
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./Search.vue";
</style>