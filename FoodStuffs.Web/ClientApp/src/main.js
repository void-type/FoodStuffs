import Vue from "vue"
import App from "./vue/App"

Vue.config.productionTip = false;

/* eslint-disable no-new */
const mainApp = new Vue({
    el: "#app",
    render: h => h(App)
});