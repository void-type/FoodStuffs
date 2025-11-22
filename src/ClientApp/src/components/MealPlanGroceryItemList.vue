<script lang="ts" setup>
import type { PropType } from 'vue';
import type {
  GetMealPlanResponseExcludedGroceryItem,
  GetMealPlanResponseRecipeGroceryItem,
} from '@/api/data-contracts';
import { computed, reactive } from 'vue';
import GroceryItemInventoryQuantity from './GroceryItemInventoryQuantity.vue';

const props = defineProps({
  title: { type: String, required: true },
  groceryItems: { type: Array<GetMealPlanResponseExcludedGroceryItem>, required: true },
  onItemClick: { type: Function, required: true },
  buttonLabel: { type: String, required: false, default: 'Move' },
  showCopyList: { type: Boolean, required: false, default: false },
  onClear: { type: Function as PropType<() => unknown | null>, required: false, default: null },
  collapsable: { type: Boolean, required: false, default: false },
  collapsed: { type: Boolean, required: false, default: false },
  getGroceryItemDetails: { type: Function, required: true },
  getGroceryAisleDetails: { type: Function, required: true },
});

interface GroupItem extends GetMealPlanResponseExcludedGroceryItem {
  details: GetMealPlanResponseRecipeGroceryItem;
}

const groceryItemsGrouped = computed(() => {
  const items = props.groceryItems.map((x) => {
    return {
      ...x,
      details: props.getGroceryItemDetails(x.id) || {},
    };
  });

  items.sort((a, b) => (a?.details.name || '').localeCompare(b?.details.name || ''));

  const groupedById = new Map<number, Array<GroupItem>>();

  items.forEach((item) => {
    if (!groupedById.has(item.details.groceryAisleId)) {
      groupedById.set(item.details.groceryAisleId, []);
    }

    groupedById.get(item.details.groceryAisleId)?.push(item);
  });

  const grouped = Array.from(groupedById).map(x => ({
    groceryAisleId: x[0],
    groceryAisle: props.getGroceryAisleDetails(x[0]) || {
      name: 'No aisle',
      order: Number.MAX_VALUE,
      id: 0,
    },
    items: x[1],
  }));

  grouped.sort((a, b) => {
    if (a.groceryAisle === null || b.groceryAisle === null) {
      return 0;
    }

    return a.groceryAisle.order - b.groceryAisle.order;
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

const itemCount = computed(() => {
  const count = props.groceryItems.length;
  return count > 0 ? ` (${count})` : '';
});

function copyList() {
  const lines: string[] = [];

  groceryItemsGrouped.value.forEach((group) => {
    lines.push(`# ${group.groceryAisle.name}`);

    group.items.forEach((item) => {
      lines.push(`${item.quantity}x ${item.details.name}`);
    });
  });

  // This doesn't paste as multiple items from firefox (chrome works)
  const text = lines.join('\n');

  navigator.clipboard.writeText(text);
  data.copyTooltipText = 'List copied!';
}

const accordionId = computed(() => `accordion-${Math.random().toString(36).substr(2, 9)}`);
const collapseId = computed(() => `collapse-${Math.random().toString(36).substr(2, 9)}`);
</script>

<template>
  <div>
    <div v-if="!collapsable" class="card">
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
      <div v-for="group in groceryItemsGrouped" :key="group.groceryAisleId">
        <div class="card-header">
          {{ group.groceryAisle.name }}
        </div>
        <ul class="list-group list-group-flush">
          <li v-for="item in group.items" :key="item.id" class="list-group-item p-3">
            <div class="grid gap-sm">
              <div
                class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-6 d-flex justify-content-between align-items-center"
              >
                <span>{{ item.quantity }}x {{ item.details.name }}</span>
                <button type="button" class="btn btn-sm btn-primary" @click="onItemClick(item.id)">
                  {{ buttonLabel }}
                </button>
              </div>
              <GroceryItemInventoryQuantity
                :id="`inventory-${item.id}`"
                v-model="item.details.inventoryQuantity"
                class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-6"
                :inline="true"
                :item-id="item.id"
                @click.stop.prevent
              />
            </div>
          </li>
        </ul>
      </div>
      <ul class="list-group list-group-flush">
        <li v-if="groceryItemsGrouped.length < 1" class="list-group-item p-4 text-center">
          No grocery items.
        </li>
      </ul>
    </div>

    <div v-else :id="accordionId" class="accordion">
      <div class="accordion-item">
        <div class="accordion-header">
          <div
            class="accordion-button" :class="[{ collapsed }]"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="`#${collapseId}`"
            :aria-expanded="!collapsed"
            :aria-controls="collapseId"
          >
            <div class="d-flex justify-content-between align-items-center w-100 me-2">
              <span>{{ title }}{{ itemCount }}</span>
            </div>
          </div>
        </div>
        <div
          :id="collapseId"
          class="accordion-collapse collapse" :class="[{ show: !collapsed }]"
          :data-bs-parent="`#${accordionId}`"
        >
          <div class="accordion-body p-0 card border-none">
            <div class="d-flex gap-2 ms-auto py-2 px-3">
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
                  <span id="copyTooltipText" class="copy-tooltip-text">{{
                    data.copyTooltipText
                  }}</span>
                  Copy
                </button>
              </div>
            </div>
            <div v-for="group in groceryItemsGrouped" :key="group.groceryAisleId">
              <div class="card-header">
                {{ group.groceryAisle.name }}
              </div>
              <ul class="list-group list-group-flush">
                <li v-for="item in group.items" :key="item.id" class="list-group-item p-3">
                  <div class="grid gap-sm">
                    <div
                      class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-6 d-flex justify-content-between align-items-center"
                    >
                      <span>{{ item.quantity }}x {{ item.details.name }}</span>
                      <button
                        type="button"
                        class="btn btn-sm btn-primary"
                        @click="onItemClick(item.id)"
                      >
                        {{ buttonLabel }}
                      </button>
                    </div>
                    <GroceryItemInventoryQuantity
                      :id="`inventory-${item.id}`"
                      v-model="item.details.inventoryQuantity"
                      class="g-col-12 g-col-sm-6 g-col-md-12 g-col-xl-6"
                      :inline="true"
                      :item-id="item.id"
                      @click.stop.prevent
                    />
                  </div>
                </li>
              </ul>
            </div>
            <ul class="list-group list-group-flush">
              <li v-if="groceryItemsGrouped.length < 1" class="list-group-item p-4 text-center">
                No grocery items.
              </li>
            </ul>
          </div>
        </div>
      </div>
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
