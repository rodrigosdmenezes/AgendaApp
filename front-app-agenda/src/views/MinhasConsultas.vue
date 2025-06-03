<template>
    <div class="container">
        <div class="consultas-agendadas-box">
            <h1>Minhas Consultas</h1>
            <ul v-if="consultas.length > 0">
                <li v-for="(consulta, index) in consultas" :key="index">
                    <strong>Médico:</strong> {{ consulta.nomeMedico }} <br />
                    <strong>Data:</strong> {{ formatarData(consulta.dataHora) }}
                </li>
            </ul>
            <p v-else>Nenhuma consulta agendada.</p>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const consultas = ref([])

const formatarData = (dataString) => {
    const data = new Date(dataString)
    return data.toLocaleDateString('pt-BR') + ' ' + data.toLocaleTimeString('pt-BR', {
        hour: '2-digit',
        minute: '2-digit',
    })
}

onMounted(async () => {
    try {
        const response = await axios.get('http://localhost:PORTA/api/consulta/paciente/${pacienteId}')
        consultas.value = response.data
    } catch (error) {
        console.error('Erro ao buscar consultas:', error)
    }
})
</script>

<style scoped>
.container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    font-family: 'Poppins', sans-serif;
}

.consultas-agendadas-box {
    display: flex;
    flex-direction: column;
    gap: 16px;
    padding: 2rem;
    border: 1px solid #ccc;
    border-radius: 12px;
    width: 300px;
}

h1 {
    margin-bottom: 1.5rem;
    color: #333;
}

ul {
    list-style: none;
    padding: 0;
    width: 100%;
    max-width: 400px;
}

li {
    background-color: #fff;
    padding: 1rem;
    margin-bottom: 1rem;
    border: 1px solid #ccc;
    border-radius: 12px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

li strong {
    color: #40e0d0;
}

p {
    font-size: 1rem;
    color: #666;
}
</style>