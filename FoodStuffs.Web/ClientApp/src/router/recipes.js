const HomeView = () => import(/* webpackChunkName: "recipes" */ '../views/Home.vue');
const EditView = () => import(/* webpackChunkName: "recipes" */ '../views/Edit.vue');
const SearchView = () => import(/* webpackChunkName: "recipes" */ '../views/Search.vue');

export default [
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
];
