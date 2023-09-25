<script lang="ts" setup>
import { onMounted } from 'vue';
import { RouterView, useRoute } from 'vue-router';
import useAppStore from '@/stores/appStore';
import AppHeader from '@/components/AppHeader.vue';
import AppNav from '@/components/AppNav.vue';
import AppFooter from '@/components/AppFooter.vue';
import AppMessageCenter from '@/components/AppMessageCenter.vue';
import RouterHelpers from '@/models/RouterHelpers';
import AppModal from '@/components/AppModal.vue';
import ApiHelpers from '@/models/ApiHelpers';
import useRecipeStore from '@/stores/recipeStore';
import DarkModeHelpers from '@/models/DarkModeHelpers';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';
import useMessageStore from '@/stores/messageStore';

const appStore = useAppStore();
const messageStore = useMessageStore();
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
    .catch((response) => messageStore.setApiFailureMessages(response));

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
  <AppHeader class="d-print-none">
    <template #navItems>
      <AppNav />
    </template>
  </AppHeader>
  <AppMessageCenter class="d-print-none" />
  <main id="main" class="mb-4" tabindex="-1">
    <RouterView />
  </main>
  <AppModal />
  <AppFooter class="mt-2" />
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
  --bs-accordion-btn-icon-width: #{$accordion-icon-width};
  --bs-accordion-btn-icon: #{escape-svg($accordion-button-icon)};
  --bs-accordion-btn-icon-transition: #{$accordion-icon-transition};
  --bs-accordion-btn-active-icon: #{escape-svg($accordion-button-active-icon)};
  --bs-accordion-btn-icon-transform: #{$accordion-icon-transform};
  --bs-accordion-btn-padding-y: #{$accordion-button-padding-y};
  --bs-accordion-btn-padding-x: #{$accordion-button-padding-x};
  --bs-accordion-btn-color: #{$accordion-button-color};
  --bs-accordion-btn-bg: #{$accordion-button-bg};

  &.card-hover:hover:not(.active),
  .card-hover:hover:not(.active) {
    background-color: var(--bs-gray-200);
  }

  .card-link {
    text-decoration: none;
    color: unset;
  }

  .card-floating-toolbar {
    position: absolute;
    top: 0;
    right: 0;
  }

  .btn-card-control {
    padding: 0.5rem var(--bs-card-cap-padding-x);
    display: inline-block;
  }
}

body.bg-dark {
  --bs-link-color: #{lighten($primary, 20%)};
  --bs-link-hover-color: #{lighten($primary, 10%)};
  --bs-border-color: var(--bs-gray-800);

  .text-muted {
    --bs-text-opacity: 1;
    color: var(--bs-gray-600) !important;
  }

  .page-link.active,
  .active > .page-link {
    border-color: unset;
  }

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
  .form-select,
  .dropdown-menu {
    background-color: var(--bs-dark);
    color: var(--bs-white);
  }

  .dropdown-item {
    color: var(--bs-white);

    &:hover {
      background-color: var(--bs-gray-800);
    }
  }

  .modal-content {
    background-color: var(--bs-dark);
    color: var(--bs-light);

    .modal-header .btn-close {
      --bs-btn-close-bg: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23e6e6e6'%3e%3cpath d='M.293.293a1 1 0 0 1 1.414 0L8 6.586 14.293.293a1 1 0 1 1 1.414 1.414L9.414 8l6.293 6.293a1 1 0 0 1-1.414 1.414L8 9.414l-6.293 6.293a1 1 0 0 1-1.414-1.414L6.586 8 .293 1.707a1 1 0 0 1 0-1.414z'/%3e%3c/svg%3e");
    }
  }

  .pagination {
    --bs-pagination-color: unset;
    --bs-pagination-bg: unset;
    --bs-pagination-disabled-bg: unset;
    --bs-pagination-disabled-border-color: unset;
  }

  .accordion,
  .card {
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
