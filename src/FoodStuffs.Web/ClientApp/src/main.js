import 'core-js/stable';
import Vue from 'vue';
import axios from 'axios';
import BootstrapVue from 'bootstrap-vue';
import VueProgressBar from 'vue-progressbar';
import App from './App.vue';
import router from './router';
import store from './store';

Vue.use(BootstrapVue);

Vue.use(
  VueProgressBar,
  {
    color: '#94F099',
    thickness: '2px',
    location: 'top',
    position: 'fixed',
    inverse: false,
    autoFinish: false,
    autoRevert: true,
    transition: {
      speed: '0.2s',
      opacity: '0.6s',
      termination: 300,
    },
  },
);

const app = new Vue({
  render: (h) => h(App),
  router,
  store,
}).$mount('#app');

const progress = app.$Progress;

axios.interceptors.request.use((config) => {
  progress.start();
  return config;
}, (error) => {
  progress.finish();
  return Promise.reject(error);
});

axios.interceptors.response.use((response) => {
  progress.finish();
  return response;
}, (error) => {
  progress.finish();
  return Promise.reject(error);
});
