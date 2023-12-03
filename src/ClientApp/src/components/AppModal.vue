<script lang="ts" setup>
import useAppStore from '@/stores/appStore';
import { watch } from 'vue';
import { storeToRefs } from 'pinia';
import { Modal } from 'bootstrap';

const appStore = useAppStore();
const { modalIsActive, modalParameters } = storeToRefs(appStore);

const modalControllerOptions: Modal.Options = {
  backdrop: 'static',
  keyboard: false,
  focus: true,
};

function getModal() {
  return Modal.getOrCreateInstance('#app-modal', modalControllerOptions);
}

function okAction() {
  modalIsActive.value = false;
  const action = modalParameters.value.okAction;

  if (action) {
    action();
    // prevent duplicate firings
    modalParameters.value.okAction = undefined;
  }
}

function cancelAction() {
  modalIsActive.value = false;
  const action = modalParameters.value.cancelAction;

  if (action) {
    action();
    // prevent duplicate firings
    modalParameters.value.cancelAction = undefined;
  }
}

watch(modalIsActive, (isActive) => {
  if (isActive) {
    getModal().show();
  } else {
    getModal().hide();
  }
});
</script>

<template>
  <Teleport to="body">
    <div
      id="app-modal"
      class="modal fade"
      tabindex="-1"
      aria-labelledby="app-modal-title"
      aria-hidden="true"
      @keydown.esc.prevent.stop="cancelAction()"
    >
      <div class="modal-dialog modal-dialog-scrollable modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 id="app-modal-title" class="modal-title">{{ modalParameters.title }}</h5>
            <button
              type="button"
              class="btn-close"
              aria-label="Cancel"
              @click="cancelAction()"
            ></button>
          </div>
          <div class="modal-body">{{ modalParameters.description }}</div>
          <div class="modal-footer">
            <button
              id="app-modal-cancel-button"
              type="button"
              class="btn btn-outline-light"
              @click="cancelAction()"
            >
              Cancel
            </button>
            <button
              id="app-modal-ok-button"
              type="button"
              class="btn btn-primary"
              @click="okAction()"
            >
              OK
            </button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<style lang="scss" scoped></style>
