const View = () => import(/* webpackChunkName: "recipes" */ '../views/View.vue');
const Edit = () => import(/* webpackChunkName: "recipes" */ '../views/Edit.vue');
const Search = () => import(/* webpackChunkName: "recipes" */ '../views/Search.vue');

function newRecipeProps(route) {
  const oldRecipe = route.params.newRecipeSuggestion;
  if (oldRecipe === undefined || oldRecipe === null) {
    return oldRecipe;
  }

  const newRecipe = { ...oldRecipe, id: 0, name: `${oldRecipe.name} Copy` };

  return { newRecipeSuggestion: newRecipe };
}

export default [
  {
    name: 'view',
    path: '/view/:id',
    component: View,
    props: (route) => ({
      id: +route.params.id,
    }),
  },
  {
    name: 'edit',
    path: '/edit/:id',
    component: Edit,
    props: (route) => ({
      id: +route.params.id,
    }),
  },
  {
    name: 'new',
    path: '/new',
    component: Edit,
    props: newRecipeProps,
  },
  {
    name: 'search',
    path: '/search',
    component: Search,
    props: (route) => ({ query: route.query }),
  },
];
