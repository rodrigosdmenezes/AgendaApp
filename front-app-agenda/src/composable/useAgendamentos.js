import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useAuthStore } from '@/store/authStore'

export function useAgendamento() {
  const authStore = useAuthStore()

  const medicos = ref([])
  const medicoSelecionado = ref('')
  const horarios = ref([])
  const horarioSelecionado = ref('')
  const nomePaciente = ref('')

  const buscarMedicos = async () => {
    try {
      const response = await axios.get('http://localhost:5074/api/main/medicos/disponiveis', {
        headers: {
          Authorization: `Bearer ${authStore.token}`
        }
      })
      medicos.value = response.data
    } catch (error) {
      console.error('Erro ao buscar médicos:', error)
    }
  }

  const formatarHorario = (dataString) => {
    const data = new Date(dataString)
    if (isNaN(data)) return "Data inválida"
    return `${data.toLocaleDateString('pt-BR')} ${data.toLocaleTimeString('pt-BR', {
      hour: '2-digit',
      minute: '2-digit'
    })}`
  }

  const buscarHorarios = () => {
    if (!medicoSelecionado.value) return
    const medico = medicos.value.find(m => m.medicoId === medicoSelecionado.value)

    if (medico) {
      horarios.value = medico.horariosDisponiveis.map(h => {
        const dataStr = typeof h === 'string' ? h : h.dataHora
        const data = new Date(dataStr)
        if (isNaN(data)) return "Data inválida"
        return `${data.toLocaleDateString('pt-BR')} ${data.toLocaleTimeString('pt-BR', {
          hour: '2-digit',
          minute: '2-digit'
        })}`
      })
    } else {
      horarios.value = []
    }
  }

  const agendarConsulta = async () => {
    const body = {
      medicoId: medicoSelecionado.value,
      horario: horarioSelecionado.value,
      pacienteNome: nomePaciente.value
    }

    try {
      await axios.post('http://localhost:5074/api/main/consulta', body, {
        headers: {
          Authorization: `Bearer ${authStore.token}`
        }
      })
      alert('Consulta agendada com sucesso!')
    } catch (error) {
      console.error('Erro ao agendar consulta:', error)
    }
  }

  onMounted(buscarMedicos)

  return {
    medicos,
    medicoSelecionado,
    horarios,
    horarioSelecionado,
    nomePaciente,
    buscarHorarios,
    agendarConsulta,
    formatarHorario
  }
}
