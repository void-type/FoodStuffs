<template>
    <div>
        <input type="text" name="newCategoryName" v-model="newCategoryName"/><button v-on:keyup.tab="addCategory()" v-on:click.prevent="addCategory()">Add</button>
        <span class="tag" v-for="category in categories" v-bind:key="category">
            {{category}} &nbsp;
            <span class="tag-remove-button" v-on:click="removeCategory(category)">
            </span>
        </span>
    </div>
</template>

<script>
    import { mapMutations } from "vuex"

    export default {
        props: ["categories"],
        data: function () {
            return {
                newCategoryName: ""
            }
        },
        methods: {
            addCategory() {
                this.addCategoryToCurrentRecipe(this.newCategoryName);
                this.newCategoryName = "";
            },

            removeCategory(categoryToRemove) {
                this.removeCategoryFromCurrentRecipe(categoryToRemove);
            },

            ...mapMutations({
                addCategoryToCurrentRecipe: "addCategoryToCurrentRecipe",
                removeCategoryFromCurrentRecipe: "removeCategoryFromCurrentRecipe"
            })
        }
    }
</script>

<style lang="scss" scoped>
    @import 'CategoryEditor';
</style>