<script setup lang="ts">
import { RouterView } from 'vue-router';
</script>

<template>
  <nav class="navbar navbar-expand-md navbar-dark bg-dark">
    <a class="navbar-brand" href="/">
      <img
        src="https://avatars1.githubusercontent.com/u/3291875?s=88&v=4"
        width="30"
        height="30"
        class="d-inline-block align-top mr-1"
        alt=""
      />void-type</a
    >
  </nav>

  <main>
    <RouterView />
  </main>
</template>

<style lang="scss">
body {
  font-family: Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>
