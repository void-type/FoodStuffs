import { createApp } from 'vue';
import { createPinia } from 'pinia';
import 'bootstrap';
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import App from '@/App.vue';
import router from '@/router';
import { library, config as fontAwesomeConfig } from '@fortawesome/fontawesome-svg-core';
import {
  faTimes,
  faThumbtack,
  faAlignJustify,
  faPencil,
  faPlus,
} from '@fortawesome/free-solid-svg-icons';

// Prevents inline styling to appease CSP.
fontAwesomeConfig.autoAddCss = false;

library.add(faTimes, faThumbtack, faAlignJustify, faPencil, faPlus);

const app = createApp(App);

app.use(createPinia()).use(router).mount('#app');
