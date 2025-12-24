import { resolve, dirname } from 'path'
import { fileURLToPath } from 'url'

const __dirname = dirname(fileURLToPath(import.meta.url))

export default {
  root: resolve(__dirname, 'wwwroot'),
  server: {
    fs: {
      // Allow serving files from project root and up
      allow: [resolve(__dirname, '..', '..')]
    }
  }
}