<template>
    <div class="form-group">
        <div>
            <div>
                <input type="text" v-bind:id="fieldName" v-bind:name="fieldName" v-model="newTagName" />
                <button v-on:click.prevent="addTagClick()">Add</button>
            </div>
            <div class="tags">
                <span v-for="tag in tags" v-bind:key="tag">
                    {{tag}}&nbsp;&nbsp;
                    <span v-on:click="removeTagClick(tag)">
                        &#x2716;
                    </span>
                </span>
            </div>
        </div>
        <label v-bind:for="fieldName">{{label}}</label>
    </div>
</template>

<script>
    export default {
        props: ["fieldName", "label", "tags", "addTag", "removeTag"],
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
    @import "./TagEditor";
</style>