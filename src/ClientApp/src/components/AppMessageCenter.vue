<script lang="ts" setup>
import { useMessageStore } from '@/stores/messageStore';
import { storeToRefs } from 'pinia';

const messageStore = useMessageStore();
const { messages } = storeToRefs(messageStore);
const { clearMessage } = messageStore;
</script>

<template>
  <div
    v-show="messages.length > 0"
    id="message-center"
    :class="{
      shadow: true,
      'sticky-top': true,
      'mb-0': true,
    }"
  >
    <div
      v-for="(message, id) in messages"
      :key="id"
      :class="{
        alert: true,
        'alert-dismissible': true,
        'alert-danger': message.isError,
        'alert-success': !message.isError,
      }"
    >
      {{ message.text }}
      <button
        type="button"
        class="btn-close"
        aria-label="Close"
        @click="clearMessage(message)"
      ></button>
    </div>
  </div>
</template>

<style lang="scss" scoped>
#message-center {
  z-index: 999;

  div.alert {
    text-align: center;
    margin: 0;
    --bs-alert-padding-x: 0.5rem;
    --bs-alert-padding-y: 0.5rem;
  }

  .alert-dismissible .btn-close {
    padding: 0.8rem 1rem;
  }
}
</style>
