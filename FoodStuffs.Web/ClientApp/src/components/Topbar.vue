<template>
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
                      :class="{'current-page': $route.name === 'login',
                              'pull-right': true}">
            Login
        </router-link>
    </div>
</header>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  computed: {
    ...mapGetters(['applicationName']),
  },
  methods: {
    ...mapActions(['fetchApplicationName']),
  },
  mounted() {
    this.fetchApplicationName();
    document.title = this.applicationName;
  },
};
</script>

<style lang="scss" scoped>
@import "../style/variables";

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

@media #{$medium-screen} {
  header .logo span {
    display: none;
  }
}
</style>

