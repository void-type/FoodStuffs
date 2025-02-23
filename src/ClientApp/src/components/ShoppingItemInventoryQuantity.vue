<script lang="ts" setup>
import { type PropType } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import useMessageStore from '@/stores/messageStore';
import type { HttpResponse } from '@/api/http-client';
import type ShoppingItemInventoryWorking from '@/models/ShoppingItemInventoryWorking';

const model = defineModel({
  type: Number as PropType<number | null | undefined>,
  required: true,
});

defineProps({
  id: {
    type: String,
    required: true,
  },
  item: {
    type: Object as PropType<ShoppingItemInventoryWorking>,
    required: false,
    default: () => {},
  },
  inline: {
    type: Boolean,
    required: false,
    default: false,
  },
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

async function onInventoryChange(item: ShoppingItemInventoryWorking) {
  const inventoryQuantity = Math.max(0, model.value || 0);

  const request = {
    id: item.id,
    inventoryQuantity,
  };

  try {
    const response = await api().shoppingItemsSaveInventory(request);

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}
</script>

<template>
  <div v-if="!inline">
    <label :for="id" class="form-label">Inventory</label>
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
      @change="onInventoryChange(item)"
    />
  </div>
  <div v-else>
    <label :for="id" class="visually-hidden">Inventory</label>
    <div class="input-group">
      <div class="input-group-text">Inventory</div>
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
        @change="onInventoryChange(item)"
      />
    </div>
  </div>
</template>

<style scoped>
.input-group-text {
  background-color: unset;
}
</style>
