<template>
    <div v-if="messages.length > 0"
         :class="{messages: true, danger: isError}"
         @click="clearMessages()">
        <ul>
            <li v-for="message in messages"
                :key="message">
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
    @import "./MessageCenter.vue";
</style>