<template>
  <header class="shadow">
    <b-navbar
      toggleable="md"
      type="dark"
      variant="primary"
    >
      <b-navbar-toggle
        target="nav-collapse"
      />

      <b-navbar-brand
        class="d-flex align-items-center"
        :to="'/'"
      >
        <div
          class="logo"
        >
          <img
            src="img/logo.svg"
            alt="logo"
          >
        </div>
        {{ brand }}
      </b-navbar-brand>

      <b-collapse
        id="nav-collapse"
        is-nav
      >
        <slot name="navItems" />

        <b-navbar-nav
          class="ml-auto"
        >
          <b-nav-item-dropdown
            :text="user.login"
            right
          >
            <b-dropdown-item
              disabled
            >
              Roles: {{ userRoles }}
            </b-dropdown-item>
          </b-nav-item-dropdown>
        </b-navbar-nav>
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
@import "@/style/theme";

.navbar-dark .navbar-brand {
  color: $secondary;
  font-weight: 600;
}

.logo img {
  max-height: 40px;
  max-width: auto;
  margin-right: 1rem;
}
</style>
