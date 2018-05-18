<template>
    <section>
        <SelectSidebar />
        <EditorForm />
    </section>
</template>

<script>
import SelectSidebar from '../components/SelectSidebar.vue';
import EditorForm from '../components/EditorForm.vue';

export default {
  components: {
    SelectSidebar,
    EditorForm,
  },
};
</script>

<style lang="scss" scoped>

</style>
