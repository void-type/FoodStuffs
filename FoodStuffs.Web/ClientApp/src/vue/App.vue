<template>
  <div>
    <header>
      <div class="topbar">
        <div>
          <a class="logo" href="#">
            <img src="../assets/logo.png" alt="FoodStuffs logo" />
          </a>
          <a class="title" href="#">
            {{applicationName}}
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
      <div class="text-center"><a href="https://github.com/void-type/foodstuffs">FoodStuffs is open source!</a></div>
    </footer>
  </div>
</template>

<script>
  import MessageCenter from "./components/MessageCenter";
  import router from "../router"
  import store from "../store";

  export default {
    data: function () {
      return {
        applicationName: document.getElementById("applicationName") !== null ? document.getElementById("applicationName").value : "FoodStuffs"
      }
    },
    router,
    store,
    components: {
      MessageCenter
    }
  };
</script>

<style lang="scss">
  @import './App.scss';
</style>
