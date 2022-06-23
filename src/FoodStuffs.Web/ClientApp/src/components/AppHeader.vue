<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import { computed } from 'vue';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const { applicationName, user } = storeToRefs(appStore);
const userRoles = computed(() => (user.value?.authorizedAs || []).join(', '));
</script>

<template>
  <header class="navbar navbar-dark bg-primary navbar-expand-md shadow">
    <nav class="container-xxl">
      <router-link :to="{ name: 'home' }" class="navbar-brand text-secondary">
        <img src="/img/logo.svg" alt="logo" class="d-inline-block align-text-top me-3" />
        {{ applicationName }}
      </router-link>
      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbar-menu"
        aria-controls="navbar-menu"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div id="navbar-menu" class="navbar-collapse collapse">
        <slot name="navItems"></slot>
        <ul class="navbar-nav ms-auto">
          <li class="nav-item dropdown">
            <a
              role="button"
              aria-haspopup="true"
              aria-expanded="false"
              href="#"
              class="nav-link dropdown-toggle"
              data-bs-toggle="dropdown"
              ><span>{{ user.login }}</span></a
            >
            <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdown">
              <li class="dropdown-item">Roles: {{ userRoles }}</li>
            </ul>
          </li>
        </ul>
      </div>
    </nav>
  </header>
</template>

<style lang="scss" scoped>
@import '@/styles/theme';

.navbar-brand > img {
  max-height: 30px;
  max-width: auto;
}
</style>
