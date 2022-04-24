import { createRouter, createWebHistory, type RouteLocationNormalized } from 'vue-router';
import { nextTick } from 'vue';
import useAppStore from '@/stores/appStore';
import Home from '@/pages/Home.vue';
import RouterHelpers from '@/models/RouterHelpers';

function newRecipeProps(route: RouteLocationNormalized) {
  const oldRecipe = route.params;
  if (oldRecipe === undefined || oldRecipe === null) {
    return oldRecipe;
  }

  const newRecipe = { ...oldRecipe, id: 0, name: `${oldRecipe.name} Copy` };

  return { newRecipeSuggestion: newRecipe };
}

const router = createRouter({
  scrollBehavior: () => ({ left: 0, top: 0 }),
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: 'active',
  linkExactActiveClass: 'active',
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: { title: 'Home' },
    },
    {
      name: 'view',
      path: '/view/:id',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/View.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'View' },
    },
    {
      name: 'edit',
      path: '/edit/:id',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Edit.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'Edit' },
    },
    {
      name: 'new',
      path: '/new',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Edit.vue'),
      props: newRecipeProps,
      meta: { title: 'New' },
    },
    {
      name: 'search',
      path: '/search',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Search.vue'),
      props: (route) => ({ query: route.query }),
      meta: { title: 'Search' },
    },
    {
      path: '/cards',
      name: 'cards',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Cards.vue'),
      meta: { title: 'Cards' },
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: { name: 'home' },
      meta: { title: 'Home' },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const appStore = useAppStore();
  appStore.clearMessages();
  next();
});

router.afterEach((to) => {
  nextTick(() => RouterHelpers.setTitle(to));
});

export default router;
