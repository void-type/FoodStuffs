import { createApp } from 'vue';
import { createPinia } from 'pinia';
import 'bootstrap';

import App from '@/App.vue';
import router from '@/router';

// TODO: progressbar?

const app = createApp(App);

app.use(createPinia());
app.use(router);

app.mount('#app');
