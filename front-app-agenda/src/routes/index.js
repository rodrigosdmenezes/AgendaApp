import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'
import CadastroPaciente from '@/views/CadastroPaciente.vue'
import CadastroMedico from '@/views/CadastroMedico.vue'
import AgendamentoConsulta from '@/views/AgendamentoConsulta.vue'
import DisponibilizarHorarios from '@/views/DisponibilizarHorarios.vue'

const routes = [
  { path: '/', component: LoginView },
  { path: '/cadastro-paciente', component: CadastroPaciente },
  { path: '/cadastro-medico', component: CadastroMedico },
  { path: '/agendamento', component: AgendamentoConsulta },
  { path: '/disponibilizar', component: DisponibilizarHorarios },
  // outras rotas...
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
