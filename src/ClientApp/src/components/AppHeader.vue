<script lang="ts" setup>
import logoSvg from '@/img/logo.svg';
import { storeToRefs } from 'pinia';
import { computed, ref } from 'vue';
import useAppStore from '@/stores/appStore';
import type { HTMLInputEvent } from '@/models/HTMLInputEvent';
import useMessageStore from '@/stores/messageStore';
import type { HttpResponse } from '@/api/http-client';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelper from '@/models/ApiHelper';
import AppHeaderSearch from './AppHeaderSearch.vue';

const appStore = useAppStore();
const messageStore = useMessageStore();
const { applicationName, user, useDarkMode } = storeToRefs(appStore);
const userRoles = computed(() => (user.value?.authorizedAs || []).join(', '));

const api = ApiHelper.client;

const isRebuilding = ref(false);

async function rebuildSearch() {
  try {
    isRebuilding.value = true;

    const response = await api().recipesRebuild();

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
        />
        {{ applicationName }}
      </router-link>
      <AppHeaderSearch v-model="searchText" class="ms-auto me-2 d-none d-sm-block d-md-none" />
      <button
        type="button"
        class="navbar-toggler border"
        data-bs-toggle="collapse"
        data-bs-target="#navbar-menu"
        aria-controls="navbar-menu"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <AppHeaderSearch v-model="searchText" class="mt-2 d-sm-none w-100" />
      <div id="navbar-menu" class="navbar-collapse collapse">
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
              data-bs-auto-close="outside"
              ><span>{{ user.login }}</span></a
            >
            <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdown">
              <li class="dropdown-item">Roles: {{ userRoles || 'none' }}</li>
              <li class="dropdown-item">
                <button class="btn btn-secondary" :disabled="isRebuilding" @click="rebuildSearch">
                  <font-awesome-icon icon="fa-rotate-right" />
                  {{ isRebuilding ? 'Rebuilding...' : 'Rebuild index' }}
                </button>
              </li>
              <li class="dropdown-item">
                <div class="form-check form-switch">
                  <label class="w-100" for="useDarkMode" aria-label="Use dark mode"
                    ><font-awesome-icon icon="fa-moon" /> Dark mode</label
                  >
                  <input
                    id="useDarkMode"
                    :checked="useDarkMode"
                    class="form-check-input"
                    type="checkbox"
                    @change="
                      (e: Event) =>
                        appStore.setDarkMode((e as HTMLInputEvent).target?.checked === true)
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

<style lang="scss" scoped></style>
