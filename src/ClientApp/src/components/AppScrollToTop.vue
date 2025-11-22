<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { onMounted, onUnmounted, ref } from 'vue';
import RouterHelper from '@/models/RouterHelper';

const props = defineProps({
  show: {
    type: Boolean,
    default: false,
  },
});

const isWithinScrollRange = ref(true);

function handleScroll() {
  const scrollTop = window.scrollY || document.documentElement.scrollTop;
  const { clientHeight } = document.documentElement;
  const footer = document.getElementById('footer');
  const footerTop = footer?.getBoundingClientRect().top ?? 0;

  isWithinScrollRange.value = scrollTop > 300 && footerTop > clientHeight;
}

onMounted(async () => {
  window.addEventListener('scroll', handleScroll);
  handleScroll();
});

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll);
});
</script>

<template>
  <Transition name="fade">
    <div v-if="props.show || isWithinScrollRange" class="scroll-to-top">
      <button
        class="btn btn-outline-secondary rounded-circle opacity-75"
        type="button"
        @click="RouterHelper.scrollToTop()"
      >
        <FontAwesomeIcon icon="fa-arrow-up" />
      </button>
    </div>
  </Transition>
</template>

<style lang="scss" scoped>
.scroll-to-top {
  position: fixed;
  bottom: 1rem;
  left: 50%;
  transform: translateX(-50%);
  z-index: 1000;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
