import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'
import CadastroPaciente from '@/views/CadastroPaciente.vue'
import CadastroMedico from '@/views/CadastroMedico.vue'
// outras importações...

const routes = [
  { path: '/', component: LoginView },
  { path: '/cadastro-paciente', component: CadastroPaciente },
  { path: '/cadastro-medico', component: CadastroMedico },
  // outras rotas...
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
