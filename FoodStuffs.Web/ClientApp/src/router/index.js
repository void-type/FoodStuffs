import Vue from "vue"
import Router from "vue-router"
import HomeView from "../vue/views/Home"
import EditView from "../vue/views/Edit"
import SearchView from "../vue/views/Search"

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView
    },
    {
      path: "/edit",
      name: "edit",
      component: EditView
    },
    {
      path: "/search",
      name: "search",
      component: SearchView
    }
  ]
});