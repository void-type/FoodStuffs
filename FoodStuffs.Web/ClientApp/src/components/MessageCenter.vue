<template>
  <ul v-if="messages.length > 0">
    <li v-for="message in messages" v-bind:key="message">{{message}}</li>
  </ul>
</template>

<script>
  export default {
    name: "message-center"
  }
  //Vue.component("message-center", {
  //  template: require("./message-center.html"),
  //  props: ["messages", "is-error"]
  //});
</script>

<style lang="scss">
  @import 'MessageCenter';
</style>
