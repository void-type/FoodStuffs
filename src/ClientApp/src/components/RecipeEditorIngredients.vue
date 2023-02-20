<script lang="ts" setup>
import type { GetRecipeResponseIngredient } from '@/api/data-contracts';
import { Collapse } from 'bootstrap';
import { computed, nextTick, reactive, ref, watch, type PropType } from 'vue';
import { Sortable } from 'sortablejs-vue3';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { clamp } from '@/models/FormatHelpers';

const props = defineProps({
  modelValue: {
    type: Object as PropType<Array<GetRecipeResponseIngredient>>,
    required: true,
  },
  isFieldInError: {
    type: Function,
    required: true,
  },
});

const emit = defineEmits(['update:modelValue']);

const data = reactive({
  ingredients: [] as GetRecipeResponseIngredient[],
});

function newClick() {
  const newLength = data.ingredients.push({
    name: '',
    quantity: 1,
    order: Math.max(...data.ingredients.map((x) => x.order || 0)) + 1,
    isCategory: false,
  });

  console.log('new', newLength);

  const newIndex = newLength - 1;
  const element = `#ingredient-${newIndex}-accordion-collapse`;

  nextTick(() => Collapse.getOrCreateInstance(element).show());
}

function deleteClick(index: number) {
  console.log('delete', index);

  data.ingredients.splice(index, 1);

  const expandNextIndex = clamp(index, 0, data.ingredients.length - 1);
  const element = `#ingredient-${expandNextIndex}-accordion-collapse`;

  nextTick(() => Collapse.getOrCreateInstance(element).show());
}

function copy(ingredientArray: GetRecipeResponseIngredient[]) {
  return JSON.parse(JSON.stringify(ingredientArray)) as GetRecipeResponseIngredient[];
}

function setOrderFromIndex(ingredientArray: GetRecipeResponseIngredient[]) {
  ingredientArray.forEach((x, i) => (x.order = i + 1));
}

function getSortableIngredients(ingredientArray: GetRecipeResponseIngredient[]) {
  const sortedIngredients = copy(ingredientArray);
  sortedIngredients.sort((a, b) => (a.order || 0) - (b.order || 0));
  setOrderFromIndex(sortedIngredients);

  return sortedIngredients;
}

watch(
  () => [props.modelValue],
  () => {
    console.log('prop change', props.modelValue);

    const sortableIngredients = getSortableIngredients(props.modelValue);

    if (JSON.stringify(data.ingredients) !== JSON.stringify(sortableIngredients)) {
      data.ingredients = sortableIngredients;
    }
  }
);

watch(
  data,
  (newValue) => {
    console.log('data change', newValue.ingredients);
    const valueCopy = copy(newValue.ingredients);
    setOrderFromIndex(valueCopy);
    emit('update:modelValue', valueCopy);
  }
);
</script>

<template>
  <div>
    <!-- TODO: --bs-accordion-btn-icon should be white for dark mode -->
    <div class="accordion" id="ingredient-accordion">
      <Sortable :list="data.ingredients" item-key="order">
        <template #item="{ element, index }">
          <div class="accordion-item sortable-draggable">
            <h2 class="accordion-header" :id="`ingredient-${index}-accordion-header`">
              <button
                class="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                :data-bs-target="`#ingredient-${index}-accordion-collapse`"
                aria-expanded="false"
                :aria-controls="`ingredient-${index}-accordion-collapse`"
              >
                <font-awesome-icon icon="fa-sort" class="me-2" />
                <span v-if="element.isCategory" class="fw-bold">{{ element.name }}</span>
                <span v-else>{{ element.quantity }}x {{ element.name }}</span>
              </button>
            </h2>
            <div
              :id="`ingredient-${index}-accordion-collapse`"
              class="accordion-collapse collapse"
              :aria-labelledby="`ingredient-${index}-accordion-header`"
              data-bs-parent="#ingredient-accordion"
            >
              <div class="grid p-3" style="--bs-gap: 1em">
                <div class="g-col-12 g-col-md-12">
                  <label :for="`ingredient-${index}-name`" class="form-label">Name</label>
                  <input
                    :id="`ingredient-${index}-name`"
                    v-model="element.name"
                    required
                    type="text"
                    :class="{
                      'form-control': true,
                      'is-invalid': isFieldInError('ingredients'),
                    }"
                  />
                </div>
                <div v-if="!element.isCategory" class="g-col-12 g-col-md-4">
                  <label :for="`ingredient-${index}-quantity`" class="form-label">Quantity</label>
                  <input
                    :id="`ingredient-${index}-quantity`"
                    v-model="element.quantity"
                    required
                    type="number"
                    min="1"
                    :class="{
                      'form-control': true,
                      'is-invalid': isFieldInError('ingredients'),
                    }"
                  />
                </div>
                <div class="g-col-12 g-col-md-4">
                  <label :for="`ingredient-${index}-order`" class="form-label">Order</label>
                  <input
                    :id="`ingredient-${index}-order`"
                    v-model="element.order"
                    required
                    type="number"
                    min="1"
                    :class="{
                      'form-control': true,
                      'is-invalid': isFieldInError('ingredients'),
                    }"
                  />
                </div>
                <div class="g-col-12">
                  <div class="form-check">
                    <input
                      :id="`ingredient-${index}-isCategory`"
                      v-model="element.isCategory"
                      class="form-check-input"
                      type="checkbox"
                      :class="{ 'is-invalid': isFieldInError('ingredients') }"
                    />
                    <label :for="`ingredient-${index}-isCategory`" class="form-check-label"
                      >Is Category</label
                    >
                  </div>
                </div>
                <div class="btn-toolbar g-col-12">
                  <button
                    class="btn btn-danger btn-sm d-inline ms-auto"
                    @click.stop.prevent="deleteClick(index)"
                  >
                    Delete
                  </button>
                </div>
              </div>
            </div>
          </div>
        </template>
      </Sortable>
    </div>
    <div class="btn-toolbar">
      <button class="btn btn-secondary btn-sm me-2 mt-3" @click.stop.prevent="newClick()">
        New ingredient
      </button>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.accordion-button {
  padding: 0.75em;
}

.accordion .sortable-draggable {
  &,
  .fa-sort {
    cursor: grab;
  }
}
</style>
