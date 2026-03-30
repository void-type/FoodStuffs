import { defineStore } from 'pinia';
import { clamp } from '@/models/FormatHelper';

interface ImageLightboxState {
  isActive: boolean;
  images: string[];
  currentIndex: number;
}

export default defineStore('imageLightbox', {
  state: (): ImageLightboxState => ({
    isActive: false,
    images: [],
    currentIndex: 0,
  }),

  actions: {
    open(images: string[], startIndex = 0) {
      if (images.length === 0) {
        return;
      }

      this.images = images;
      this.currentIndex = clamp(startIndex, 0, images.length - 1);
      this.isActive = true;
    },

    close() {
      this.isActive = false;
      this.images = [];
      this.currentIndex = 0;
    },

    next() {
      if (this.currentIndex < this.images.length - 1) {
        this.currentIndex++;
      }
    },

    prev() {
      if (this.currentIndex > 0) {
        this.currentIndex--;
      }
    },
  },
});
