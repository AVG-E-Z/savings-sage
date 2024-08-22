import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import dotenv from 'dotenv';
dotenv.config();

// https://vitejs.dev/config/
export default defineConfig({
    server: {
        proxy: {
            '/api': {
                target: process.env.NODE_ENV !== 'production'
                    ? process.env.DEVELOPMENT_BACKEND_URL
                    : process.env.DEPLOYMENT_BACKEND_URL  ? process.env.DEPLOYMENT_BACKEND_URL : process.env.PROXY_API ,
                changeOrigin: true,
            },
        },
    },
  plugins: [react()],
})
