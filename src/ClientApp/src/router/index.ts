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
      name: 'view',
      path: '/view/:id',
      component: () => import('@/pages/RecipeViewPage.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'View' },
    },
    {
      name: 'edit',
      path: '/edit/:id',
      component: () => import('@/pages/RecipeEditPage.vue'),
      props: (route) => ({
        id: +route.params.id,
      }),
      meta: { title: 'Edit' },
    },
    {
      name: 'new',
      path: '/new',
      component: () => import('@/pages/RecipeEditPage.vue'),
      props: (route) => ({
        copy: +(route.query?.copy || 0),
      }),
      meta: { title: 'New' },
    },
    {
      name: 'search',
      path: '/search',
      component: () => import('@/pages/RecipeSearchPage.vue'),
      props: (route) => ({ query: route.query }),
      meta: { title: 'Search' },
    },
    {
      name: 'meals',
      path: '/meals',
      component: () => import('@/pages/MealsPage.vue'),
      meta: { title: 'Meals' },
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
  const element = `#navbar-menu`;
  Collapse.getOrCreateInstance(element).hide();
  RouterHelpers.setTitle(to);
});

export default router;
