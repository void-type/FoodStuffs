import Vue from "vue"
import Router from "vue-router"
import Home from "../vue/views/Home"
import Edit from "../vue/views/Edit"

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: "/",
            name: "home",
            component: Home
        },
        {
            path: "/edit",
            name: "edit",
            component: Edit
        }
    ]
});