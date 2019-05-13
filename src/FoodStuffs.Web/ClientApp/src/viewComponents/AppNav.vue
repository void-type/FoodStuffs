<template>
  <nav>
    <ul>
      <li
        v-for="link in links"
        :key="link.label"
      >
        <router-link
          :to="{name: link.route}"
          :class="{'current-page': $route.name === link.route}"
        >
          {{ link.label }}
        </router-link>
      </li>
    </ul>
  </nav>
</template>

<script>
export default {
  props: {
    links: {
      type: Array,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";

nav {
  background-color: $color-primary-dark;
  height: $topbar-height;

  a,
  a-link {
    display: flex;
    align-items: center;
    justify-content: center;
    height: $topbar-height;
    padding: 0em 1rem;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;

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
</style>
