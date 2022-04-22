<script setup lang="ts">
// import type { DirectiveBinding } from 'vue';
import { storeToRefs, mapActions } from 'pinia';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();

// function onScroll(event: Event, element: Element) {
//   const isFixed = window.scrollY > 58;
//   element.classList.toggle('fixed-alert', isFixed);
// }

// const vMessageCenterScroll = {
//   mounted(el: Element, binding: DirectiveBinding) {
//     function f() {
//       if (binding.value(onScroll, el)) {
//         window.removeEventListener('scroll', f);
//       }
//     }

//     window.addEventListener('scroll', f);
//   },
// };

const { messages, messageIsError } = storeToRefs(appStore);
const { clearMessages } = mapActions(useAppStore, { clearMessages: 'clearMessages' });
</script>

<template>
  <div v-message-center-scroll>
    <div
      v-show="messages.length > 0"
      :class="{
        alert: true,
        'alert-dismissible': true,
        shadow: true,
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
      <button
        type="button"
        class="btn-close"
        data-bs-dismiss="alert"
        aria-label="Close"
        @click="clearMessages()"
      ></button>
    </div>
  </div>
</template>

<style lang="scss" scoped>
div.alert {
  ul {
    text-align: center;
    list-style: none;
    margin: 0;
  }
}

div.fixed-alert {
  position: fixed;
  top: 0;
  z-index: 999;
  width: 100%;
}
</style>
