require("./recipe-table.scss");
var Recipe = require("../../store/recipe.js");

Vue.component("recipe-table", {
    template: require("./recipe-table.html"),
    props: ["recipes"],
    methods: {
        selectRecipe: function (recipe) {
            this.$emit("recipe-selected", recipe);
        },

        newRecipe: function () {
            this.$emit("recipe-selected", new Recipe());
        }
    }
});