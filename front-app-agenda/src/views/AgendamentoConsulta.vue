<template>
    <div class="agendamento-container">
        <div class="agendamento-box">
            <h1>Agendar Consulta</h1>
            <!-- Seleção de médico -->
            <div class="form-group">
                <label for="medico">Selecione o Médico</label>
                <select id="medico" v-model="medicoSelecionado" @change="buscarHorarios">
                    <option value="" disabled>Escolha um médico</option>
                    <option v-for="medico in medicos" :key="medico.id" :value="medico.id">
                        {{ medico.nome }}
                    </option>
                </select>
            </div>

            <!-- Seleção de horário -->
            <div v-if="horarios.length > 0" class="form-group">
                <label for="horario">Horários Disponíveis</label>
                <select id="horario" v-model="horarioSelecionado">
                    <option value="" disabled>Escolha um horário</option>
                    <option v-for="horario in horarios" :key="horario">
                        {{ horario }}
                    </option>
                </select>
            </div>
            <!-- Botão de agendamento -->
            <button @click="agendarConsulta" :disabled="!horarioSelecionado">Confirmar Consulta</button>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const medicos = ref([])
const medicoSelecionado = ref('')
const horarios = ref([])
const horarioSelecionado = ref('')

onMounted(async () => {
    // Buscar lista de médicos
    const response = await axios.get('http://localhost:PORTA/api/medicos')
    medicos.value = response.data
})

const buscarHorarios = async () => {
    if (!medicoSelecionado.value) return
    const response = await axios.get(`http://localhost:PORTA/api/consultas/disponiveis/${medicoSelecionado.value}`)
    horarios.value = response.data
}

const agendarConsulta = async () => {
    const body = {
        medicoId: medicoSelecionado.value,
        horario: horarioSelecionado.value,
        pacienteNome: 'NOME_DO_PACIENTE_LOGADO' // você pode pegar do store/autenticação
    }

    await axios.post('http://localhost:PORTA/api/consultas', body)
    alert('Consulta agendada com sucesso!')
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