<template>
  <div>
    <header>
      <div class="topbar">
        <div>
          <a class="logo" href="#">
            <img src="../../assets/logo.png" alt="FoodStuffs" />
          </a>
          <a class="title" href="#">
            FoodStuffs
          </a>
        </div>
        <a href="#">Login</a>
      </div>
    </header>
    <MessageCenter></MessageCenter>
    <main>
      <router-view></router-view>
    </main>
    <footer>
      <div class="text-center">&copy; FoodStuffs</div>
    </footer>
  </div>
</template>

<script>
  import MessageCenter from "../components/MessageCenter";
  import store from "../store";

  export default {
    store,
    components: {
      MessageCenter
    }
  };
</script>

<style lang="scss">
  @import './App.scss';
</style>
