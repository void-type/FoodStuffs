require("./recipe-table.scss");

Vue.component("recipe-table", {
    template: require("./recipe-table.html"),
    props: ["recipes"],
    methods: {
        selectRecipe: function (recipe) {
            this.$emit("recipeSelected", recipe);
        },

        newRecipe: function () {
            this.$emit("recipeSelected", {});
        }
    }
});