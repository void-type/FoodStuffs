<script lang="ts" setup>
import { useMessageStore } from '@/stores/messageStore';
import { storeToRefs } from 'pinia';

const messageStore = useMessageStore();
const { messages } = storeToRefs(messageStore);
const { clearMessage } = messageStore;
</script>

<template>
  <div
    id="message-center"
    :class="{
      shadow: messages.length > 0,
      'sticky-top': true,
      'mb-0': true,
    }"
  >
    <transition-group name="list" class="alert-wrapper" tag="div" appear>
      <div
        v-for="message in messages"
        :key="message.id"
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
    </transition-group>
  </div>
</template>

<style lang="scss" scoped>
#message-center {
  z-index: 999;

  div.alert-wrapper {
    position: absolute;
    width: 600px;
    max-width: 90%;
    margin-left: auto;
    margin-right: auto;
    left: 0;
    right: 0;

    div.alert {
      margin: 0;
      margin-top: 0.5rem;
      text-align: center;
      --bs-alert-padding-x: 0.5rem;
      --bs-alert-padding-y: 0.5rem;
    }

    .list-move,
    .list-enter-active,
    .list-leave-active {
      transition: all 0.3s ease;
    }

    .list-leave-active {
      position: absolute;
      left: 0;
      right: 0;
      width: 100%;
    }

    .list-enter-from {
      opacity: 0;
      transform: translateX(60px);
    }

    .list-leave-to {
      opacity: 0;
      transform: translateX(-60px);
    }

    .alert-dismissible .btn-close {
      padding: 0.8rem 1rem;
    }
  }
}
</style>
