import { createApp } from 'vue';
import { createPinia } from 'pinia';
import 'bootstrap';
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import VueProgressBar from '@aacassandra/vue3-progressbar';

import App from '@/App.vue';
import router from '@/router';

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(VueProgressBar, {
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
});

app.mount('#app');
