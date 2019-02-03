<template>
  <div id="app">
    <vue-progress-bar />
    <AppHeader
      :brand="applicationName"
      :user="user" />
    <AppMessageCenter
      class="no-print" />
    <main class="container">
      <router-view />
    </main>
    <AppFooter />
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import router from './router';
import store from './store';
import progressBar from './util/progressBar';
import AppMessageCenter from './viewComponents/AppMessageCenter.vue';
import AppHeader from './viewComponents/AppHeader.vue';
import AppFooter from './viewComponents/AppFooter.vue';

export default {
  components: {
    AppMessageCenter,
    AppHeader,
    AppFooter,
  },
  router,
  store,
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
    this.fetchApplicationInfo();
  },
  methods: {
    ...mapActions('app', [
      'fetchApplicationInfo',
    ]),
  },
};
</script>

<style lang="scss">
@import "./style/theme";

html {
  height: 100%;
}

body,
pre {
  font-family: $font-family;
  color: $color-neutral;
  background-color: $color-background;
  margin: 0;
}

header,
main {
  padding-left: 2em;
  padding-right: 2em;
}

.container {
  margin: 0 auto;
  max-width: $contained-width;
}

main {
  display: block;

  & > section {
    display: flex;
    padding: 2em 0;

    & > *:not(:last-child) {
      margin-right: 2em;
    }
  }
}

h1 {
  font-size: 2em;
}

h1,
h2,
h3,
h4 {
  color: $color-primary-dark;
}

a,
a:link {
  text-decoration: none;
  color: $color-primary-dark;
  cursor: pointer;

  &:hover,
  &:active {
    color: $color-secondary-dark;
  }
}

.slide-fade-enter-active {
  transition: all 0.3s ease;
}
.slide-fade-leave-active {
  transition: all 0.8s cubic-bezier(1, 0.5, 0.8, 1);
}

@media #{$extra-large-screen} {
  body {
    font-size: 130%;
  }
}

@media #{$medium-screen} {
  main > section {
    flex-direction: column;

    & > *:not(:last-child) {
      margin-right: 0;
    }
  }

  header,
  main,
  footer {
    padding-left: 1em;
    padding-right: 1em;
  }
}

@media #{$small-screen} {
  body {
    font-size: 90%;
  }
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

  body {
    font-size: 90% !important;
    background-color: $color-neutral-inverse;
  }

  p,
  pre,
  h1,
  h2,
  h3,
  h4 {
    color: $color-neutral;
  }
}
</style>
