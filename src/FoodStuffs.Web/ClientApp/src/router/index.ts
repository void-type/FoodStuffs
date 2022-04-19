import { createRouter, createWebHistory } from 'vue-router';
import RecipeCards from '@/pages/Cards.vue';

const router = createRouter({
  scrollBehavior: () => ({ left: 0, top: 0 }),
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: RecipeCards,
    },
    {
      path: '/:pathMatch(.*)*',
      component: () => import('../pages/Cards.vue'),
    },
  ],
});

export default router;
