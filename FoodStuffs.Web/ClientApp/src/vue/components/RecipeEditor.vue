<template>
    <form v-on:keyup.ctrl.enter="saveClick(currentRecipe)">
        <h1>Edit Recipe</h1>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('Name') > -1}">
                <input type="text" id="name" name="name" v-model="currentRecipe.name" />
                <label title="Name" for="name">Name</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('Ingredients') > -1}">
                <textarea id="ingredients" name="ingredients" v-model="currentRecipe.ingredients"></textarea>
                <label title="Ingredients" for="ingredients">Ingredients</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('Directions') > -1}">
                <textarea id="directions" name="directions" v-model="currentRecipe.directions"></textarea>
                <label title="Directions" for="directions">Directions</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('PrepTimeMinutes') > -1}">
                <input type="number" id="prepTimeMinutes" name="prepTimeMinutes" v-model="currentRecipe.prepTimeMinutes" />
                <label title="Prep Time Minutes" for="prepTimeMinutes">Prep Time Minutes</label>
            </div>
            <div class="form-group" v-bind:class="{danger: errorFields.indexOf('CookTimeMinutes') > -1}">
                <input type="number" id="cookTimeMinutes" name="cookTimeMinutes" v-model="currentRecipe.cookTimeMinutes" />
                <label title="Cook Time Minutes" for="cookTimeMinutes">Cook Time Minutes</label>
            </div>
        </div>
        <div class="form-row">
            <CategoryEditor v-bind:fieldName="'categories'" v-bind:fieldTitle="'Categories'" v-bind:categories="currentRecipe.categories" v-bind:class="{danger: errorFields.indexOf('Categories') > -1}"></CategoryEditor>
        </div>
        <div class="form-row button-row">
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
    import { mapActions } from "vuex";
    import CategoryEditor from "./CategoryEditor";

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
                this.refresh(this.currentRecipe.id);
                this.$router.push({ name: "home" });
            },
            ...mapActions({
                deleteClick: "deleteRecipe",
                refresh: "fetchRecipes",
                saveClick: "saveRecipe",
                selectRecipe: "selectRecipe"
            })
        },
        components:
        {
            CategoryEditor
        }
    }
</script>

<style lang="scss" scoped>
    @import "./RecipeEditor";
</style>