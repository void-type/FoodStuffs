import { defineStore } from 'pinia';

interface SidebarStoreState {
  isSidebarVisibleSetting: boolean | null;
}

export const useSidebarStore = defineStore('sidebar', {
  state: (): SidebarStoreState => ({
    isSidebarVisibleSetting: null,
  }),

  getters: {},

  actions: {
    setSidebarVisibleSetting(value: boolean) {
      this.isSidebarVisibleSetting = value;
    },
  },
});

export default useSidebarStore;
