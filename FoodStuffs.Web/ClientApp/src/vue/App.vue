<template>
    <div>
        <div id="no-print">
            <header>
                <div class="topbar">
                    <router-link v-bind:to="{name: 'home'}" class="logo">
                        <img src="../assets/logo.png" alt="FoodStuffs logo" />
                        <span>{{applicationName}}</span>
                    </router-link>
                    <nav>
                        <ul>
                            <li><router-link v-bind:to="{name: 'home'}">Home</router-link></li>
                            <li><router-link v-bind:to="{name: 'search'}">Search</router-link></li>
                        </ul>
                    </nav>
                    <router-link v-bind:to="{name: 'home'}" class="pull-right">Login</router-link>
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
        <div id="print-only">
            <RecipeViewer></RecipeViewer>
        </div>
    </div>
</template>

<script>
    import { mapActions } from "vuex";
    import router from "../router";
    import store from "../store";
    import MessageCenter from "./components/MessageCenter";
    import RecipeViewer from "./components/RecipeViewer";

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
            ...mapActions({
                refresh: "fetchRecipes"
            })
        },
        router,
        store,
        components: {
            MessageCenter,
            RecipeViewer
        },
        beforeMount() {
            this.refresh();
        }
    };
</script>

<style lang="scss">
    @import "./App";
</style>