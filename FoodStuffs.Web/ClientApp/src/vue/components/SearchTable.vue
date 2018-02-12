<template>
    <table>
        <thead>
            <tr>
                <th class="ptr"
                    @click="sortByNameClick()">
                    Name &nbsp;
                    <span v-html="selectedSortSymbol"></span>
                </th>
                <th>Category</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="recipe in recipes"
                :key="recipe.id"
                @click="selectClick(recipe)">
                <td>{{recipe.name}}</td>
                <td>{{recipe.categories.join(", ")}}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import { mapActions } from "vuex";
    import sortTypes from "../../models/recipeSearchSortTypes";

    export default {
        data: function () {
            return {
                sortTypes: sortTypes
            }
        },
        props: {
            recipes: {
                type: Array,
                required: true
            },
            selectedSort: {
                type: String,
                required: true
            }
        },
        computed: {
            selectedSortSymbol() {
                return this.sortTypes.filter(type => type.name === this.selectedSort)[0].symbol;
            }
        },
        methods: {
            ...mapActions(["selectRecipe"]),
            selectClick(recipe) {
                this.selectRecipe(recipe);
                this.$router.push({ name: "home" });
            },
            sortByNameClick() {
                const currentSortId = this.sortTypes.filter(type => type.name === this.selectedSort)[0].id;
                const newSortId = (currentSortId + 1) % this.sortTypes.length;
                const newSortName = this.sortTypes.filter(type => type.id === newSortId)[0].name

                this.$emit("updateSelectedSort", newSortName);
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SearchTable.vue";
</style>