import '@babel/polyfill';
import BootstrapVue from 'bootstrap-vue';
import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';

Vue.use(BootstrapVue);

new Vue({
  render: h => h(App),
  router,
  store,
}).$mount('#app');
