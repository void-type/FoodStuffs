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
      meta: { title: 'Home' },
      children: [
        {
          path: '',
          name: 'home',
          component: () => import('@/pages/HomePage.vue'),
        },
        {
          path: 'recipes',
          meta: { title: 'Recipes' },
          children: [
            {
              path: '',
              name: 'recipeSearch',
              component: () => import('@/pages/RecipeSearchPage.vue'),
              props: (route) => ({ query: route.query }),
            },
            {
              path: 'view/:id/:slug?',
              name: 'recipeView',
              meta: { title: 'View Recipe' },
              component: () => import('@/pages/RecipeViewPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
            },
            {
              path: 'edit/:id/:slug?',
              name: 'recipeEdit',
              meta: { title: 'Edit Recipe' },
              component: () => import('@/pages/RecipeEditPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
            },
            {
              path: 'new',
              name: 'recipeNew',
              meta: { title: 'New Recipe' },
              component: () => import('@/pages/RecipeEditPage.vue'),
              props: (route) => ({
                copy: +(route.query?.copy || 0),
              }),
            },
          ],
        },
        {
          path: 'meal-plans',
          meta: { title: 'Meal Plans' },
          children: [
            {
              path: '',
              name: 'mealPlanList',
              component: () => import('@/pages/MealPlanListPage.vue'),
              props: (route) => ({ query: route.query }),
            },
            {
              path: 'edit/:id',
              name: 'mealPlanEdit',
              meta: { title: 'Edit Meal Plan' },
              component: () => import('@/pages/MealPlanEditPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
            },
            {
              path: 'new',
              name: 'mealPlanNew',
              meta: { title: 'New Meal Plan' },
              component: () => import('@/pages/MealPlanEditPage.vue'),
            },
          ],
        },
        {
          path: 'shopping-items',
          meta: { title: 'Shopping Items' },
          children: [
            {
              path: '',
              name: 'shoppingItemList',
              component: () => import('@/pages/ShoppingItemListPage.vue'),
              props: (route) => ({ query: route.query }),
            },
            {
              path: 'edit/:id',
              name: 'shoppingItemEdit',
              meta: { title: 'Edit Shopping Item' },
              component: () => import('@/pages/ShoppingItemEditPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
            },
            {
              path: 'new',
              name: 'shoppingItemNew',
              meta: { title: 'New Shopping Item' },
              component: () => import('@/pages/ShoppingItemEditPage.vue'),
            },
          ],
        },
      ],
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
