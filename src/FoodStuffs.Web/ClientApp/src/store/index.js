import Vue from 'vue';
import Vuex from 'vuex';
import app from './modules/app';
import recipes from './modules/recipes';
import sidebar from './modules/sidebar';

Vue.use(Vuex);

export default new Vuex.Store({
  strict: process.env.NODE_ENV !== 'production',
  modules: {
    app,
    recipes,
    sidebar,
  },
});
