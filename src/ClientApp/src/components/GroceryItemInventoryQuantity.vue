<script lang="ts" setup>
import type { PropType } from 'vue';
import type { HttpResponse } from '@/api/http-client';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import { debounce } from '@/models/InputHelper';
import useMessageStore from '@/stores/messageStore';

const props = defineProps({
  id: {
    type: String,
    required: true,
  },
  itemId: {
    type: Number,
    required: false,
    default: () => 0,
  },
  inline: {
    type: Boolean,
    required: false,
    default: false,
  },
});

const model = defineModel({
  type: Number as PropType<number | null | undefined>,
  required: true,
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const isExpanded = ref(false);

const onInventoryChange = debounce(async () => {
  const inventoryQuantity = Math.max(0, model.value || 0);

  const request = {
    id: props.itemId,
    inventoryQuantity,
  };

  try {
    const response = await api().groceryItemsSaveInventory(request);

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
});

function changeInventory(amount: number) {
  model.value = Math.max(0, (model.value || 0) + amount);
  onInventoryChange();
}

function toggleExpanded() {
  isExpanded.value = !isExpanded.value;
}
</script>

<template>
  <div v-if="!inline">
    <label :for="id" class="form-label">Inventory</label>
    <div class="input-group">
      <button
        class="btn btn-outline-secondary text-monospace"
        aria-label="Decrement inventory by one."
        type="button"
        @click="changeInventory(-1)"
      >
        <FontAwesomeIcon icon="fa-minus" />
      </button>
      <input
        :id="id"
        v-model="model"
        required
        type="number"
        min="0"
        class="form-control" :class="{
          'is-invalid': messageStore.isFieldInError(id),
        }"
        @change="onInventoryChange()"
      >
      <button
        class="btn btn-outline-secondary text-monospace"
        aria-label="Increment inventory by one."
        type="button"
        @click="changeInventory(1)"
      >
        <FontAwesomeIcon icon="fa-plus" />
      </button>
    </div>
  </div>
  <div v-else>
    <label :for="id" class="visually-hidden">Inventory</label>
    <div class="input-group">
      <button class="input-group-text" type="button" @click="toggleExpanded">
        Inventory
      </button>
      <Transition name="slide-left">
        <button
          v-if="isExpanded"
          class="btn btn-outline-secondary text-monospace"
          aria-label="Decrement inventory by one."
          type="button"
          @click="changeInventory(-1)"
        >
          <FontAwesomeIcon icon="fa-minus" />
        </button>
      </Transition>
      <input
        :id="id"
        v-model="model"
        required
        type="number"
        min="0"
        class="form-control" :class="{
          'is-invalid': messageStore.isFieldInError(id),
        }"
        @change="onInventoryChange()"
      >
      <Transition name="slide-right">
        <button
          v-if="isExpanded"
          class="btn btn-outline-secondary text-monospace"
          aria-label="Increment inventory by one."
          type="button"
          @click="changeInventory(1)"
        >
          <FontAwesomeIcon icon="fa-plus" />
        </button>
      </Transition>
    </div>
  </div>
</template>

<style scoped>
.input-group-text {
  cursor: pointer;
}

.input-group {
  overflow: hidden;
}

.slide-left-enter-active,
.slide-left-leave-active,
.slide-right-enter-active,
.slide-right-leave-active {
  transition: all 0.1s ease;
}

.slide-left-enter-from,
.slide-left-leave-to {
  transform: scaleX(0);
  transform-origin: left;
  opacity: 0;
}

.slide-right-enter-from,
.slide-right-leave-to {
  transform: scaleX(0);
  transform-origin: right;
  opacity: 0;
}
</style>
