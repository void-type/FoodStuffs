<template>
    <form v-on:keyup.ctrl.enter="saveClick(currentRecipe)" >
        <h1>Edit Recipe</h1>
        <div class="form-group">
            <label title="Name">Name</label>
            <input type="text" name="name" v-model="currentRecipe.name" />
        </div>
        <div class="form-group">
            <label title="Ingredients">Ingredients</label>
            <textarea name="ingredients" v-model="currentRecipe.ingredients"></textarea>
        </div>
        <div class="form-group">
            <label title="Directions">Directions</label>
            <textarea name="directions" v-model="currentRecipe.directions"></textarea>
        </div>
        <div class="form-group">
            <label title="Prep Time (minutes)">Prep Time (minutes)</label>
            <input type="number" name="prepTimeMinutes" v-model="currentRecipe.prepTimeMinutes" />
        </div>
        <div class="form-group">
            <label title="Cook Time (minutes)">Cook Time (minutes)</label>
            <input type="number" name="cookTimeMinutes" v-model="currentRecipe.cookTimeMinutes" />
        </div>
        <div class="form-group">
            <label title="Categories">Categories</label>
            <CategoryEditor :categories="currentRecipe.categories"></CategoryEditor>
        </div>
        <div class="form-group">
            <label></label>
            <button v-on:click.prevent="saveClick(currentRecipe)">
                Save
            </button>
            <button v-on:click.prevent="cancelClick()">
                Cancel
            </button>
            <button class="pull-right error" v-on:click.prevent="deleteClick(currentRecipe)">
                Delete
            </button>
        </div>
    </form>
</template>

<script>
    import { mapActions, mapMutations } from "vuex"
    import CategoryEditor from "./CategoryEditor"

    export default {
        computed:
        {
            currentRecipe() {
                return this.$store.state.currentRecipe;
            }
        },
        methods:
        {
            ...mapActions({
                saveClick: "saveRecipe",
                deleteClick: "deleteRecipe",
                cancelClick: "selectNewRecipe",
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