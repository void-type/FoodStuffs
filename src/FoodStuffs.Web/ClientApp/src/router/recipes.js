const View = () => import(/* webpackChunkName: "recipes" */ '../views/View.vue');
const Edit = () => import(/* webpackChunkName: "recipes" */ '../views/Edit.vue');
const Search = () => import(/* webpackChunkName: "recipes" */ '../views/Search.vue');

export default [
  {
    name: 'view',
    component: View,
    path: '/view/:id',
    props: route => ({
      id: +route.params.id,
    }),
  },
  {
    name: 'edit',
    component: Edit,
    path: '/edit/:id',
    props: route => ({
      id: +route.params.id,
    }),
  },
  {
    path: '/new',
    name: 'new',
    component: Edit,
  },
  {
    path: '/search',
    name: 'search',
    component: Search,
  },
];
