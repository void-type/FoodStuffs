<template>
    <form v-on:keyup.ctrl.enter="saveRecipe(currentRecipe)">
        <h1>Edit Recipe</h1>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('name')}">
                <input type="text" id="name" name="name" v-model="name" />
                <label for="name">Name</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('ingredients')}">
                <textarea id="ingredients" name="ingredients" v-model="ingredients"></textarea>
                <label for="ingredients">Ingredients</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('directions')}">
                <textarea id="directions" name="directions" v-model="directions"></textarea>
                <label for="directions">Directions</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group" v-bind:class="{danger: isFieldInError('prepTimeMinutes')}">
                <input type="number" id="prepTimeMinutes" name="prepTimeMinutes" v-model="prepTimeMinutes" />
                <label for="prepTimeMinutes">Prep Time Minutes</label>
            </div>
            <div class="form-group" v-bind:class="{danger: isFieldInError('cookTimeMinutes')}">
                <input type="number" id="cookTimeMinutes" name="cookTimeMinutes" v-model="cookTimeMinutes" />
                <label for="cookTimeMinutes">Cook Time Minutes</label>
            </div>
        </div>
        <div class="form-row">
            <TagEditor v-bind:fieldName="'categories'"
                       v-bind:label="'Categories'"
                       v-bind:tags="categories"
                       v-bind:addTag="addCategoryToCurrentRecipe"
                       v-bind:removeTag="removeCategoryFromCurrentRecipe"
                       v-bind:class="{danger: isFieldInError('categories')}" />
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
    import TagEditor from "./EditorForTags";

    export default {
        computed: {
            ...mapGetters(["currentRecipe", "isFieldInError"]),
            name: {
                get() {
                    return this.$store.state.currentRecipe.name
                },
                set(value) {
                    this.$store.commit('setCurrentRecipeName', value)
                }
            },
            ingredients: {
                get() {
                    return this.$store.state.currentRecipe.ingredients
                },
                set(value) {
                    this.$store.commit('setCurrentRecipeIngredients', value)
                }
            },
            directions: {
                get() {
                    return this.$store.state.currentRecipe.directions
                },
                set(value) {
                    this.$store.commit('setCurrentRecipeDirections', value)
                }
            },
            prepTimeMinutes: {
                get() {
                    return this.$store.state.currentRecipe.prepTimeMinutes
                },
                set(value) {
                    this.$store.commit('setCurrentRecipePrepTimeMinutes', value)
                }
            },
            cookTimeMinutes: {
                get() {
                    return this.$store.state.currentRecipe.cookTimeMinutes
                },
                set(value) {
                    this.$store.commit('setCurrentRecipeCookTimeMinutes', value)
                }
            },
            categories: {
                get() {
                    return this.$store.state.currentRecipe.categories
                }
            }
        },
        methods: {
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
        components: {
            TagEditor
        }
    };
</script>

<style lang="scss" scoped>
    @import "./EditorForm.vue";
</style>