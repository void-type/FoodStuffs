import Vue from "vue"
import App from "./vue/App"

Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: "#app",
  render: createElement => createElement(App)
});