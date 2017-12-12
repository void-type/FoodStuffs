require("./recipe-form.scss");

Vue.component("recipe-form", {
    template: require("./recipe-form.html"),
    props: ["currentRecipe", "fieldsInError"],
    methods: {
        save: function () {
            if (currentRecipe.id === undefined || currentRecipe.id < 1) {
                this.$emit("createRecipe", currentRecipe);
            } else {
                this.$emit("updateRecipe", currentRecipe);
            }
        },

        delete: function () {
            this.$emit("deleteRecipe", currentRecipe);
        },

        cancel: function () {
            this.currentRecipe = null;
        }
    }
});