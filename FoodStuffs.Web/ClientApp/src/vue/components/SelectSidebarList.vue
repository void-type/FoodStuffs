<template>
    <table>
        <thead v-if="title !== null">
            <tr>
                <th>{{title}}</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="recipe in recipes"
                :key="recipe.id"
                @click="selectRecipe(recipe)">

                <td>{{recipe.name}}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import { mapActions } from "vuex";

    export default {
        props: {
            recipes: {
                type: Array,
                required: true
            },
            title: {
                type: String,
                required: false
            }
        },
        methods: {
            ...mapActions(["selectRecipe"])
        }
    };
</script>

<style lang="scss" scoped>
    @import "./SelectSidebarList.vue";
</style>