<script lang="ts" setup>
import useShoppingItemStore from '@/stores/shoppingItemStore';
import { storeToRefs } from 'pinia';
import { watch } from 'vue';
import ApiHelpers from '@/models/ApiHelpers';
import useMessageStore from '@/stores/messageStore';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: 0,
  },
});

const messageStore = useMessageStore();
const shoppingItemStore = useShoppingItemStore();
const api = ApiHelpers.client;

const { listRequest } = storeToRefs(shoppingItemStore);

function fetchList() {
  api()
    .shoppingItemsList(listRequest.value)
    .then((response) => {
      shoppingItemStore.listResponse = response.data;
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
}

watch(
  props,
  () => {
    fetchList();
  },
  { immediate: true }
);
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <div class="alert alert-warning mt-3">
      <strong>Warning:</strong> This page is a temporary placeholder until we get shopping item
      editing finished.
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
