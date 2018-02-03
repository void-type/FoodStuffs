<template>
    <form v-on:keyup.ctrl.enter="saveRecipe(currentRecipe)">
        <h1>Edit Recipe</h1>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('name')}">
                <input type="text" id="name" name="name" v-model="currentRecipe.name" />
                <label for="name">Name</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('ingredients')}">
                <textarea id="ingredients" name="ingredients" v-model="currentRecipe.ingredients"></textarea>
                <label for="ingredients">Ingredients</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('directions')}">
                <textarea id="directions" name="directions" v-model="currentRecipe.directions"></textarea>
                <label for="directions">Directions</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('prepTimeMinutes')}">
                <input type="number" id="prepTimeMinutes" name="prepTimeMinutes" v-model="currentRecipe.prepTimeMinutes" />
                <label for="prepTimeMinutes">Prep Time Minutes</label>
            </div>
            <div class="form-group" v-bind:class="{danger: isFieldInError('cookTimeMinutes')}">
                <input type="number" id="cookTimeMinutes" name="cookTimeMinutes" v-model="currentRecipe.cookTimeMinutes" />
                <label for="cookTimeMinutes">Cook Time Minutes</label>
            </div>
        </div>
        <div class="form-row">
            <TagEditor v-bind:fieldName="'categories'" v-bind:label="'Categories'" v-bind:tags="currentRecipe.categories"
                       v-bind:addTag="addCategoryToCurrentRecipe" v-bind:removeTag="removeCategoryFromCurrentRecipe"
                       v-bind:class="{danger: isFieldInError('categories')}">
            </TagEditor>
        </div>
        <div class="form-row button-row">
            <button v-on:click.prevent="saveRecipe(currentRecipe)">
                Save
            </button>
            <button v-on:click.prevent="cancelClick()">
                Cancel
            </button>
            <button class="pull-right danger" v-on:click.prevent="deleteRecipe(currentRecipe)">
                Delete
            </button>
        </div>
    </form>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";
    import TagEditor from "./TagEditor";

    export default {
        computed:
        {
            ...mapGetters([
                "currentRecipe",
                "isFieldInError"
            ])
        },
        methods:
        {
            cancelClick() {
                this.fetchRecipes(this.currentRecipe.id);
                this.$router.push({ name: "home" });
            },
            ...mapActions([
                "deleteRecipe",
                "fetchRecipes",
                "saveRecipe",
                "addCategoryToCurrentRecipe",
                "removeCategoryFromCurrentRecipe"
            ])
        },
        components:
        {
            TagEditor
        }
    }
</script>

<style lang="scss" scoped>
    @import "./RecipeEditor";
</style>