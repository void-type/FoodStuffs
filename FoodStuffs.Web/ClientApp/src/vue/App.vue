<template>
    <div>
        <div id="no-print">
            <header>
                <div class="topbar">
                    <router-link class="logo"
                                 :to="{name: 'home'}">
                        <img src="../assets/logo.png"
                             alt="FoodStuffs logo" />
                        <span>{{applicationName}}</span>
                    </router-link>
                    <nav>
                        <ul>
                            <li>
                                <router-link :to="{name: 'home'}"
                                             :class="{'current-page': $route.name === 'home'}">
                                    Home
                                </router-link>
                            </li>
                            <li>
                                <router-link :to="{name: 'edit'}"
                                             :class="{'current-page': $route.name === 'edit'}">
                                    Edit
                                </router-link>
                            </li>
                            <li>
                                <router-link :to="{name: 'search'}"
                                             :class="{'current-page': $route.name === 'search'}">
                                    Search
                                </router-link>
                            </li>
                        </ul>
                    </nav>
                    <router-link :to="{name: 'home'}"
                                 :class="{'pull-right': true, 'current-page': $route.name === 'login'}">
                        Login
                    </router-link>
                </div>
            </header>
            <MessageCenter></MessageCenter>
            <main>
                <router-view />
            </main>
            <footer>
                <div class="text-center">
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
import { mapActions, mapGetters } from "vuex";
import router from "../router";
import store from "../store";
import MessageCenter from "./components/MessageCenter";
import HomeViewer from "./components/HomeViewer";

export default {
  components: {
    MessageCenter,
    HomeViewer
  },
  router,
  store,
  computed: {
    ...mapGetters(["applicationName", "currentRecipe"])
  },
  methods: {
    ...mapActions(["fetchRecipes"])
  },
  beforeMount() {
    this.fetchRecipes();
  }
};
</script>

<style lang="scss">
@import "./variables";
@import "./common";

$topbar-height: 4em;

html {
  height: 100%;
}

body,
pre {
  font-family: $font-family;
  color: $color-neutral;
  margin: 0;
}

main,
footer {
  background-color: $color-background;
}

header,
main {
  padding-left: 2em;
  padding-right: 2em;
}

header > .topbar,
main,
footer > div {
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

header {
  position: relative;
  z-index: 2000;
  background-color: $color-primary-dark;
  box-shadow: $shadow;

  .topbar {
    display: flex;
    height: $topbar-height;

    & > div {
      display: flex;
      justify-content: space-between;
      height: 100%;
    }

    a,
    a-link {
      display: flex;
      align-items: center;

      &.logo {
        height: 100%;

        & > * {
          margin-right: 1em;
        }

        img {
          width: auto;
          height: 70%;
        }
      }

      span {
        font-size: 150%;
        font-weight: bold;
        color: $color-secondary-dark;

        &:hover,
        &:active {
          color: $color-secondary;
        }
      }

      &.pull-right {
        margin-left: auto;
      }
    }

    a,
    a:link {
      &:not(.logo) {
        display: flex;
        align-items: center;
        justify-content: center;
        height: $topbar-height;
        padding: 0em 1em;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;

        &:link,
        &:visited {
          color: $color-neutral-inverse;
        }

        &.current-page {
          color: $color-secondary-dark;
          font-weight: bold;
        }

        &:hover,
        &:active {
          background-color: mix($color-primary-dark, $color-secondary, 90%);
          color: $color-secondary;
          box-shadow: $highlight-border;
        }
      }
    }
  }
}

nav {
  background-color: $color-primary-dark;
  height: $topbar-height;

  ul {
    display: flex;
    justify-content: space-between;
    list-style: none;
    padding: 0;
    margin: 0;
    background-color: $color-primary-dark;

    li {
      display: block;
      position: relative;
      text-align: center;

      &:hover > ul {
        display: block;
        position: absolute;
      }
    }

    ul {
      display: none;
      width: 100%;
      box-shadow: $shadow;

      ul {
        left: 100%;
        top: 0;
        box-shadow: $shadow;
      }
    }
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
  main {
    padding-left: 1em;
    padding-right: 1em;
  }

  .topbar .logo span {
    display: none;
  }
}

@media #{$small-screen} {
  body {
    font-size: 90%;
  }
}
</style>