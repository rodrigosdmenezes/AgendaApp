<template>
  <div class="login-container">
    <div class="login-box">
      <h1>Login</h1>

      <select v-model="tipoUsuario">
        <option value="paciente">Paciente</option>
        <option value="medico">Médico</option>
      </select>

      <input type="email" v-model="email" placeholder="Email" />
      <input type="password" v-model="senha" placeholder="Senha" />

      <button @click="login">Entrar</button>
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

    // Redireciona baseado no tipo de usuário
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
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: white;
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
  background-color: #40e0d0; /* Verde Tiffany */
  color: white;
  padding: 10px;
  border: none;
  cursor: pointer;
  font-weight: bold;
}
</style>
