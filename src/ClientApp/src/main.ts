import { createApp } from 'vue';
import { createPinia } from 'pinia';
import 'bootstrap';
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import App from '@/App.vue';
import router from '@/router';
import { library, config as fontAwesomeConfig } from '@fortawesome/fontawesome-svg-core';
import {
  faArrowUp,
  faRotateRight,
  faMoon,
  faSearch,
  faThumbtack,
  faRotate,
  faGripVertical,
  faPlus,
  faMinus,
  faTimes,
  faCheck,
  faDiceThree,
  faPalette,
  faUtensils,
  faShoppingBasket,
} from '@fortawesome/free-solid-svg-icons';
import 'vue-color/style.css';

// Prevents inline styling to appease CSP.
fontAwesomeConfig.autoAddCss = false;

library.add(
  faArrowUp,
  faRotateRight,
  faMoon,
  faSearch,
  faThumbtack,
  faRotate,
  faGripVertical,
  faPlus,
  faMinus,
  faTimes,
  faCheck,
  faDiceThree,
  faPalette,
  faUtensils,
  faShoppingBasket
);

const app = createApp(App);

app.use(createPinia()).use(router).mount('#app');
