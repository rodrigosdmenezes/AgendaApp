<template>
    <div class="container">
        <div class="disponibilizar-container-box">
            <h1>Disponibilizar Horários</h1>

            <form @submit.prevent="adicionarHorario">
                <div class="form-group">
                    <label for="data">Data</label>
                    <input type="date" id="data" v-model="dataSelecionada" required />
                </div>

                <div class="form-group">
                    <label for="hora">Horário</label>
                    <input type="time" id="hora" v-model="horaSelecionada" required />
                </div>

                <button type="submit">Adicionar Horário</button>
            </form>

            <h2>Horários Disponíveis</h2>
            <ul>
                <li v-for="(horario, index) in horariosDisponiveis" :key="index">
                    {{ formatarData(horario.data) }} - {{ horario.hora }}
                    <button @click="removerHorario(index)">Remover</button>
                </li>
            </ul>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'

const dataSelecionada = ref('')
const horaSelecionada = ref('')

const horariosDisponiveis = ref([])

function formatarData(dataISO) {
    if (!dataISO) return ''
    const [ano, mes, dia] = dataISO.split('-')
    return `${dia}/${mes}/${ano}`
}

function adicionarHorario() {
    if (!dataSelecionada.value || !horaSelecionada.value) {
        alert('Selecione data e horário')
        return
    }

    // Montar objeto para salvar
    const novoHorario = {
        data: dataSelecionada.value,
        hora: horaSelecionada.value
    }

    // Checar se já existe
    const existe = horariosDisponiveis.value.some(h => h.data === novoHorario.data && h.hora === novoHorario.hora)
    if (existe) {
        alert('Horário já disponível!')
        return
    }

    horariosDisponiveis.value.push(novoHorario)

    // Limpar inputs
    dataSelecionada.value = ''
    horaSelecionada.value = ''
}

function removerHorario(index) {
    horariosDisponiveis.value.splice(index, 1)
}
</script>

<style scoped>
.container {
    display: flex;
    font-family: 'Poppins', sans-serif;
    justify-content: center;
    align-items: center;
    height: 100vh;
}

.disponibilizar-container-box {
    display: flex;
    flex-direction: column;
    gap: 16px;
    padding: 2rem;
    border: 1px solid #ccc;
    border-radius: 12px;
    width: 300px;
}

form {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

button {
    background-color: #40e0d0;
    color: white;
    border: none;
    padding: 10px;
    cursor: pointer;
    font-weight: bold;
}

ul {
    margin-top: 20px;
    padding-left: 0;
    list-style: none;
}

li {
    display: flex;
    justify-content: space-between;
    padding: 6px 0;
    border-bottom: 1px solid #ddd;
}

li button {
    background: #ff4d4f;
    padding: 4px 8px;
}
</style>