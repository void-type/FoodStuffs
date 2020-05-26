<template>
  <b-alert
    v-if="messages.length > 0"
    show
    :variant="messageIsError ? 'danger' : 'success'"
    class="shadow"
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
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

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
</style>
