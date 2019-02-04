<template>
  <header class="no-print">
    <div class="container">
      <router-link
        :to="'/'"
        class="logo">
        <img
          src="../assets/logo.png"
          alt="logo" >
        <span>{{ brand }}</span>
      </router-link>
      <slot name="nav" />
      <router-link
        :to="'/'"
        :class="{'current-page': $route.name === 'login',
                 'pull-right': true}"
      >Sign in</router-link>
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
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;

      &.logo {
        height: 100%;

        & > img,
        span {
          margin-right: 1rem;
        }

        img {
          width: auto;
          height: 70%;
        }

        span {
          font-size: 150%;
          font-weight: 600;
          color: $color-secondary;

          &:hover,
          &:active {
            color: $color-secondary;
          }
        }
      }

      &:not(.logo) {
        &:link,
        &:visited {
          padding: 0em 1rem;
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

      &.pull-right {
        margin-left: auto;
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
