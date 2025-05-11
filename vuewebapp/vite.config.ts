import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 57006,
    proxy: {
      '/api/catalog': {
        target: 'http://catalog', // Update with the local Catalog backend URL
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api\/catalog/, ''), // Optional rewrite
      },
    },
  },
});
