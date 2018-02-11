<template>
    <div v-if="currentRecipe.name">
        <h1>{{currentRecipe.name}}</h1>
        <h3>Ingredients</h3>
        <pre>{{currentRecipe.ingredients}}</pre>
        <h3>Directions</h3>
        <pre>{{currentRecipe.directions}}</pre>
        <hr />
        <p>
            <strong>Prep Time: </strong>{{currentRecipe.prepTimeMinutes}} minutes. <strong>Cook Time: </strong>{{currentRecipe.cookTimeMinutes}} minutes.<br />
            <strong>Categories: </strong><span class="categories"><span v-for="category in currentRecipe.categories" v-bind:key="category">{{category}}</span></span><br />
            <strong>Created By: </strong>{{currentRecipe.createdBy}}. <strong>Modified By: </strong>{{currentRecipe.modifiedBy}}<br />
            <strong>Created On: </strong>{{currentRecipe.createdOn | dateString}}. <strong>Modified On: </strong>{{currentRecipe.modifiedOn | dateString}}
        </p>
        <router-link v-bind:to="{name: 'edit'}" tag="button" class="no-print">Edit</router-link>
    </div>
</template>

<script>
    import dateString from "../../filters/dateString";
    import { mapGetters } from "vuex";

    export default {
        computed: {
            ...mapGetters(["currentRecipe"])
        },
        filters: {
            dateString: dateString
        }
    };
</script>

<style lang="scss" scoped>
    @import "./HomeViewer";
</style>