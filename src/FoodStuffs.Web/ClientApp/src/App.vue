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
@import '@/style/theme';

// Sticky footer
html,
body,
#app {
  height: 100%;
}
#app {
  display: flex;
  flex-direction: column;
}
main {
  flex: 1 0 auto;
}
footer {
  flex-shrink: 0;
}

// Modal background
.modal-content {
  background: $body-bg;
}

// Minimum button width
Button animation input[type='button'].btn,
input[type='submit'].btn,
button.btn,
a.btn,
.btn {
  min-width: 5rem;
}

// Colored headings
h1,
h2,
h3,
h4,
h5,
h6,
.h1,
.h2,
.h3,
.h4,
.h5,
.h6 {
  color: $primary;
}

// Hover table cursor
table.table-hover {
  cursor: pointer;
}

// Printable screens
@media screen {
  .print-only {
    display: none;
  }
}

@media print {
  .no-print {
    display: none;
  }

  div {
    background-color: $white;
  }

  button,
  .btn {
    display: none;
  }

  p,
  pre,
  h1,
  h2,
  h3,
  h4 {
    color: $black;
  }
}
</style>
