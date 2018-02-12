<template>
    <div>
        <div id="no-print">
            <header>
                <div class="topbar">
                    <router-link class="logo"
                                 :to="{name: 'home'}">
                        <img src="../assets/logo.png"
                             alt="FoodStuffs logo" />
                        <span>{{applicationName}}</span>
                    </router-link>
                    <nav>
                        <ul>
                            <li>
                                <router-link :to="{name: 'home'}"
                                             :class="{'current-page': $route.name === 'home'}">
                                    Home
                                </router-link>
                            </li>
                            <li>
                            <router-link :to="{name: 'search'}"
                                         :class="{'current-page': $route.name === 'search'}">
                                Search
                            </router-link>
                            </li>
                        </ul>
                    </nav>
                    <router-link :to="{name: 'home'}"
                                 :class="{'pull-right': true, 'current-page': $route.name === 'login'}">
                        Login
                    </router-link>
                </div>
            </header>
            <MessageCenter></MessageCenter>
            <main>
                <router-view />
            </main>
            <footer>
                <div class="text-center">
                    <a href="https://github.com/void-type/foodstuffs">
                        FoodStuffs is open source!
                    </a>
                </div>
            </footer>
        </div>
        <div id="print-only">
            <HomeViewer></HomeViewer>
        </div>
    </div>
</template>

<script>
    import { mapActions } from "vuex";
    import router from "../router";
    import store from "../store";
    import MessageCenter from "./components/MessageCenter";
    import HomeViewer from "./components/HomeViewer";

    export default {
        data: function () {
            return {
                applicationName:
                document.getElementById("applicationName") !== null
                    ? document.getElementById("applicationName").value
                    : "FoodStuffs"
            };
        },
        methods: {
            ...mapActions(["fetchRecipes"])
        },
        router,
        store,
        components: {
            MessageCenter,
            HomeViewer
        },
        beforeMount() {
            this.fetchRecipes();
        }
    };
</script>

<style lang="scss">
    @import "./App.vue";
</style>