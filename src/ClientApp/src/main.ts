import { config as fontAwesomeConfig, library } from '@fortawesome/fontawesome-svg-core';
import {
  faArrowUp,
  faBold,
  faCalendarDays,
  faCheck,
  faCircleInfo,
  faClock,
  faCloudArrowUp,
  faCode,
  faExpand,
  faFilter,
  faFire,
  faGripVertical,
  faHeading,
  faItalic,
  faKitchenSet,
  faLink,
  faLinkSlash,
  faListOl,
  faListUl,
  faMinus,
  faMoon,
  faPalette,
  faParagraph,
  faPen,
  faPlus,
  faRotate,
  faRotateLeft,
  faRotateRight,
  faRulerHorizontal,
  faSearch,
  faShoppingBasket,
  faSort,
  faSortDown,
  faSortUp,
  faStrikethrough,
  faTerminal,
  faThumbtack,
  faTimes,
  faUser,
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
  faCalendarDays,
  faCheck,
  faCircleInfo,
  faClock,
  faCloudArrowUp,
  faCode,
  faExpand,
  faFilter,
  faFire,
  faGripVertical,
  faHeading,
  faItalic,
  faKitchenSet,
  faLink,
  faLinkSlash,
  faListOl,
  faListUl,
  faMinus,
  faMoon,
  faPalette,
  faParagraph,
  faPen,
  faPlus,
  faRotate,
  faRotateLeft,
  faRotateRight,
  faRulerHorizontal,
  faSearch,
  faShoppingBasket,
  faSort,
  faSortDown,
  faSortUp,
  faStrikethrough,
  faTerminal,
  faThumbtack,
  faTimes,
  faUser,
  faUtensils,
);

const app = createApp(App);

app.use(createPinia()).use(router).mount('#app');
