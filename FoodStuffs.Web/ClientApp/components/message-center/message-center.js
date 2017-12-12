require("./message-center.scss");

Vue.component("message-center", {
    template: require("./message-center.html"),
    props: ["messages", "isError"]
});