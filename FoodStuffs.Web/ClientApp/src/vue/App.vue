<template>
    <div>
        <div id="no-print">
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
        <div id="print-only">
            <RecipeViewer></RecipeViewer>
        </div>
    </div>
</template>

<script>
    import { mapActions } from "vuex";
    import router from "../router"
    import store from "../store";
    import MessageCenter from "./components/MessageCenter";
    import RecipeViewer from "./components/RecipeViewer";

    export default {
        data: function () {
            return {
                applicationName: document.getElementById("applicationName") !== null ? document.getElementById("applicationName").value : "FoodStuffs"
            }
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