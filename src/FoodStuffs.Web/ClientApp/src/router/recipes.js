const View = () => import(/* webpackChunkName: "recipes" */ '../views/View.vue');
const Edit = () => import(/* webpackChunkName: "recipes" */ '../views/Edit.vue');
const Search = () => import(/* webpackChunkName: "recipes" */ '../views/Search.vue');

export default [
  {
    alias: '/view',
    name: 'view',
    component: View,
  },
  {
    path: '/edit',
    name: 'edit',
    component: Edit,
  },
  {
    path: '/search',
    name: 'search',
    component: Search,
  },
];
