<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { computed } from 'vue';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();

const { applicationName, user } = storeToRefs(appStore);

const userRoles = computed(() => (user.value?.authorizedAs || []).join(', '));
</script>

<template>
<!-- TODO: START HERE, FINISH NAV -->
  <header class="shadow">
    <nav class="navbar navbar-dark bg-primary navbar-expand-md">
      <div class="container-fluid">
        <a :to="'/'" aria-current="page" class="navbar-brand">
          <img src="/img/logo.svg" alt="logo" class="d-inline-block align-text-top" />
          {{ applicationName }}
        </a>

        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div id="nav-collapse" class="navbar-collapse collapse" style="display: none">
          <slot name="navItems"></slot>
          <ul class="navbar-nav ml-auto">
            <li id="__BVID__20" class="nav-item b-nav-dropdown dropdown">
              <a
                id="__BVID__20__BV_toggle_"
                role="button"
                aria-haspopup="true"
                aria-expanded="false"
                href="#"
                target="_self"
                class="nav-link dropdown-toggle"
                ><span>{{ user.login }}</span></a
              >
              <ul
                tabindex="-1"
                class="dropdown-menu dropdown-menu-right"
                aria-labelledby="__BVID__20__BV_toggle_"
              >
                <li role="presentation">
                  <a
                    role="menuitem"
                    href="#"
                    target="_self"
                    tabindex="-1"
                    aria-disabled="true"
                    class="dropdown-item disabled"
                  >
                    Roles: {{ userRoles }}
                  </a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </header>

  <header class="shadow">
    <b-navbar toggleable="md" type="dark" variant="primary">
      <b-navbar-toggle target="nav-collapse" />

      <b-navbar-brand class="d-flex align-items-center" :to="'/'">
        <div class="logo">
          <img src="/img/logo.svg" alt="logo" />
        </div>
        {{ applicationName }}
      </b-navbar-brand>

      <b-collapse id="nav-collapse" is-nav>
        <slot name="navItems" />

        <b-navbar-nav class="ml-auto">
          <b-nav-item-dropdown :text="user.login" right>
            <b-dropdown-item disabled> Roles: {{ userRoles }} </b-dropdown-item>
          </b-nav-item-dropdown>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
  </header>
</template>

<style lang="scss" scoped>
.navbar-dark .navbar-brand {
  color: $secondary;
  font-weight: 600;
}

.navbar-brand > img {
  max-height: 30px;
  max-width: auto;
  margin-right: 1rem;
}
</style>
