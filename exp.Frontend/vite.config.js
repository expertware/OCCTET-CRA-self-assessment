import { fileURLToPath, URL } from 'node:url'

import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'

const defaultConfig = {
  plugins: [vue()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
};
 
export default defineConfig(({ command, mode }) => {
  process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };
 
  if (command === "serve") {
    const isDev = mode === "development";
    return {
      ...defaultConfig,
      server: {
        proxy: {
          "/api": {
            target: isDev ? "https://localhost:7260/" : "address",
            changeOrigin: isDev,
            secure: !isDev,
          },
        },
      },
    };
  } else {
    return defaultConfig;
  }
});




