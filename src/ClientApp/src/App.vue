<script lang="ts" setup>
import { onMounted } from 'vue';
import { RouterView, useRoute } from 'vue-router';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import AppHeader from '@/components/AppHeader.vue';
import AppNav from '@/components/AppNav.vue';
import AppFooter from '@/components/AppFooter.vue';
import AppMessageCenter from '@/components/AppMessageCenter.vue';
import RouterHelpers from '@/models/RouterHelpers';

const appStore = useAppStore();

const { clearMessages } = appStore;

const route = useRoute();

onMounted(() => {
  new Api()
    .appInfoList()
    .then((response) => {
      appStore.setApplicationInfo(response.data);
      RouterHelpers.setTitle(route);
    })
    .catch((response) => appStore.setApiFailureMessages(response));

  new Api().appVersionList().then((response) => appStore.setVersionInfo(response.data));
});
</script>

<template>
  <div id="app-inner" tabindex="-1" @keydown.stop.prevent.esc="clearMessages()">
    <div class="container-xxl visually-hidden-focusable">
      <router-link class="d-inline-flex p-2 m-1" :to="{ hash: '#main', query: route.query }"
        >Skip to main content</router-link
      >
    </div>
    <AppHeader class="d-print-none">
      <template #navItems>
        <AppNav />
      </template>
    </AppHeader>
    <AppMessageCenter class="d-print-none" />
    <main id="main" class="mb-4" tabindex="-1">
      <RouterView />
    </main>
    <AppFooter />
  </div>
</template>

<style lang="scss">
@import '@/styles/theme';
@import 'bootstrap/scss/bootstrap';

// Sticky footer
html,
body,
#app,
#app-inner {
  height: 100%;
}
#app-inner {
  display: flex;
  flex-direction: column;
}
main {
  flex: 1 0 auto;
}
footer {
  flex-shrink: 0;
}

// Modal background
.modal-content {
  background: $body-bg;
}

// Minimum button width
Button animation input[type='button'].btn,
input[type='submit'].btn,
button.btn,
a.btn,
.btn {
  min-width: 5rem;
}

// Colored headings
h1,
h2,
h3,
h4,
h5,
h6,
.h1,
.h2,
.h3,
.h4,
.h5,
.h6 {
  color: $primary;
}

// Printable screens
@media print {
  div {
    background-color: $white;
  }

  button,
  .btn {
    display: none;
  }

  p,
  pre,
  h1,
  h2,
  h3,
  h4 {
    color: $black;
  }
}
</style>
