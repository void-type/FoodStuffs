<template>
  <div
    v-if="messages.length > 0"
    :class="{'shadow': true,
             'alert-danger': messageIsError,
             'alert-success': !messageIsError}"
    @click="clearMessages()"
  >
    <ul>
      <li
        v-for="message in messages"
        :key="message"
      >
        {{ message }}
      </li>
    </ul>
  </div>
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
div {
  position: relative;
  margin: 0;
  padding: 0;
  width: 100%;
  cursor: pointer;

  ul {
    padding: 1em 1em 1em 2.5em;
    text-align: center;
    list-style: none;
    margin: 0;

    & > li {
      margin: 0 auto;
      color: #000000;
    }
  }
}
</style>
