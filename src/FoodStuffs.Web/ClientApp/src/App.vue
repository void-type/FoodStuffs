<template>
  <div id="app">
    <vue-progress-bar />
    <AppHeader
      :brand="applicationName"
      :user="user" >
      <AppNav
        slot="nav"
        :links="navItems" />
    </AppHeader>
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
import AppNav from './viewComponents/AppNav.vue';
import AppFooter from './viewComponents/AppFooter.vue';

export default {
  components: {
    AppMessageCenter,
    AppHeader,
    AppNav,
    AppFooter,
  },
  data() {
    return {
      navItems: [
        {
          label: 'Search',
          route: 'search',
        },
        {
          label: 'New',
          route: 'new',
        },
      ],
    };
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
    this.fetchRecipesList();
  },
  methods: {
    ...mapActions({
      fetchApplicationInfo: 'app/fetchApplicationInfo',
      fetchRecipesList: 'recipes/fetchList',
    }),
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
  padding-left: 2rem;
  padding-right: 2rem;
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
      margin-right: 2rem;
    }
  }
}

h1 {
  font-size: 2rem;
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
      margin-bottom: 1.5rem;
    }
  }

  header,
  main,
  footer {
    padding-left: 1rem;
    padding-right: 1rem;
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
    background-color: $color-neutral-inverse;

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
      color: $color-neutral;
    }
  }
}
</style>
