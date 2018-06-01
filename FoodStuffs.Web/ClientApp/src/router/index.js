import Vue from 'vue';
import Router from 'vue-router';
import HomeView from '../views/Home.vue';
import EditView from '../views/Edit.vue';
import SearchView from '../views/Search.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      alias: '/home',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/edit',
      name: 'edit',
      component: EditView,
    },
    {
      path: '/search',
      name: 'search',
      component: SearchView,
    },
    {
      path: '*',
      redirect: { name: 'home' },
    },
  ],
});
