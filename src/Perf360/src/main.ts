import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { useAuthStore } from './stores/auth'
import auth from './plugins/auth'

const app = createApp(App)

app.use(createPinia())

const authStore = useAuthStore()
authStore.getCurrentUserInformation().then(() => {
  app.use(router)
  app.use(auth)
  app.mount('#app')
})
