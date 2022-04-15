import Vue from 'vue';
import Vuex from 'vuex';
import app from './modules/app';
import recipes from './modules/recipes';
import sidebar from './modules/sidebar';

Vue.use(Vuex);

const store = new Vuex.Store({
  strict: process.env.NODE_ENV !== 'production',
  modules: {
    app,
    recipes,
    sidebar,
  },
});

// Setup localStorage of recent recipes.
const storedRecentRecipes = localStorage.getItem('recentRecipes');

if (storedRecentRecipes) {
  store.commit('recipes/SET_RECENT_RECIPES', JSON.parse(storedRecentRecipes));
}

store.subscribe((mutation, state) => {
  if (mutation.type === 'recipes/SET_RECENT_RECIPES') {
    localStorage.setItem('recentRecipes', JSON.stringify(state.recipes.recentRecipes));
  }
});

export default store;
