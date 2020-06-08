<template>
  <div
    v-message-center-scroll="onScroll"
  >
    <b-alert
      :show="messages.length > 0"
      :variant="messageIsError ? 'danger' : 'success'"
      class="shadow mb-0"
      dismissible
      @dismissed="clearMessages()"
    >
      <ul>
        <li
          v-for="message in messages"
          :key="message"
        >
          {{ message }}
        </li>
      </ul>
    </b-alert>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import Vue from 'vue';

Vue.directive('message-center-scroll', {
  inserted(el, binding) {
    function f(evt) {
      if (binding.value(evt, el)) {
        window.removeEventListener('scroll', f);
      }
    }

    window.addEventListener('scroll', f);
  },
});

export default {
  computed: {
    ...mapGetters('app', [
      'messages',
      'messageIsError',
    ]),
  },
  methods: {
    ...mapActions('app', [
      'clearMessages',
    ]),
    onScroll(event, element) {
      const isFixed = window.scrollY > 56;
      element.classList.toggle('fixed-alert', isFixed);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "../style/theme";

div.alert {
  color: $body-color;

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
