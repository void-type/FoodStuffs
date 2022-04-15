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
    component: () => import(/* webpackChunkName: "recipes" */ '../views/View.vue'),
    props: (route) => ({
      id: +route.params.id,
    }),
  },
  {
    name: 'edit',
    path: '/edit/:id',
    component: () => import(/* webpackChunkName: "recipes" */ '../views/Edit.vue'),
    props: (route) => ({
      id: +route.params.id,
    }),
  },
  {
    name: 'new',
    path: '/new',
    component: () => import(/* webpackChunkName: "recipes" */ '../views/Edit.vue'),
    props: newRecipeProps,
  },
  {
    name: 'search',
    path: '/search',
    component: () => import(/* webpackChunkName: "recipes" */ '../views/Search.vue'),
    props: (route) => ({ query: route.query }),
  },
];
