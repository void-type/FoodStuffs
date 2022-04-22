<template>
  <ul class="navbar-nav">
    <li class="nav-item">
      <a :to="{ name: 'search' }" class="nav-link" target="_self">Search</a>
    </li>
    <li class="nav-item">
      <a :to="{ name: 'new' }" class="nav-link" target="_self">New</a>
    </li>
  </ul>
</template>

<script setup lang="ts"></script>

<style lang="scss" scoped></style>
