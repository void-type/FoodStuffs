import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '@/store';
import Home from '@/views/Home.vue';
import recipes from './recipes';

Vue.use(VueRouter);

const router = new VueRouter({
  mode: 'history',
  linkActiveClass: 'active',
  linkExactActiveClass: 'active',
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
    },
    ...recipes,
    {
      path: '*',
      redirect: { name: 'home' },
    },
  ],
});

router.beforeEach((to, from, next) => {
  store.dispatch('app/clearMessages');
  next();
});

export default router;
