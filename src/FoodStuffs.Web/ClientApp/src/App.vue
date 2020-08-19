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
    <main>
      <router-view />
    </main>
    <AppFooter />
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import initializeStore from './models/initializeStore';
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
    initializeStore();
  },
};
</script>

<style lang="scss">
@import "./style/theme";

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

Button animation input[type="button"].btn,
input[type="submit"].btn,
button.btn,
a.btn,
.btn {
  // box-shadow: $btn-box-shadow;
  // position: relative;
  min-width: 5rem;

  // &:active:not(:disabled) {
  //   box-shadow: $btn-active-box-shadow;
  //   top: 3px;
  // }
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
