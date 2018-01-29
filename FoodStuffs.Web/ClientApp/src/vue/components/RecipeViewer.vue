<template>
    <div v-if="currentRecipe.name">
        <h1>{{currentRecipe.name}}</h1>
        <h2>Ingredients</h2>
        <pre>{{currentRecipe.ingredients}}</pre>
        <h2>Directions</h2>
        <pre>{{currentRecipe.directions}}</pre>
        <hr />
        <p>
            <strong>Prep Time: </strong>{{currentRecipe.prepTimeMinutes}} minutes. <strong>Cook Time: </strong>{{currentRecipe.cookTimeMinutes}} minutes.<br />
            <strong>Categories: </strong><span class="category" v-for="category in currentRecipe.categories" v-bind:key="category">{{category}}</span><br />
            <strong>Created By: </strong>{{currentRecipe.createdBy}}. <strong>Modified By: </strong>{{currentRecipe.modifiedBy}}<br />
            <strong>Created On: </strong>{{currentRecipe.createdOn | dateString}}. <strong>Modified On: </strong>{{currentRecipe.modifiedOn | dateString}}
        </p>
        <div class="button-row">
            <router-link v-bind:to="{name: 'edit'}" tag="button" class="no-print">Edit</router-link>
        </div>
    </div>
</template>

<script>
    import dateString from "../../filters/dateString";

    export default {
        computed:
        {
            currentRecipe() {
                return this.$store.state.currentRecipe;
            }
        },
        filters: {
            dateString: dateString
        }
    }
</script>

<style lang="scss" scoped>
    @import "./RecipeViewer";
</style>