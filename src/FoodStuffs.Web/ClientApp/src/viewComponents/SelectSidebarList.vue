<template>
  <div
    class="card mt-2"
  >
    <b-card-header
      class="h5 mb-0 hover"
      @click="toggleSidebarVisible"
    >
      {{ title }}
    </b-card-header>
    <b-collapse
      :id="`collapse-${title}`"
      :visible="isSidebarVisible"
    >
      <b-list-group
        flush
      >
        <b-list-group-item
          v-for="recipe in recipes"
          :key="recipe.id"
          :to="{ name: routeName, params: { id: recipe.id } }"
        >
          {{ recipe.name }}
        </b-list-group-item>
      </b-list-group>
    </b-collapse>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import router from '@/router';

export default {
  props: {
    recipes: {
      type: Array,
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
  },
  data() {
    return {
      isScreenLarge: false,
    };
  },
  computed: {
    ...mapGetters({
      isSidebarVisibleSetting: 'sidebar/sidebarVisible',
    }),
    isSidebarVisible() {
      return this.isSidebarVisibleSetting || this.isScreenLarge;
    },
  },
  created() {
    this.setIsScreenLarge();

    // Initialize sidebars as collapsed on small screens.
    if (this.isSidebarVisibleSetting === null) {
      this.setSidebarVisibleSetting(this.isScreenLarge);
    }

    window.addEventListener('resize', this.setIsScreenLarge);
  },
  destroyed() {
    window.removeEventListener('resize', this.setIsScreenLarge);
  },
  methods: {
    ...mapActions({
      setSidebarVisibleSetting: 'sidebar/setSidebarVisible',
    }),
    toggleSidebarVisible() {
      if (this.isScreenLarge) {
        return;
      }

      this.setSidebarVisibleSetting(!this.isSidebarVisibleSetting);
    },
    setIsScreenLarge() {
      const width = window.innerWidth
      || document.documentElement.clientWidth
      || document.body.clientWidth;

      this.isScreenLarge = width >= 992;
    },
    viewRecipe(recipe) {
      router.push({ name: this.routeName, params: { id: recipe.id } }).catch(() => {});
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/style/theme";

@include media-breakpoint-down(md) {
  .hover:hover {
    background-color: $gray-200;
    cursor: pointer;
  }
}
</style>
