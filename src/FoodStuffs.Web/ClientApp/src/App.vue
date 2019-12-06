<template>
  <div id="app">
    <vue-progress-bar />
    <AppHeader
      :brand="applicationName"
      :user="user"
    >
      <AppNav slot="navItems" />
    </AppHeader>
    <AppMessageCenter
      class="no-print"
    />
    <main class="mt-4 mb-4">
      <b-container>
        <router-view />
      </b-container>
    </main>
    <AppFooter />
  </div>
</template>

<script>
import BootstrapVue from 'bootstrap-vue';
import Vue from 'vue';
import { mapGetters } from 'vuex';
import progressBar from './util/progressBar';
import initializeStore from './models/initializeStore';
import AppMessageCenter from './viewComponents/AppMessageCenter.vue';
import AppHeader from './viewComponents/AppHeader.vue';
import AppNav from './viewComponents/AppNav.vue';
import AppFooter from './viewComponents/AppFooter.vue';

Vue.use(BootstrapVue);

export default {
  components: {
    AppMessageCenter,
    AppHeader,
    AppNav,
    AppFooter,
  },
  computed: {
    ...mapGetters({
      applicationName: 'app/applicationName',
      user: 'app/user',
    }),
  },
  watch: {
    applicationName(newApplicationName) {
      document.title = newApplicationName;
    },
  },
  mounted() {
    progressBar.setupProgressBarHooks(this.$Progress);

    initializeStore();
  },
};
</script>

<style lang="scss">
@import "./style/theme";

$primary: $color-primary;
$secondary: $color-secondary;
$body-bg: $color-background;

$font-family-sans-serif: "-apple-system", "BlinkMacSystemFont", "Segoe UI",
  "Roboto", "Helvetica Neue", Arial, "Noto Sans", sans-serif;
$enable-rounded: false;
$print-page-size: auto;

input[type="button"].btn,
input[type="submit"].btn,
button.btn,
a.btn {
  box-shadow: $shadow;
  position: relative;
  min-width: 5rem;

  &:active:not(:disabled) {
    box-shadow: $shadow-collapse;
    top: 3px;
  }
}

@import "~bootstrap/scss/bootstrap";
@import "~bootstrap-vue/src/index";

h1,
h2,
h3,
h4,
h5,
h6 {
  color: $color-primary;
}

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
    background-color: white;
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
    color: black;
  }
}
</style>
