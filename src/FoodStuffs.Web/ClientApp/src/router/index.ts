import { createRouter, createWebHistory, type RouteLocationNormalized } from 'vue-router';
import Home from '@/pages/Home.vue';
import useAppStore from '@/stores/app';
import { nextTick } from 'vue';

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
    },
    {
      name: 'view',
      path: '/view/:id',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/View.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
    },
    {
      name: 'edit',
      path: '/edit/:id',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Edit.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
    },
    {
      name: 'new',
      path: '/new',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Edit.vue'),
      props: newRecipeProps,
    },
    {
      name: 'search',
      path: '/search',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Search.vue'),
      props: (route) => ({ query: route.query }),
    },
    {
      path: '/cards',
      name: 'cards',
      component: () => import(/* webpackChunkName: "recipes" */ '@/pages/Cards.vue'),
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: { name: 'home' },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const appStore = useAppStore();
  appStore.clearMessages();
  next();
});

router.afterEach(() => {
  nextTick(() => {
    const appStore = useAppStore();
    document.title = appStore.applicationName;
  });
});

export default router;
