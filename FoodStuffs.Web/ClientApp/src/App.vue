<template>
  <div>
    <header>
      <div class="topbar">
        <div>
          <a class="logo" href="#">
            <img src="./assets/logo.png" alt="FoodStuffs" />
          </a>
          <a class="title" href="#">
            FoodStuffs
          </a>
        </div>
        <a href="#">Login</a>
      </div>
    </header>
    <message-center :class="{error : isError}"
                    :messages="messages"
                    :is-error="isError">
    </message-center>
    <main>
      <router-view></router-view>
    </main>
    <footer>
      <div class="text-center">&copy; FoodStuffs</div>
    </footer>
  </div>
</template>

<script>
  export default {
    name: "app"
  };
</script>

<style lang="scss">
  @import './App.scss';
</style>
