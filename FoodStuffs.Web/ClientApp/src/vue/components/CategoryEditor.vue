<template>
    <div class="form-group">
        <div>
            <div>
                <input type="text" v-bind:id="fieldName" v-bind:name="fieldName" v-model="newCategoryName" />
                <button v-on:keyup.tab="addCategory()" v-on:click.prevent="addCategory()">Add</button>
            </div>
            <div class="tags">
                <span v-for="category in categories" v-bind:key="category">
                    {{category}} &nbsp;
                    <span v-on:click="removeCategory(category)">
                        &#x2716
                    </span>
                </span>
            </div>
        </div>
        <label v-bind:title="fieldTitle" v-bind:for="fieldName">{{fieldTitle}}</label>
    </div>
</template>

<script>
    import { mapActions } from "vuex";

    export default {
        props: ["fieldName", "fieldTitle", "categories"],
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

            ...mapActions({
                addCategoryToCurrentRecipe: "addCategoryToCurrentRecipe",
                removeCategoryFromCurrentRecipe: "removeCategoryFromCurrentRecipe"
            })
        }
    }
</script>

<style lang="scss" scoped>
    @import "./CategoryEditor";
</style>