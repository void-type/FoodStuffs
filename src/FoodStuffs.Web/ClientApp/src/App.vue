<script setup lang="ts">
import { mapActions, storeToRefs } from 'pinia';
import { onMounted, watch } from 'vue';
import { RouterView } from 'vue-router';
import { Api } from '@/api/Api';
import useAppStore from '@/stores/appStore';
import AppHeader from '@/components/AppHeader.vue';
import AppNav from '@/components/AppNav.vue';
import AppFooter from '@/components/AppFooter.vue';
import AppMessageCenter from '@/components/AppMessageCenter.vue';

const appStore = useAppStore();

const { applicationName } = storeToRefs(appStore);

const { clearMessages } = mapActions(useAppStore, {
  clearMessages: 'clearMessages',
});

onMounted(() => {
  new Api()
    .appInfoList()
    .then((response) => appStore.setApplicationInfo(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));

  new Api()
    .appVersionList()
    .then((response) => appStore.setVersionInfo(response.data))
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    .catch(() => {});
});

// TODO: fix how titles are set using router
watch(applicationName, (newApplicationName) => {
  document.title = newApplicationName;
});
</script>

<template>
  <!-- TODO: add a skip nav link -->
  <div id="app" @keydown.stop.prevent.esc="clearMessages()">
    <!-- TODO: bind this to the API -->
    <vue-progress-bar />
    <AppHeader>
      <template #navItems>
        <AppNav />
      </template>
    </AppHeader>
    <AppMessageCenter class="no-print" />
    <main>
      <RouterView />
    </main>
    <AppFooter />
  </div>
</template>

<style lang="scss">
// Sticky footer
html,
body,
#app {
  height: 100%;
}
#app {
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

// Hover table cursor
table.table-hover {
  cursor: pointer;
}

// Printable screens
@media screen {
  .print-only {
    display: none;
  }
}

@media print {
  .no-print {
    display: none;
  }

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
