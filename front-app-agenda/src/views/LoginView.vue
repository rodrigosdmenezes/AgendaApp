<template>
  <div class="login-container">
    <div class="login-box">
      <h1>AppAgenda</h1>
      <select v-model="tipoUsuario">
        <option value="paciente">Paciente</option>
        <option value="medico">Médico</option>
      </select>

      <input type="email" v-model="email" placeholder="Email" />
      <input type="password" v-model="senha" placeholder="Senha" />

      <button @click="login">Entrar</button>

      <p class="cadastro-msg">
        Não tem login? Se cadastre como
        <a href="#" @click.prevent="irCadastro('paciente')">Paciente</a> ou
        <a href="#" @click.prevent="irCadastro('medico')">Médico</a>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/store/authStore'

const router = useRouter()
const tipoUsuario = ref('paciente')
const email = ref('')
const senha = ref('')
const authStore = useAuthStore()

const login = async () => {
  try {
    await authStore.login({
      email: email.value,
      senha: senha.value,
      tipo: tipoUsuario.value
    })

    if (tipoUsuario.value === 'paciente') {
      router.push('/dashboard-paciente')
    } else {
      router.push('/dashboard-medico')
    }

  } catch (error) {
    alert('Erro ao fazer login. Verifique seu e-mail e senha.')
    console.error(error)
  }
}

function irCadastro(tipo) {
  if (tipo === 'paciente') {
    router.push('/cadastro-paciente')
  } else if (tipo === 'medico') {
    router.push('/cadastro-medico')
  }
}
</script>

<style scoped>
h1 {
  text-align: center;
}

.login-container {
  display: flex;
  justify-content: center;
  align-items: center;     
  height: 100vh;   
}

.login-box {
  display: flex;
  flex-direction: column;
  gap: 16px;
  padding: 2rem;
  border: 1px solid #ccc;
  border-radius: 12px;
  width: 300px;
}

input,
select {
  padding: 10px;
  font-size: 1rem;
}

button {
  background-color: #40e0d0;
  color: white;
  padding: 10px;
  border: none;
  cursor: pointer;
  font-weight: bold;
}

.cadastro-msg {
  margin-top: 12px;
  font-size: 0.95rem;
  color: #444;
  font-family: 'Poppins', sans-serif;
  font-weight: 400;
}

.cadastro-msg a {
  color: #40e0d0;
  text-decoration: none;
  font-weight: 600;
  cursor: pointer;
  transition: color 0.3s ease;
}

.cadastro-msg a:hover {
  color: #2bb3a6;
  text-decoration: underline;
}
</style>
