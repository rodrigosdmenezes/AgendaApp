import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'
import CadastroPaciente from '@/views/CadastroPaciente.vue'
import CadastroMedico from '@/views/CadastroMedico.vue'
import AgendamentoConsulta from '@/views/AgendamentoConsulta.vue'
import DisponibilizarHorarios from '@/views/DisponibilizarHorarios.vue'
import MinhasConsultas from '@/views/MinhasConsultas.vue'
import ConsultasMedico from '@/views/ConsultasMedico.vue'

const routes = [
  { path: '/', component: LoginView },
  { path: '/cadastro-paciente', component: CadastroPaciente },
  { path: '/cadastro-medico', component: CadastroMedico },
  { path: '/agendamento', component: AgendamentoConsulta },
  { path: '/disponibilizar', component: DisponibilizarHorarios },
  { path: '/minhas-consultas', name: 'MinhasConsultas', component: MinhasConsultas },
  { path: '/consultas-agendadas', name: 'ConsultasAgendadas', component: ConsultasMedico }
  // outras rotas...
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
