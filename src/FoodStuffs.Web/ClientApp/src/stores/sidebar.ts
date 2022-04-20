import { defineStore } from 'pinia';

interface SidebarStoreState {
  sidebarVisible: boolean | null;
}

export const useSidebarStore = defineStore('sidebar', {
  state: (): SidebarStoreState => ({
    sidebarVisible: null,
  }),

  getters: {},

  actions: {
    setSidebarVisible(value: boolean) {
      this.sidebarVisible = value;
    },
  },
});

export default useSidebarStore;
