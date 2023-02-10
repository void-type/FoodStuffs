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

const appStore = useAppStore();
const recipeStore = useRecipeStore();
const route = useRoute();
const api = ApiHelpers.client;

onMounted(() => {
  recipeStore.loadQueuedRecipe();
  appStore.setDarkMode(DarkModeHelpers.getInitialDarkModeSetting());

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
  <div id="app-inner" tabindex="-1">
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
    <AppModal />
    <AppFooter class="mt-2" />
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
  overflow-y: scroll;
}
main {
  flex: 1 0 auto;
}
footer {
  flex-shrink: 0;
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

body.bg-dark {
  footer {
    border-top: var(--bs-gray-800) 1px solid;
  }
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

  .form-check-input {
    background-color: var(--bs-dark);
    border-color: var(--bs-light);

    &:checked {
      background-color: var(--bs-primary);
    }
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
