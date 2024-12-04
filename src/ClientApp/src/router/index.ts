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
          props: (route) => ({ query: route.query }),
          children: [
            {
              path: '',
              name: 'recipeSearch',
              component: () => import('@/pages/RecipeSearchPage.vue'),
            },
            {
              path: 'view/:id/:slug?',
              name: 'recipeView',
              component: () => import('@/pages/RecipeViewPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
              meta: { title: 'View Recipe' },
            },
            {
              path: 'edit/:id/:slug?',
              name: 'recipeEdit',
              component: () => import('@/pages/RecipeEditPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
              meta: { title: 'Edit Recipe' },
            },
            {
              path: 'new',
              name: 'recipeNew',
              component: () => import('@/pages/RecipeEditPage.vue'),
              props: (route) => ({
                copy: +(route.query?.copy || 0),
              }),
              meta: { title: 'New Recipe' },
            },
          ],
        },
        {
          path: 'meal-plans',
          meta: { title: 'Meal Plans' },
          props: (route) => ({ query: route.query }),
          children: [
            {
              path: '',
              name: 'mealPlanList',
              component: () => import('@/pages/MealPlanListPage.vue'),
            },
            {
              path: 'edit/:id',
              name: 'mealPlanEdit',
              component: () => import('@/pages/MealPlanEditPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
              meta: { title: 'Edit Meal Plan' },
            },
            {
              path: 'new',
              name: 'mealPlanNew',
              component: () => import('@/pages/MealPlanEditPage.vue'),
              meta: { title: 'New Meal Plan' },
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
              component: () => import('@/pages/ShoppingItemEditPage.vue'),
              props: (route) => ({
                id: +route.params.id,
              }),
              meta: { title: 'Edit Shopping Item' },
            },
            {
              path: 'new',
              name: 'shoppingItemNew',
              component: () => import('@/pages/ShoppingItemEditPage.vue'),
              meta: { title: 'New Shopping Item' },
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
