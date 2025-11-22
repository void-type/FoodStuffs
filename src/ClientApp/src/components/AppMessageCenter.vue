<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { storeToRefs } from 'pinia';
import useMessageStore from '@/stores/messageStore';

const messageStore = useMessageStore();
const { messages } = storeToRefs(messageStore);
const { clearMessage, clearMessageTimeout } = messageStore;
</script>

<template>
  <div id="message-center" class="sticky-bottom d-print-none">
    <transition-group v-if="messages.length > 0" name="list" class="alert-wrapper" tag="div" appear>
      <div
        v-for="message in messages"
        :key="message.id"
        class="alert alert-dismissible" :class="{
          'alert-danger': message.isError,
          'alert-success': !message.isError,
        }"
      >
        <FontAwesomeIcon
          v-if="typeof message.timeout === 'undefined'"
          class="opacity-50 me-2"
          icon="fa-thumbtack"
        />
        {{ message.text }}
        <button
          type="button"
          class="btn-close"
          aria-label="Close message"
          @click="clearMessage(message)"
          @mouseenter="clearMessageTimeout(message)"
          @focusin="clearMessageTimeout(message)"
        />
      </div>
    </transition-group>
  </div>
</template>

<style lang="scss" scoped>
#message-center {
  z-index: 999;
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  width: 100%;
  display: flex;
  justify-content: center;
  pointer-events: none;

  div.alert-wrapper {
    width: 90%;
    max-width: 400px;
    margin: 0;
    pointer-events: auto;

    div.alert {
      margin: 0;
      margin-bottom: 0.2em;
      --bs-alert-padding-x: 1rem;
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
