<script lang="ts" setup>
import { onMounted, reactive } from 'vue';

// TODO: finish implementing from here: https://stackabuse.com/how-to-create-a-confirmation-dialogue-in-vue-js/
const data = reactive({
  id: 'app-modal',
  title: '',
  description: '',
  // eslint-disable-next-line no-unused-vars
  resolvePromise: undefined as unknown as (value?: unknown) => void | undefined,
  // eslint-disable-next-line no-unused-vars
  rejectPromise: undefined as unknown as (reason?: unknown) => void | undefined,
});

function show(options: { title: string; description: string }) {
  data.title = options.title;
  data.description = options.description;

  return new Promise((resolve, reject) => {
    data.resolvePromise = resolve;
    data.rejectPromise = reject;
  });
}

onMounted(() => {
  const modal = document.getElementById(data.id);
  const button = document.getElementById(`${data.id}-cancel-button`);

  modal?.addEventListener('shown.bs.modal', () => {
    button?.focus();
  });
});
</script>

<template>
  <Teleport to="body">
    <div
      id="{{id}}"
      class="modal fade"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="recipe-delete-modal"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 id="recipe-delete-modal" class="modal-title">{{ data.title }}</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">{{ data.description }}</div>
          <div class="modal-footer">
            <button
              :id="`${data.id}-cancel-button`"
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
            >
              No
            </button>
            <button type="button" class="btn btn-primary" @click="okAction()">Yes</button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<style lang="scss" scoped></style>
