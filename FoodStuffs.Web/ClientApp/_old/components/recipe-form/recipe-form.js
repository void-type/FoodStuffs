require("./recipe-form.scss");
var Recipe = require("../../store/createdRecipeModel.js");

Vue.component("recipe-form", {
    template: require("./recipe-form.html"),
    props: ["current-recipe", "fields-in-error"],
    methods: {
        saveClick: function () {
            if (this.currentRecipe.id === undefined || this.currentRecipe.id < 1) {
                this.$emit("create-recipe", this.currentRecipe);
            } else {
                this.$emit("update-recipe", this.currentRecipe);
            }
        },

        deleteClick: function () {
            this.$emit("delete-recipe", this.currentRecipe);
        },

        cancelClick: function () {
            this.currentRecipe = new Recipe();
        }
    }
});