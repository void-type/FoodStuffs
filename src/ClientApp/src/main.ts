import { config as fontAwesomeConfig, library } from '@fortawesome/fontawesome-svg-core';
import {
  faArrowUp,
  faCheck,
  faDiceThree,
  faGripVertical,
  faMinus,
  faMoon,
  faPalette,
  faPlus,
  faRotate,
  faRotateRight,
  faSearch,
  faShoppingBasket,
  faThumbtack,
  faTimes,
  faUtensils,
} from '@fortawesome/free-solid-svg-icons';
import { createPinia } from 'pinia';
import { createApp } from 'vue';
import App from '@/App.vue';
import router from '@/router';
import 'bootstrap';
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
  faShoppingBasket,
);

const app = createApp(App);

app.use(createPinia()).use(router).mount('#app');
