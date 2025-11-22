<script lang="ts" setup>
import type { HttpResponse } from '@/api/http-client';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { storeToRefs } from 'pinia';
import { ref } from 'vue';
import logoSvg from '@/img/logo.svg';
import ApiHelper from '@/models/ApiHelper';
import useAppStore from '@/stores/appStore';
import useMessageStore from '@/stores/messageStore';
import AppHeaderQuickSearch from './AppHeaderQuickSearch.vue';

const appStore = useAppStore();
const messageStore = useMessageStore();
const { applicationName, user, useDarkMode } = storeToRefs(appStore);

const api = ApiHelper.client;

const isRebuilding = ref(false);

async function rebuildSearch() {
  try {
    isRebuilding.value = true;

    const response = await api().appRebuildIndexes();

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  } finally {
    isRebuilding.value = false;
  }
}

const searchText = ref('');
</script>

<template>
  <header id="header" class="navbar navbar-expand-md navbar-dark bg-primary d-print-none">
    <nav class="container-xxl">
      <router-link :to="{ name: 'home' }" class="navbar-brand">
        <img
          :src="logoSvg"
          alt="logo"
          class="d-inline-block align-text-top"
          width="24"
          height="24"
        >
        {{ applicationName }}
      </router-link>
      <AppHeaderQuickSearch v-model="searchText" class="ms-auto me-2 d-none d-sm-block d-md-none" />
      <button
        type="button"
        class="navbar-toggler border"
        data-bs-toggle="collapse"
        data-bs-target="#navbar-menu"
        aria-controls="navbar-menu"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon" />
      </button>
      <AppHeaderQuickSearch v-model="searchText" class="mt-2 d-sm-none w-100" />
      <div id="navbar-menu" class="navbar-collapse collapse">
        <slot name="navItems" />
        <AppHeaderQuickSearch v-model="searchText" class="ms-auto me-2 d-none d-md-block" />
        <ul class="navbar-nav">
          <li class="nav-item dropdown">
            <a
              role="button"
              aria-haspopup="true"
              aria-expanded="false"
              href="#"
              class="nav-link dropdown-toggle"
              data-bs-toggle="dropdown"
              data-bs-auto-close="outside"
            ><span>{{ user.login }}</span></a>
            <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdown">
              <!-- <li class="dropdown-item-text fw-bold">Roles</li>
              <li v-for="role in user.authorizedAs" :key="role" class="dropdown-item-text">
                {{ role }}
              </li>
              <li v-if="(user.authorizedAs?.length || 0) < 1" class="dropdown-item-text text-muted">
                No roles.
              </li>
              <li><hr class="dropdown-divider" /></li> -->
              <li class="dropdown-item">
                <div class="form-check form-switch">
                  <label class="w-100" for="useDarkMode" aria-label="Use dark mode"><FontAwesomeIcon icon="fa-moon" /> Dark mode</label>
                  <input
                    id="useDarkMode"
                    :checked="useDarkMode"
                    class="form-check-input"
                    type="checkbox"
                    @change="
                      (e: Event) =>
                        appStore.setDarkMode((e.target as HTMLInputElement).checked === true)
                    "
                  >
                </div>
              </li>
              <li class="dropdown-item">
                <button class="btn btn-secondary" :disabled="isRebuilding" @click="rebuildSearch">
                  <FontAwesomeIcon icon="fa-rotate-right" />
                  {{ isRebuilding ? 'Rebuilding...' : 'Rebuild index' }}
                </button>
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </nav>
  </header>
</template>

<style lang="scss" scoped></style>
