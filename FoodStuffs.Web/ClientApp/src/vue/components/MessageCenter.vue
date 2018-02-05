<template>
    <div v-if="messages.length > 0"
         v-bind:class="{messages: true, danger: isError}"
         v-on:click="clearMessages()">
        <ul>
            <li v-for="message in messages"
                v-bind:key="message">
                {{message}}
            </li>
        </ul>
    </div>
</template>

<script>
    import { mapActions, mapGetters } from "vuex";

    export default {
        computed: {
            ...mapGetters(["messages", "isError"])
        },
        methods: {
            ...mapActions(["clearMessages"])
        }
    };
</script>

<style lang="scss" scoped>
    @import "./MessageCenter";
</style>