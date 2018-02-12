<template>
    <div>
        <div>
            <div class="tags">
                <span v-for="tag in tags" :key="tag">
                    {{tag}}&nbsp;&nbsp;
                    <span @click="removeTagClick(tag)">
                        &#x2716;
                    </span>
                </span>
            </div>
        </div>

        <div>
            <input type="text"
                   v-model="newTagName"
                   :id="fieldName"
                   :name="fieldName"
                   @keydown.enter.prevent="addTagClick()" />
            <button @click.prevent="addTagClick()">
                Add
            </button>
        </div>

        <label :for="fieldName">{{label}}</label>
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
            }
        },
        data: function () {
            return {
                newTagName: ""
            };
        },
        methods: {
            addTagClick() {
                this.$emit("addTag", this.newTagName);
                this.newTagName = "";
            },

            removeTagClick(tagToRemove) {
                this.$emit("removeTag", tagToRemove);
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "./EditorForTags.vue";
</style>