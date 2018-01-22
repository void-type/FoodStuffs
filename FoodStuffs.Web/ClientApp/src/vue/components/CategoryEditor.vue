<template>
    <div>
        <input type="text" v-bind:id="name" v-bind:name="name" v-model="newCategoryName" /><button v-on:keyup.tab="addCategory()" v-on:click.prevent="addCategory()">Add</button>
        <span class="tag" v-for="category in categories" v-bind:key="category">
            {{category}} &nbsp;
            <span class="tag-remove-button" v-on:click="removeCategory(category)">
                &#x2716
            </span>
        </span>
    </div>
</template>

<script>
    import { mapMutations } from "vuex"

    export default {
        props: ["name", "categories"],
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