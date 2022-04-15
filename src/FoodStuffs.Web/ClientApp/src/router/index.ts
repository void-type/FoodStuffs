import { createRouter, createWebHistory } from 'vue-router'
import RecipeCards from '../views/RecipeCards.vue'

const router = createRouter({
  scrollBehavior: () => ({ left: 0, top: 0 }),
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: RecipeCards
    },
    {
      path: '/:pathMatch(.*)*',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/RecipeCards.vue')
    }
  ]
})

export default router
