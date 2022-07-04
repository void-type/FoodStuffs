<script lang="ts" setup>
import type { PropType } from 'vue';
import type { ListRecipesResponse } from '@/api/data-contracts';

const props = defineProps({
  recipes: {
    type: Object as PropType<Array<ListRecipesResponse>>,
    required: true,
  },
  title: {
    type: String,
    required: false,
    default: '',
  },
  routeName: {
    type: String,
    required: true,
  },
});
</script>

<template>
  <div class="card">
    <div class="card-header h5 mb-0">
      {{ title }}
    </div>
    <div class="list-group list-group-flush">
      <router-link
        v-for="recipe in recipes"
        :key="recipe.id"
        class="list-group-item"
        :to="{ name: props.routeName, params: { id: recipe.id } }"
      >
        {{ recipe.name }}
      </router-link>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
