import { createApp } from 'vue'
import App from './App.vue'

import { createPinia } from 'pinia'  // importar Pinia

const app = createApp(App)

const pinia = createPinia()  // criar instância do Pinia

app.use(pinia)  // registrar Pinia na app

app.mount('#app')
