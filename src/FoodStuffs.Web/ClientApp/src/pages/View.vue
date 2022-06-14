<script lang="ts" setup>
import { reactive } from 'vue';
import GetRecipeResponse from '@/models/api/recipes/GetRecipeResponse';
import SelectSidebar from '@/viewComponents/SelectSidebar.vue';
import RecipeViewer from '@/viewComponents/RecipeViewer.vue';

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
});

const sourceRecipe = reactive(new GetRecipeResponse());

//   watch: {
//     id() {
//       this.fetchRecipe(this.id);
//     },
//   },
//   created() {
//     this.fetchRecipe(this.id);
//   },
//   methods: {
//     ...mapActions({
//       setApiFailureMessages: 'app/setApiFailureMessages',
//       addToRecent: 'recipes/addToRecent',
//     }),
//     fetchRecipe(id) {
//       webApi.recipes.get(
//         id,
//         (data) => {
//           this.sourceRecipe = data;
//         },
//         (response) => this.setApiFailureMessages(response)
//       );
//     },
//   },
//   beforeRouteUpdate(to, from, next) {
//     this.addToRecent(this.sourceRecipe);
//     next();
//   },
//   beforeRouteLeave(to, from, next) {
//     this.addToRecent(this.sourceRecipe);
//     next();
//   },
// };
</script>

<template>
  <b-container>
    <b-row>
      <b-col md="12" lg="3" class="no-print mt-4">
        <SelectSidebar :route-name="'view'" />
      </b-col>
      <b-col class="mt-4">
        <h1>{{ sourceRecipe.name }}</h1>
        <RecipeViewer class="mt-4" :recipe="sourceRecipe" />
      </b-col>
    </b-row>
  </b-container>
</template>

<style lang="scss" scoped></style>
