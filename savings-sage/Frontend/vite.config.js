import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
    server: {
        proxy: {
            '/api': {
                target: 'https://backend-savings-sage-latest.onrender.com',
                changeOrigin: true,
                //rewrite: (path) => path.replace(/^\/api/, ''),
            },
        },
    },
  plugins: [react()],
})
