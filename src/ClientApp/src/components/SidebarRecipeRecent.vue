<script lang="ts" setup>
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import SelectSidebarList from '@/components/SidebarRecipeBase.vue';

defineProps({
  routeName: {
    type: String,
    required: true,
  },
});

const recipeStore = useRecipeStore();

const { recentRecipes } = storeToRefs(recipeStore);
</script>

<template>
  <div>
    <SelectSidebarList
      v-if="recentRecipes.length > 0"
      :recipes="recentRecipes"
      title="Recent"
      :route-name="routeName"
    />
  </div>
</template>

<style lang="scss" scoped></style>
