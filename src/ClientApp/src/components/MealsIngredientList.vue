<script lang="ts" setup>
import type { PropType } from 'vue';

const props = defineProps({
  title: { type: String, required: true },
  ingredients: { type: Object as PropType<[string, number][]>, required: true },
  onClear: { type: Function as PropType<() => unknown | null>, required: false, default: null },
  onIngredientClick: { type: Function, required: true },
  showCopyList: { type: Boolean, required: false, default: false },
});

function clear() {
  if (props.onClear !== null) {
    props.onClear();
  }
}

function copyList() {
  // TODO: add tooltip: https://www.w3schools.com/howto/tryit.asp?filename=tryhow_js_copy_clipboard2
  // TODO: this doesn't paste as multiple items from firefox (chrome works)
  var text = props.ingredients.map((x) => `${x[1]}x ${x[0]}`).join(`\n`);
  navigator.clipboard.writeText(text);
}
</script>

<template>
  <div>
    <div class="card">
      <h5 class="card-header d-flex justify-content-between align-items-center">
        {{ title }}
        <button v-if="onClear !== null" type="button" class="btn btn-secondary" @click="clear()">
          Clear
        </button>
        <button v-if="showCopyList" type="button" class="btn btn-secondary" @click="copyList()">
          Copy
        </button>
      </h5>
      <ul class="list-group list-group-flush">
        <li
          v-for="[ingredient, quantity] in ingredients"
          :key="ingredient"
          tabindex="0"
          role="button"
          class="list-group-item card-hover"
          @keydown.stop.prevent.enter="onIngredientClick(ingredient)"
          @click="onIngredientClick(ingredient)"
        >
          {{ quantity }}x {{ ingredient }}
        </li>
        <li v-if="ingredients.length < 1" class="list-group-item p-4 text-center">
          No ingredients
        </li>
      </ul>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
