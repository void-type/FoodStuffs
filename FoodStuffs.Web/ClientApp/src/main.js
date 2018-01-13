import Vue from "vue"
import App from "./views/App"
import router from "./router/router"
import store from "./store"

Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: "#app",
  router,
  store,
  render: h => h(App)
});
