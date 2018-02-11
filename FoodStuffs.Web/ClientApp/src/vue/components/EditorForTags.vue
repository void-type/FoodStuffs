<template>
    <div class="form-group">
        <div>
            <div class="tags">
                <span v-for="tag in tags" v-bind:key="tag">
                    {{tag}}&nbsp;&nbsp;
                    <span v-on:click="removeTagClick(tag)">
                        &#x2716;
                    </span>
                </span>
            </div>
        </div>

        <div>
            <input type="text"
                   v-model="newTagName"
                   v-bind:id="fieldName"
                   v-bind:name="fieldName" />
            <button v-on:click.prevent="addTagClick()">Add</button>
        </div>

        <label v-bind:for="fieldName">{{label}}</label>
    </div>
</template>

<script>
    export default {
        props: {
            fieldName: {
                type: String,
                required: true
            },
            label: {
                type: String,
                required: true
            },
            tags: {
                type: Array,
                required: true
            },
            addTag: {
                type: Function,
                required: true
            },
            removeTag: {
                type: Function,
                required: true
            }
        },
        data: function () {
            return {
                newTagName: ""
            };
        },
        methods: {
            addTagClick() {
                this.addTag(this.newTagName);
                this.newTagName = "";
            },

            removeTagClick(tagToRemove) {
                this.removeTag(tagToRemove);
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./EditorForTags.vue";
</style>