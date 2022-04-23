<script setup lang="ts">
import { computed, onMounted, reactive, ref, type Directive } from 'vue';
import { storeToRefs } from 'pinia';
import useAppStore from '@/stores/appStore';

const appStore = useAppStore();
const { messages, messageIsError } = storeToRefs(appStore);
const { clearMessages } = appStore;

// Get the initial offset for the top of the message center
let messageCenterTop = 0;
onMounted(() => {
  messageCenterTop = document.getElementById('message-center')?.getBoundingClientRect()?.top || 0;
});

// When the message center scrolls off the page, fix it to the top.
const messageCenterIsFixed = ref(false);
const vMessageCenterScroll: Directive = {
  mounted() {
    function onScroll() {
      messageCenterIsFixed.value = window.scrollY >= messageCenterTop;
    }
    window.addEventListener('scroll', onScroll);
  },
};

// We need an element the same height as the message center to ensure smooth scrolling
// when the center is fixed to the top of the page.
const messageCenterDiv = reactive(ref<HTMLDivElement | null>());
const messageCenterHeight = computed(() => {
  const element = messageCenterDiv.value;

  if (element == null || typeof element === 'undefined') {
    return 0;
  }

  const styles = window.getComputedStyle(element);
  const margin = parseFloat(styles.marginTop) + parseFloat(styles.marginBottom);

  return Math.ceil(element.offsetHeight + margin);
});
</script>

<template>
  <div>
    <div
      v-show="messages.length > 0 && messageCenterIsFixed"
      :style="{ height: messageCenterHeight + 'px' }"
    />
    <div
      id="message-center"
      ref="messageCenterDiv"
      v-message-center-scroll
      :class="{ 'fixed-alert': messageCenterIsFixed }"
    >
      <div
        v-show="messages.length > 0"
        :class="{
          alert: true,
          'alert-dismissible': true,
          shadow: true,
          'mb-0': true,
          'alert-danger': messageIsError,
          'alert-success': !messageIsError,
        }"
      >
        <ul>
          <li v-for="(message, id) in messages" :key="id">
            {{ message }}
          </li>
        </ul>
        <button
          type="button"
          class="btn-close"
          aria-label="Close"
          @click="clearMessages()"
        ></button>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
div.alert {
  ul {
    text-align: center;
    list-style: none;
    margin: 0;
  }
}

div.fixed-alert {
  position: fixed;
  top: 0;
  z-index: 999;
  width: 100%;
}
</style>
