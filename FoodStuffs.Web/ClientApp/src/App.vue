<template>
    <div id="app">
        <div id="no-print">
          <Topbar />
          <MessageCenter></MessageCenter>
          <main>
              <router-view />
          </main>
          <footer>
              <div>
                  <a href="https://github.com/void-type/foodstuffs">
                      FoodStuffs is open source!
                  </a>
              </div>
          </footer>
        </div>
        <div id="print-only">
            <HomeViewer :currentRecipe="currentRecipe" />
        </div>
    </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import router from './router';
import store from './store';
import HomeViewer from './components/HomeViewer.vue';
import MessageCenter from './components/MessageCenter.vue';
import Topbar from './components/Topbar.vue';

export default {
  components: {
    HomeViewer,
    MessageCenter,
    Topbar,
  },
  router,
  store,
  computed: {
    ...mapGetters(['currentRecipe']),
  },
  methods: {
    ...mapActions(['fetchRecipes']),
  },
  beforeMount() {
    this.fetchRecipes();
  },
};

</script>

<style lang="scss">
@import "./style/variables";

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

main > section {
  display: flex;
  padding: 2em 0;

  & > *:not(:last-child) {
    margin-right: 2em;
  }
}

footer {
  border-top: $border;
  padding: 1em 0em;

  & div {
    text-align: center;
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

@media screen {
  body {
    background-color: $color-background;
  }

  #print-only,
  .print-only {
    display: none;
  }
}

@media print {
  #no-print,
  .no-print {
    display: none;
  }

  body {
    font-size: 90% !important;
  }

  div {
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
</style>
