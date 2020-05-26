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
      <router-view />
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

input[type="button"].btn,
input[type="submit"].btn,
button.btn,
a.btn,
.btn {
  box-shadow: $btn-box-shadow;
  position: relative;
  min-width: 5rem;

  &:active:not(:disabled) {
    box-shadow: $btn-active-box-shadow;
    top: 3px;
  }
}

h1,
h2,
h3,
h4,
h5,
h6 {
  color: $primary;
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
