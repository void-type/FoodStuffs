import axios from 'axios';
import BootstrapVue from 'bootstrap-vue';
import Vue from 'vue';
import VueProgressBar from 'vue-progressbar';

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

function setupProgressbarHooks(progress) {
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
}

export default {
  setup(app) {
    setupProgressbarHooks(app.$Progress);
  },
};
