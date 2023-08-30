import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig(({ command, mode }) => {
  return {
    plugins: [vue()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
    build: {
      rollupOptions: {
        input: {
          app: './app.html',
        },
      },
      outDir: '../HomeSensors.Web/wwwroot',
      emptyOutDir: true,
      sourcemap: mode === 'development',
    },
    watch: {
      include: './src/**',
    },
  };
});
