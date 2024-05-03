import { createRouter, createWebHistory } from 'vue-router';
import RouterHelpers from '@/models/RouterHelpers';
import useMessageStore from '@/stores/messageStore';
import { Collapse } from 'bootstrap';

const router = createRouter({
  scrollBehavior(to, from, savedPosition) {
    if (to.hash) {
      document.getElementById(to.hash.slice(1))?.focus();
      return {
        el: to.hash,
      };
    }

    if (savedPosition) {
      return savedPosition;
    }

    document.getElementById('app')?.focus();

    return {
      el: '#app',
    };
  },
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: 'active',
  linkExactActiveClass: 'active',
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/pages/HomePage.vue'),
      meta: { title: 'Home' },
    },
    {
      name: 'recipeView',
      path: '/recipe/:id/:slug?',
      component: () => import('@/pages/RecipeViewPage.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'View Recipe' },
    },
    {
      name: 'recipeEdit',
      path: '/recipe-edit/:id/:slug?',
      component: () => import('@/pages/RecipeEditPage.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'Edit Recipe' },
    },
    {
      name: 'recipeNew',
      path: '/recipe-new',
      component: () => import('@/pages/RecipeEditPage.vue'),
      props: (route) => ({
        copy: +(route.query?.copy || 0),
      }),
      meta: { title: 'New Recipe' },
    },
    {
      name: 'recipeSearch',
      path: '/recipe-search',
      component: () => import('@/pages/RecipeSearchPage.vue'),
      props: (route) => ({ query: route.query }),
      meta: { title: 'Search Recipes' },
    },
    {
      name: 'planSearch',
      path: '/plan-search',
      component: () => import('@/pages/PlanSearchPage.vue'),
      meta: { title: 'Search Plans' },
    },
    {
      name: 'planEdit',
      path: '/plan-edit/:id/:slug?',
      component: () => import('@/pages/PlanEditPage.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'Edit Meal Set' },
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: { name: 'home' },
      meta: { title: 'Home' },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const messageStore = useMessageStore();
  messageStore.clearMessages();
  next();
});

router.afterEach((to) => {
  Collapse.getOrCreateInstance('#navbar-menu', { toggle: false }).hide();

  RouterHelpers.setTitle(to);
});

export default router;
