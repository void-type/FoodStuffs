<script lang="ts" setup>
import logoSvg from '@/img/logo.svg';
import { storeToRefs } from 'pinia';
import { computed, ref } from 'vue';
import useAppStore from '@/stores/appStore';
import type { HTMLInputEvent } from '@/models/HTMLInputEvent';
import useMessageStore from '@/stores/messageStore';
import ApiHelpers from '@/models/ApiHelpers';
import AppHeaderSearch from './AppHeaderSearch.vue';

const appStore = useAppStore();
const messageStore = useMessageStore();
const { applicationName, user, useDarkMode } = storeToRefs(appStore);
const userRoles = computed(() => (user.value?.authorizedAs || []).join(', '));

const api = ApiHelpers.client;

const isRebuilding = ref(false);

async function rebuildSearch() {
  isRebuilding.value = true;

  const response = await api().recipesRebuildIndexCreate();

  isRebuilding.value = false;

  if (response.data.message) {
    messageStore.setSuccessMessage(response.data.message);
  }
}

const searchText = ref('');
</script>

<template>
  <header id="header" class="navbar navbar-expand-md navbar-dark bg-primary shadow">
    <nav class="container-xxl">
      <router-link :to="{ name: 'home' }" class="navbar-brand text-light">
        <img
          :src="logoSvg"
          alt="logo"
          class="d-inline-block align-text-top"
          width="24"
          height="24"
        />
        {{ applicationName }}
      </router-link>
      <AppHeaderSearch v-model="searchText" class="ms-auto me-2 d-none d-sm-block d-md-none" />
      <button
        type="button"
        class="navbar-toggler"
        data-bs-toggle="collapse"
        data-bs-target="#navbar-menu"
        aria-controls="navbar-menu"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div id="navbar-menu" class="navbar-collapse collapse">
        <AppHeaderSearch v-model="searchText" class="mt-3 d-sm-none" />
        <slot name="navItems"></slot>
        <AppHeaderSearch v-model="searchText" class="ms-auto me-2 d-none d-md-block" />
        <ul class="navbar-nav">
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
              <li class="dropdown-item">
                <button
                  class="btn btn-outline-light"
                  :disabled="isRebuilding"
                  @click.stop.prevent="rebuildSearch"
                >
                  {{ isRebuilding ? 'Rebuilding...' : 'Rebuild index' }}
                </button>
              </li>
              <li class="dropdown-item">
                <div class="form-check form-switch" title="Use dark mode">
                  <label
                    class="form-check-label"
                    for="useDarkMode"
                    title="Use dark mode"
                    aria-label="Use dark mode"
                    @click.stop
                    >🌙</label
                  >
                  <input
                    id="useDarkMode"
                    :checked="useDarkMode"
                    class="form-check-input"
                    type="checkbox"
                    @change="
                      (e) => appStore.setDarkMode((e as HTMLInputEvent).target?.checked === true)
                    "
                  />
                </div>
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </nav>
  </header>
</template>

<style lang="scss" scoped>
.form-check-label {
  width: 100%;
}
</style>
