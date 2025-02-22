<script lang="ts" setup>
import type { SaveMealPlanRequestPantryShoppingItem } from '@/api/data-contracts';
import { computed, reactive, type PropType } from 'vue';
import ShoppingItemInventoryQuantity from './ShoppingItemInventoryQuantity.vue';

const props = defineProps({
  title: { type: String, required: true },
  shoppingItems: { type: Array<SaveMealPlanRequestPantryShoppingItem>, required: true },
  onClear: { type: Function as PropType<() => unknown | null>, required: false, default: null },
  onItemClick: { type: Function, required: true },
  getShoppingItemDetails: { type: Function, required: true },
  showCopyList: { type: Boolean, required: false, default: false },
});

const shoppingItemsSorted = computed(() => {
  const sortableItems = props.shoppingItems.map((x) => ({
    value: x,
    sortValue: props.getShoppingItemDetails(x.id)?.name || '',
  }));

  sortableItems.sort((a, b) => a?.sortValue.localeCompare(b?.sortValue));

  return sortableItems.map((x) => x.value);
});

const defaultCopyTooltip = 'Copy list';

const data = reactive({
  copyTooltipText: defaultCopyTooltip,
});

function clear() {
  if (props.onClear !== null) {
    props.onClear();
  }
}

function copyList() {
  // This doesn't paste as multiple items from firefox (chrome works)
  const text = shoppingItemsSorted.value
    .map((x) => `${x.quantity}x ${props.getShoppingItemDetails(x.id)?.name}`)
    .join(`\n`);

  navigator.clipboard.writeText(text);
  data.copyTooltipText = 'List copied!';
}
</script>

<template>
  <div>
    <div class="card">
      <div class="card-header fw-bold d-flex justify-content-between align-items-center">
        {{ title }}
        <button
          v-if="onClear !== null"
          type="button"
          class="btn btn-sm btn-secondary"
          @click.stop.prevent="clear()"
        >
          Clear
        </button>
        <div v-if="showCopyList" class="copy-tooltip">
          <button
            type="button"
            class="btn btn-sm btn-secondary"
            :title="data.copyTooltipText"
            @click.stop.prevent="copyList()"
            @mouseout="data.copyTooltipText = defaultCopyTooltip"
            @focusout="data.copyTooltipText = defaultCopyTooltip"
          >
            <span id="copyTooltipText" class="copy-tooltip-text">{{ data.copyTooltipText }}</span>
            Copy
          </button>
        </div>
      </div>
      <ul class="list-group list-group-flush">
        <li
          v-for="{ id, quantity } in shoppingItemsSorted"
          :key="id"
          tabindex="0"
          role="button"
          class="list-group-item card-hover p-3"
          @keydown.stop.prevent.enter="onItemClick(id)"
          @click="onItemClick(id)"
        >
          <div class="grid gap-sm">
            <div class="g-col-12 g-col-xl-8">
              {{ quantity }}x {{ getShoppingItemDetails(id)?.name }}
            </div>
            <ShoppingItemInventoryQuantity
              :id="`inventory-${id}`"
              v-model="(getShoppingItemDetails(id) || { inventoryQuantity: 0 }).inventoryQuantity"
              class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-4"
              :inline="true"
              :item="{ id, inventoryQuantity: getShoppingItemDetails(id)?.inventoryQuantity }"
              @click.stop.prevent
            />
          </div>
        </li>
        <li v-if="shoppingItemsSorted.length < 1" class="list-group-item p-4 text-center">
          No shopping items.
        </li>
      </ul>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.copy-tooltip {
  position: relative;
}

.copy-tooltip .copy-tooltip-text {
  visibility: hidden;
  width: 120px;
  background-color: var(--bs-gray-700);
  color: var(--bs-white);
  text-align: center;
  border-radius: 6px;
  padding: 5px;
  position: absolute;
  z-index: 1;
  bottom: 150%;
  left: 50%;
  margin-left: -60px;
  opacity: 0;
  transition: opacity 0.3s;
}

.copy-tooltip .copy-tooltip-text::after {
  content: '';
  position: absolute;
  top: 100%;
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: #555 transparent transparent transparent;
}

.copy-tooltip:hover .copy-tooltip-text {
  visibility: visible;
  opacity: 1;
}
</style>
