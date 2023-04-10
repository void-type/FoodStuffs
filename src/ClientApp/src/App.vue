<script lang="ts" setup>
import { onMounted } from 'vue';
import { RouterView, useRoute } from 'vue-router';
import useAppStore from '@/stores/appStore';
import AppHeader from '@/components/AppHeader.vue';
import AppNav from '@/components/AppNav.vue';
import AppFooter from '@/components/AppFooter.vue';
import AppMessageCenter from '@/components/AppMessageCenter.vue';
import RouterHelpers from '@/models/RouterHelpers';
import AppModal from './components/AppModal.vue';
import ApiHelpers from './models/ApiHelpers';
import useRecipeStore from './stores/recipeStore';
import DarkModeHelpers from './models/DarkModeHelpers';
import RecipeStoreHelpers from './models/RecipeStoreHelpers';

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const route = useRoute();
const api = ApiHelpers.client;

onMounted(() => {
  appStore.setDarkMode(DarkModeHelpers.getInitialDarkModeSetting());

  recipeStore.addToRecent(RecipeStoreHelpers.getQueuedRecent());

  api()
    .appInfoList()
    .then((response) => {
      appStore.setApplicationInfo(response.data);
      RouterHelpers.setTitle(route);

      if (response.data.antiforgeryToken) {
        ApiHelpers.setHeader(
          response.data.antiforgeryTokenHeaderName || 'X-Csrf-Token',
          response.data.antiforgeryToken
        );
      }
    })
    .catch((response) => appStore.setApiFailureMessages(response));

  api()
    .appVersionList()
    .then((response) => appStore.setVersionInfo(response.data));
});
</script>

<template>
  <div id="skip-nav" class="container-xxl visually-hidden-focusable">
    <router-link class="d-inline-flex p-2 m-1" :to="{ hash: '#main', query: route.query }"
      >Skip to main content</router-link
    >
  </div>
  <AppHeader id="header" class="d-print-none">
    <template #navItems>
      <AppNav />
    </template>
  </AppHeader>
  <AppMessageCenter class="d-print-none" />
  <main id="main" class="mb-4" tabindex="-1">
    <RouterView />
  </main>
  <AppModal />
  <AppFooter id="footer" class="mt-2" />
</template>

<style lang="scss">
@import '@/styles/theme';
@import 'bootstrap/scss/bootstrap';
@import '@fortawesome/fontawesome-svg-core/styles.css';

// Sticky footer
html,
body {
  box-sizing: border-box;
  height: 100%;
  padding: 0;
  margin: 0;
}
#app {
  box-sizing: border-box;
  min-height: 100%;
  display: flex;
  flex-direction: column;
}
#skip-nav,
#header,
#message-center,
#footer {
  flex-grow: 0;
  flex-shrink: 0;
}
#main {
  flex-grow: 1;
}

// Minimum button width
Button animation input[type='button'].btn,
input[type='submit'].btn,
button.btn,
a.btn,
.btn {
  min-width: 5rem;
}

footer {
  border-top: var(--bs-gray-500) 1px solid;
}

.card {
  &.card-hover:hover:not(.active),
  .card-hover:hover:not(.active) {
    background-color: var(--bs-gray-200);
  }

  .card-link {
    text-decoration: none;
    color: unset;

    & > .img-fluid {
      max-height: 16rem;
    }
  }
}

body.bg-dark {
  --bs-link-color: #{lighten($primary, 20%)};
  --bs-link-hover-color: #{lighten($primary, 10%)};

  .card,
  .list-group,
  .list-group-item:not(.active) {
    background-color: inherit;
    color: inherit;
    outline: var(--bs-gray-800) 1px solid;

    &.card-hover:hover:not(.active),
    .card-hover:hover:not(.active) {
      background-color: var(--bs-gray-800);

      .card-link:hover {
        background-color: inherit;
        color: inherit;
      }
    }
  }

  .form-control,
  .form-select {
    background-color: var(--bs-dark);
    color: var(--bs-white);
  }

  .modal-content {
    color: var(--bs-dark);
  }

  .pagination {
    --bs-pagination-color: unset;
    --bs-pagination-bg: unset;
    --bs-pagination-disabled-bg: unset;
    --bs-pagination-disabled-border-color: unset;
  }

  .accordion {
    --bs-accordion-color: unset;
    --bs-accordion-bg: unset;
    --bs-accordion-btn-color: var(--bs-white);
    --bs-accordion-active-color: var(--bs-white);
    --bs-accordion-active-bg: var(--bs-gray-800);
    --bs-accordion-btn-icon: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23ffffff'%3e%3cpath fill-rule='evenodd' d='M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z'/%3e%3c/svg%3e");
    --bs-accordion-btn-active-icon: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23ffffff'%3e%3cpath fill-rule='evenodd' d='M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z'/%3e%3c/svg%3e");
  }

  .form-check:not(.form-switch) .form-check-input {
    background-color: var(--bs-dark);
    border-color: var(--bs-light);

    &:checked {
      background-color: var(--bs-primary);
    }
  }

  footer {
    border-top: var(--bs-gray-800) 1px solid;
  }
}

// Printable screens
@media print {
  div {
    background-color: var(--bs-white);
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
    color: var(--bs-black);
  }
}
</style>