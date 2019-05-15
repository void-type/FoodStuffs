import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '../store';
import recipes from './recipes';

Vue.use(VueRouter);

const router = new VueRouter({
  mode: 'history',
  linkActiveClass: 'active',
  linkExactActiveClass: 'active',
  routes: [
    ...recipes,
    {
      path: '*',
      redirect: { name: 'search' },
    },
  ],
});

router.beforeEach((to, from, next) => {
  store.dispatch('app/clearMessages');
  next();
});

export default router;
