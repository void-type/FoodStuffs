import Vue from "vue"
import Vuex from "vuex"
import state from "./state"
import mutations from "./mutations"

Vue.use(Vuex);

export default new Vuex.Store({
  state,
  mutations
});

//// Old App state
//import axios from "axios";

//import RecipeTable from "./RecipeTable";
//import RecipeForm from "./RecipeForm";

//import appState from "../store/appState";
//import Recipe from "../models/createdRecipeViewModel";

//export default {
//  components: {
//    RecipeTable,
//    RecipeForm
//  },
//  created: function () {
//    this.list();
//  },
//  data: function () { return appState },
//  methods: {
//    list: function () {
//      axios.get("api/recipes/list")
//        .then(function (response) {
//          appState.recipes = response.data.items;
//        })
//        .catch(function () {
//          appState.isError = true;
//          appState.messages = error.response.data.items;
//        });
//    },

//    createRecipe: function (recipe) {
//      appState.messages = new Array();

//      axios.put("api/recipes", recipe)
//        .then(this.success)
//        .catch(this.failure);
//    },

//    updateRecipe: function (recipe) {
//      appState.messages = new Array();

//      axios.post("api/recipes", recipe)
//        .then(this.success)
//        .catch(this.failure);
//    },

//    deleteRecipe: function (recipe) {
//      appState.messages = new Array();

//      axios.delete("api/recipes", { params: { id: recipe.id } })
//        .then(this.success)
//        .catch(this.failure);
//    },

//    success: function (response) {
//      appState.isError = false;

//      appState.messages = [response.data.message];

//      appState.fieldsInError = new Array();

//      appRoot.currentRecipe = new Recipe();

//      this.list();
//    },

//    failure: function (error) {
//      appState.isError = true;

//      appState.messages = error.response.data.items.map(function (item) {
//        return item.errorMessage;
//      });

//      appState.fieldsInError = error.response.data.items.map(function (item) {
//        return item.fieldName;
//      });

//      this.list();
//    },

//    recipeSelected: function (recipe) {
//      this.clearErrors();
//      appRoot.currentRecipe = recipe;
//    },

//    clearErrors: function () {
//      appState.fieldsInError = new Array();
//      appState.messages = new Array();
//    }
//  }
//};

//// Recipe Form
//var Recipe = require("../models/createdRecipeViewModel.js");

//export default {
//  props: ["current-recipe", "fields-in-error"],
//  methods: {
//    saveClick: function () {
//      if (this.currentRecipe.id === undefined || this.currentRecipe.id < 1) {
//        this.$emit("create-recipe", this.currentRecipe);
//      } else {
//        this.$emit("update-recipe", this.currentRecipe);
//      }
//    },

//    deleteClick: function () {
//      this.$emit("delete-recipe", this.currentRecipe);
//    },

//    cancelClick: function () {
//      this.currentRecipe = new Recipe();
//    }
//  }
//}
