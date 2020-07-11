<template>
  <header class="shadow">
    <b-navbar
      toggleable="md"
      type="dark"
      variant="primary"
    >
      <b-navbar-toggle
        class="mr-3"
        target="nav_collapse"
      />
      <router-link
        :to="'/'"
        class="logo"
      >
        <img
          src="/img/logo.svg"
          alt="logo"
          class="d-inline-block align-top"
        >
        <b-navbar-brand>{{ brand }}</b-navbar-brand>
      </router-link>
      <b-collapse
        id="nav_collapse"
        is-nav
      >
        <slot name="navItems" />
        <b-nav-text
          :title="userRoles"
          class="ml-auto"
        >
          {{ user.login }}
        </b-nav-text>
      </b-collapse>
    </b-navbar>
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
  computed: {
    userRoles() {
      return this.user.authorizedAs.join(', ');
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";

.navbar-dark .navbar-brand {
  color: $secondary;
  font-weight: 600;
  font-size: 1.5em;
}

.logo img {
  max-height: 2.5em;
  margin-right: 0.5em;
}
</style>
