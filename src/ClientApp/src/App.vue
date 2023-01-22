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
      <RouterView @keydown.stop.prevent.esc="clearMessages()" />
    </main>
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
  border-top: $gray-500 1px solid;
}

body.bg-dark {
  footer {
    border-top: $gray-800 1px solid;
  }
}

.card {
  outline: $gray-500 1px solid;

  &.card-hover:hover:not(.active),
  .card-hover:hover:not(.active) {
    background-color: $gray-200;
  }

  .card-link {
    text-decoration: none;
    color: unset;

    & > img {
      max-height: 350px;
    }
  }
}

body.bg-dark {
  .card,
  .list-group,
  .list-group-item:not(.active) {
    background-color: inherit;
    color: inherit;
    outline: $gray-800 1px solid;

    &.card-hover:hover:not(.active),
    .card-hover:hover:not(.active) {
      background-color: $gray-800;

      .card-link:hover {
        background-color: inherit;
        color: inherit;
      }
    }
  }

  .form-control,
  .form-select {
    background-color: $dark;
    color: $light;
  }

  .form-check-input {
    background-color: $dark;
    border-color: $light;

    &:checked {
      background-color: $primary;
    }
  }
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
