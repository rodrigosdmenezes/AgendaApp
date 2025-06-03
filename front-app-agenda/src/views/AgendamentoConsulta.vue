<template>
    <div class="agendamento-container">
        <div class="agendamento-box">
            <h1>Agendar Consulta</h1>

            <!-- Seleção de médico -->
            <div class="form-group">
                <label for="medico">Selecione o Médico</label>
                <select id="medico" v-model="medicoSelecionado" @change="buscarHorarios">
                    <option value="" disabled>Escolha um médico</option>
                    <option v-for="medico in medicos" :key="medico.medicoId" :value="medico.medicoId">
                        {{ medico.nome }} - {{ medico.especialidade }}
                    </option>
                </select>
            </div>

            <!-- Seleção de horário -->
            <div v-if="horarios.length > 0" class="form-group">
                <label for="horario">Horários Disponíveis</label>
                <select id="horario" v-model="horarioSelecionado">
                    <option value="" disabled>Escolha um horário</option>
                    <option v-for="horario in horarios" :key="horario" :value="horario">
                        {{ formatarHorario(horario) }}
                    </option>
                </select>
            </div>

            <!-- Confirmação do nome -->
            <div v-if="horarioSelecionado" class="form-group">
                <label for="nomePaciente">Confirme seu nome completo</label>
                <input type="text" id="nomePaciente" v-model="nomePaciente" placeholder="Digite seu nome completo" />
            </div>

            <!-- Botão de agendamento -->
            <button @click="agendarConsulta" :disabled="!horarioSelecionado">Confirmar Consulta</button>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useAuthStore } from '@/store/authStore'  // caminho do seu store

const authStore = useAuthStore() // Aqui você usa o store normalmente

const medicos = ref([])
const medicoSelecionado = ref('')
const horarios = ref([])
const horarioSelecionado = ref('')
const nomePaciente = ref('')

onMounted(async () => {
    try {
        // Passando o token no header Authorization
        console.log('Token:', authStore.token)
        const response = await axios.get('http://localhost:5074/api/main/medicos/disponiveis', {
            headers: {
                Authorization: `Bearer ${authStore.token}`
            }
        })
        medicos.value = response.data
    } catch (error) {
        console.error('Erro ao buscar médicos:', error)
    }
})

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
        console.log('horariosDisponiveis:', medico.horariosDisponiveis)
        horarios.value = medico.horariosDisponiveis.map(h => {
            // Se for objeto, tenta usar h.dataHora, senão h direto
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
</script>

<style scoped>
.agendamento-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    font-family: 'Poppins', sans-serif;
}

.agendamento-box {
    display: flex;
    flex-direction: column;
    gap: 16px;
    padding: 2rem;
    border: 1px solid #ccc;
    border-radius: 12px;
    width: 300px;
}

.form-group {
    display: flex;
    flex-direction: column;
}

select,
button {
    padding: 10px;
    font-size: 1rem;
}

button {
    background-color: #40e0d0;
    color: white;
    border: none;
    cursor: pointer;
    font-weight: bold;
    border-radius: 8px;
}
</style>
