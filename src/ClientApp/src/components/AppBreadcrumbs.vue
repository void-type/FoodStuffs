<script lang="ts" setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const breadcrumbs = computed(() => {
  const matched = route.matched
    .filter((r) => r.meta.title)
    .map((r) => ({
      name: r.name || r.children.find((c) => c.path === '')?.name,
      // TODO: look up params from the store and merge so we're taken back to our original list.
      params: route.params,
      title: r.meta.title,
    }));

  return matched;
});
</script>

<template>
  <div class="mt-2">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li
          v-for="(segment, index) in breadcrumbs"
          :key="index"
          class="breadcrumb-item"
          :aria-current="index === breadcrumbs.length - 1 ? 'page' : false"
        >
          <router-link
            v-if="index !== breadcrumbs.length - 1"
            :to="{ name: segment.name, params: segment.params }"
            >{{ segment.title }}</router-link
          >
          <span v-else>{{ segment.title }}</span>
        </li>
      </ol>
    </nav>
  </div>
</template>

<style lang="scss" scoped></style>
