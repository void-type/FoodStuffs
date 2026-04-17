import { config as fontAwesomeConfig, library } from '@fortawesome/fontawesome-svg-core';
import {
  faArrowUp,
  faBold,
  faCheck,
  faCode,
  faDiceThree,
  faExpand,
  faGripVertical,
  faHeading,
  faItalic,
  faLink,
  faLinkSlash,
  faListOl,
  faListUl,
  faMinus,
  faMoon,
  faPalette,
  faParagraph,
  faPlus,
  faRotate,
  faRotateLeft,
  faRotateRight,
  faRulerHorizontal,
  faSearch,
  faShoppingBasket,
  faStrikethrough,
  faTerminal,
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
  faBold,
  faCheck,
  faCode,
  faDiceThree,
  faExpand,
  faGripVertical,
  faHeading,
  faItalic,
  faLink,
  faLinkSlash,
  faListOl,
  faListUl,
  faMinus,
  faMoon,
  faPalette,
  faParagraph,
  faPlus,
  faRotate,
  faRotateLeft,
  faRotateRight,
  faRulerHorizontal,
  faSearch,
  faShoppingBasket,
  faStrikethrough,
  faTerminal,
  faThumbtack,
  faTimes,
  faUtensils,
);

const app = createApp(App);

app.use(createPinia()).use(router).mount('#app');
