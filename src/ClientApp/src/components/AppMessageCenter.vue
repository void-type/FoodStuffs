<script lang="ts" setup>
import { storeToRefs } from 'pinia';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const { messages, messageIsError } = storeToRefs(appStore);
const { clearMessages } = appStore;
</script>

<template>
  <div
    v-show="messages.length > 0"
    id="message-center"
    :class="{
      alert: true,
      'alert-dismissible': true,
      shadow: true,
      'sticky-top': true,
      'mb-0': true,
      'alert-danger': messageIsError,
      'alert-success': !messageIsError,
    }"
  >
    <ul>
      <li v-for="(message, id) in messages" :key="id">
        {{ message }}
      </li>
    </ul>
    <button type="button" class="btn-close" aria-label="Close" @click="clearMessages()"></button>
  </div>
</template>

<style lang="scss" scoped>
#message-center {
  z-index: 999;

  ul {
    text-align: center;
    list-style: none;
    margin: 0;
  }
}
</style>
