<script lang="ts" setup>
import type {
  GetMealPlanResponseExcludedShoppingItem,
  GetMealPlanResponseRecipeShoppingItem,
} from '@/api/data-contracts';
import { computed, reactive, type PropType } from 'vue';
import ShoppingItemInventoryQuantity from './ShoppingItemInventoryQuantity.vue';

const props = defineProps({
  title: { type: String, required: true },
  shoppingItems: { type: Array<GetMealPlanResponseExcludedShoppingItem>, required: true },
  onItemClick: { type: Function, required: true },
  showCopyList: { type: Boolean, required: false, default: false },
  onClear: { type: Function as PropType<() => unknown | null>, required: false, default: null },
  getShoppingItemDetails: { type: Function, required: true },
  getGroceryDepartmentDetails: { type: Function, required: true },
});

interface GroupItem extends GetMealPlanResponseExcludedShoppingItem {
  details: GetMealPlanResponseRecipeShoppingItem;
}

const shoppingItemsGrouped = computed(() => {
  const items = props.shoppingItems.map((x) => {
    return {
      ...x,
      details: props.getShoppingItemDetails(x.id) || {},
    };
  });

  items.sort((a, b) => (a?.details.name || '').localeCompare(b?.details.name || ''));

  const groupedById = new Map<number, Array<GroupItem>>();

  items.forEach((item) => {
    if (!groupedById.has(item.details.groceryDepartmentId)) {
      groupedById.set(item.details.groceryDepartmentId, []);
    }

    groupedById.get(item.details.groceryDepartmentId)?.push(item);
  });

  const grouped = Array.from(groupedById).map((x) => ({
    groceryDepartmentId: x[0],
    groceryDepartment: props.getGroceryDepartmentDetails(x[0]) || {
      name: 'No department',
      order: Number.MAX_VALUE,
      id: 0,
    },
    items: x[1],
  }));

  grouped.sort((a, b) => {
    if (a.groceryDepartment === null || b.groceryDepartment === null) {
      return 0;
    }

    return a.groceryDepartment.order - b.groceryDepartment.order;
  });

  return grouped;
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
  const lines: string[] = [];

  shoppingItemsGrouped.value.forEach((group) => {
    lines.push(`# ${group.groceryDepartment.name}`);

    group.items.forEach((item) => {
      lines.push(`${item.quantity}x ${item.details.name}`);
    });
  });

  // This doesn't paste as multiple items from firefox (chrome works)
  const text = lines.join(`\n`);

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
      <div v-for="group in shoppingItemsGrouped" :key="group.groceryDepartmentId">
        <div class="card-header">
          {{ group.groceryDepartment.name }}
        </div>
        <ul class="list-group list-group-flush">
          <li
            v-for="item in group.items"
            :key="item.id"
            tabindex="0"
            role="button"
            class="list-group-item card-hover p-3"
            @keydown.stop.prevent.enter="onItemClick(item.id)"
            @click="onItemClick(item.id)"
          >
            <div class="grid gap-sm">
              <div class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-6">
                {{ item.quantity }}x {{ item.details.name }}
              </div>
              <ShoppingItemInventoryQuantity
                :id="`inventory-${item.id}`"
                v-model="item.details.inventoryQuantity"
                class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-6"
                :inline="true"
                :item="item"
                @click.stop.prevent
              />
            </div>
          </li>
        </ul>
      </div>
      <ul class="list-group list-group-flush">
        <li v-if="shoppingItemsGrouped.length < 1" class="list-group-item p-4 text-center">
          No grocery items.
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
