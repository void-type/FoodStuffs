<template>
    <form v-on:keyup.ctrl.enter="saveClick(currentRecipe)">
        <h1>Edit Recipe</h1>
        <div class="form-group" v-bind:class="{danger: errorFields.indexOf('Name') > -1}">
            <label title="Name" for="name">Name</label>
            <input type="text" id="name" name="name" v-model="currentRecipe.name" />
        </div>
        <div class="form-group" v-bind:class="{danger: errorFields.indexOf('Ingredients') > -1}">
            <label title="Ingredients" for="ingredients">Ingredients</label>
            <textarea id="ingredients" name="ingredients" v-model="currentRecipe.ingredients"></textarea>
        </div>
        <div class="form-group" v-bind:class="{danger: errorFields.indexOf('Directions') > -1}">
            <label title="Directions" for="directions">Directions</label>
            <textarea id="directions" name="directions" v-model="currentRecipe.directions"></textarea>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('PrepTimeMinutes') > -1}">
                <label title="Prep Time Minutes" for="prepTimeMinutes">Prep Time Minutes</label>
                <input type="number" id="prepTimeMinutes" name="prepTimeMinutes" v-model="currentRecipe.prepTimeMinutes" />
            </div>
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('CookTimeMinutes') > -1}">
                <label title="Cook Time Minutes" for="cookTimeMinutes">Cook Time Minutes</label>
                <input type="number" id="cookTimeMinutes" name="cookTimeMinutes" v-model="currentRecipe.cookTimeMinutes" />
            </div>
        </div>
        <CategoryEditor v-bind:fieldName="'categories'" v-bind:fieldTitle="'Categories'" v-bind:categories="currentRecipe.categories" v-bind:class="{danger: errorFields.indexOf('Categories') > -1}"></CategoryEditor>
        <div class="form-group">
            <label></label>
            <button v-on:click.prevent="saveClick(currentRecipe)">
                Save
            </button>
            <button v-on:click.prevent="cancelClick()">
                Cancel
            </button>
            <button class="pull-right danger" v-on:click.prevent="deleteClick(currentRecipe)">
                Delete
            </button>
        </div>
    </form>
</template>

<script>
    import { mapActions } from "vuex"
    import CategoryEditor from "./CategoryEditor"

    export default {
        computed:
        {
            currentRecipe() {
                return this.$store.state.currentRecipe;
            },
            errorFields() {
                return this.$store.state.fieldsInError;
            }
        },
        methods:
        {
            cancelClick() {
                this.selectRecipe();
                this.refresh();
            },
            ...mapActions({
                deleteClick: "deleteRecipe",
                refresh: "fetchRecipes",
                saveClick: "saveRecipe",
            })
        },
        components:
        {
            CategoryEditor
        }
    }
</script>

<style lang="scss" scoped>
    @import 'RecipeForm';
</style>