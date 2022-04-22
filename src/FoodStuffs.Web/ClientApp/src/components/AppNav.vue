<template>
  <ul class="navbar-nav me-auto mb-2 mb-lg-0">
    <li class="nav-item">
      <router-link :to="{ name: 'search' }" class="nav-link">Search</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'new' }" class="nav-link">New</router-link>
    </li>
  </ul>
</template>

<script setup lang="ts"></script>

<style lang="scss" scoped></style>
