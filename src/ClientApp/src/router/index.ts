import { createRouter, createWebHistory } from 'vue-router';
import useAppStore from '@/stores/appStore';
import RouterHelpers from '@/models/RouterHelpers';

const router = createRouter({
  scrollBehavior: (to) => {
    if (to.hash) {
      document.getElementById(to.hash.slice(1))?.focus();
      return { el: to.hash };
    }

    document.getElementById('app-template')?.focus();
    return { left: 0, top: 0 };
  },
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: 'active',
  linkExactActiveClass: 'active',
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/pages/AppHome.vue'),
      meta: { title: 'Home' },
    },
    {
      name: 'view',
      path: '/view/:id',
      component: () => import('@/pages/RecipeView.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'View' },
    },
    {
      name: 'edit',
      path: '/edit/:id',
      component: () => import('@/pages/RecipeEdit.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'Edit' },
    },
    {
      name: 'new',
      path: '/new',
      component: () => import('@/pages/RecipeEdit.vue'),
      props: RouterHelpers.newRecipeProps,
      meta: { title: 'New' },
    },
    {
      name: 'search',
      path: '/search',
      component: () => import('@/pages/RecipeSearch.vue'),
      props: (route) => ({ query: route.query }),
      meta: { title: 'Search' },
    },
    {
      path: '/cards',
      name: 'cards',
      component: () => import('@/pages/MealCards.vue'),
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
  RouterHelpers.setTitle(to);
});

export default router;
