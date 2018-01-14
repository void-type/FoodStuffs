<template>
  <ul v-if="messages.length > 0" :class="{error : $store.state.isError}">
    <li v-for="message in $store.state.messages" v-bind:key="message">{{message}}</li>
  </ul>
</template>

<script>
  export default {
    computed:
    {
      messages() {
        return this.$store.state.messages;
      },
      isError() {
        return this.$store.state.isError;
      }
    }
  }
</script>

<style lang="scss" scoped>
  @import 'MessageCenter';
</style>
