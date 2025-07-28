<script lang="ts" setup>
import { type PropType } from 'vue';
import useMessageStore from '@/stores/messageStore';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const model = defineModel({
  type: Number as PropType<number | null | undefined>,
  required: true,
});

const props = defineProps({
  id: {
    type: String,
    required: true,
  },
  label: {
    type: String,
    required: true,
  },
  inline: {
    type: Boolean,
    required: false,
    default: false,
  },
  min: {
    type: Number,
    required: false,
    default: 0,
  },
  max: {
    type: Number,
    required: false,
    default: Number.MAX_SAFE_INTEGER,
  },
});

const messageStore = useMessageStore();

const changeInventory = (amount: number) => {
  model.value = Math.min(props.max, Math.max(props.min, (model.value || 0) + amount));
};
</script>

<template>
  <div v-if="!inline">
    <label :for="id" class="form-label">{{ label }}</label>
    <div class="input-group">
      <button
        class="btn btn-outline-secondary text-monospace"
        aria-label="Decrement inventory by one."
        type="button"
        @click="changeInventory(-1)"
      >
        <font-awesome-icon icon="fa-minus" />
      </button>
      <input
        :id="id"
        v-model="model"
        required
        type="number"
        min="0"
        :class="{
          'form-control': true,
          'is-invalid': messageStore.isFieldInError(id),
        }"
      />
      <button
        class="btn btn-outline-secondary text-monospace"
        aria-label="Decrement inventory by one."
        type="button"
        @click="changeInventory(1)"
      >
        <font-awesome-icon icon="fa-plus" />
      </button>
    </div>
  </div>
  <div v-else>
    <label :for="id" class="visually-hidden">Inventory</label>
    <div class="input-group">
      <div class="input-group-text">Inventory</div>
      <button
        class="btn btn-outline-secondary text-monospace"
        aria-label="Decrement inventory by one."
        type="button"
        @click="changeInventory(-1)"
      >
        <font-awesome-icon icon="fa-minus" />
      </button>
      <input
        :id="id"
        v-model="model"
        required
        type="number"
        min="0"
        :class="{
          'form-control': true,
          'is-invalid': messageStore.isFieldInError(id),
        }"
      />
      <button
        class="btn btn-outline-secondary text-monospace"
        aria-label="Decrement inventory by one."
        type="button"
        @click="changeInventory(1)"
      >
        <font-awesome-icon icon="fa-plus" />
      </button>
    </div>
  </div>
</template>

<style scoped>
.input-group-text {
  cursor: default;
}
</style>
