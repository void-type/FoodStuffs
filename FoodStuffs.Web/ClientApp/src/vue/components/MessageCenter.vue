<template>
    <div :class="{messages: true, danger: isError}" v-if="messages.length > 0">
        <div v-on:click="clearMessages()">
            <span>
                &#x2716
            </span>
        </div>
        <ul>
            <li v-for="message in messages" v-bind:key="message">{{message}}</li>
        </ul>
    </div>
</template>

<script>
    import { mapActions } from "vuex";

    export default {
        computed:
        {
            messages() {
                return this.$store.state.messages;
            },
            isError() {
                return this.$store.state.isError;
            }
        },
        methods:
        {
            ...mapActions({
                clearMessages: "clearErrors"
            })
        }
    }
</script>

<style lang="scss" scoped>
    @import "./MessageCenter";
</style>