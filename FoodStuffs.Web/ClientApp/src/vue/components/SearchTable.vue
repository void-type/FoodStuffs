<template>
    <table>
        <thead>
            <tr>
                <th class="ptr"
                    @click="sortByNameClick()">
                    Name &nbsp;
                    <span v-html="sortSymbols[sortByNameDirection]"></span>
                </th>
                <th>Category</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="recipe in filteredRecipes"
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

    export default {
        data: function () {
            return {
                sortByNameDirection: 1,
                sortSymbols: ["&#x1F552;", "&#9660;", "&#9650;"]
            }
        },
        props: {
            recipes: {
                type: Array,
                required: true
            }
        },
        computed: {
            filteredRecipes() {
                let recipes = this.recipes.slice();

                if (this.sortByNameDirection > 0) {
                    recipes.sort(function (a, b) {
                        let x = a.name.toLowerCase();
                        let y = b.name.toLowerCase();
                        if (x < y) { return -1; }
                        if (x > y) { return 1; }
                        return 0;
                    });
                }

                if (this.sortByNameDirection > 1) {
                    recipes.reverse();
                }

                return recipes;
            }
        },
        methods: {
            ...mapActions(["selectRecipe"]),
            selectClick(recipe) {
                this.selectRecipe(recipe);
                this.$router.push({ name: "home" });
            },
            sortByNameClick() {
                this.sortByNameDirection = (this.sortByNameDirection + 1) % 3;
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SearchTable.vue";
</style>