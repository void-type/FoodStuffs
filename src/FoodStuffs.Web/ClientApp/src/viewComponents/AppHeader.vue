<template>
  <header class="no-print">
    <div class="container">
      <router-link
        :to="'/'"
        class="logo">
        <img
          src="../assets/logo.png"
          alt="FoodStuffs logo" >
        <span>{{ brand }}</span>
      </router-link>
      <nav>
        <ul>
          <li>
            <router-link
              :to="{name: 'search'}"
              :class="{'current-page': $route.name === 'search'}"
            >Search</router-link>
          </li>
          <li>
            <router-link
              :to="{name: 'new'}"
              :class="{'current-page': $route.name === 'new'}"
            >New</router-link>
          </li>
        </ul>
      </nav>
      <router-link
        :to="'/'"
        :class="{'current-page': $route.name === 'login',
                 'pull-right': true}"
      >Login</router-link>
    </div>
  </header>
</template>

<script>
export default {
  props: {
    brand: {
      type: String,
      required: true,
    },
    user: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";

header {
  background-color: $color-primary-dark;
  box-shadow: $shadow;
  height: $topbar-height;

  & > div {
    display: flex;
    height: 100%;

    a,
    a-link {
      display: flex;
      align-items: center;
      justify-content: center;
      height: $topbar-height;
      padding: 0em 1em;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;

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

      &:not(.logo) {
        &:link,
        &:visited {
          color: $color-neutral-inverse;
        }

        &.current-page {
          background-color: darken($color-primary-dark, 10%);
        }

        &:hover,
        &:active {
          background-color: mix($color-primary-dark, $color-secondary, 90%);
          color: $color-secondary;
          box-shadow: $highlight-border;
        }
      }

      span {
        font-size: 150%;
        font-weight: 600;
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
}

@media #{$medium-screen} {
  header .logo span {
    display: none;
  }
}
</style>
