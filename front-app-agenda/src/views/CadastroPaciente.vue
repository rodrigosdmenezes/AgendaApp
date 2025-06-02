<template>
  <div class="container">
    <div class="cadastro-container-box">
      <h1>Cadastro de Paciente</h1>
      <form @submit.prevent="cadastrar">
        <div class="form-group">
          <label for="nome">Nome</label>
          <input type="text" id="nome" v-model="nome" required />
        </div>

        <div class="form-group">
          <label for="cpf">CPF</label>
          <input type="text" id="cpf" v-model="cpf" required />
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <input type="email" id="email" v-model="email" required />
        </div>

        <div class="form-group">
          <label for="senha">Senha</label>
          <input type="password" id="senha" v-model="senha" required />
        </div>

        <button @click="cadastrar">Cadastrar</button>

        <p class="login-link">
          Já tem uma conta? <router-link to="/login">Entrar</router-link>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const nome = ref('')
const cpf = ref('')
const email = ref('')
const senha = ref('')

const cadastrar = () => {
  alert(`teste: ${nome.value}`)
  console.log({ nome: nome.value, cpf: cpf.value, email: email.value, senha: senha.value })
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

.cadastro-container-box {
  display: flex;
  flex-direction: column;
  gap: 16px;
  padding: 2rem;
  border: 1px solid #ccc;
  border-radius: 12px;
  width: 300px;
}

h1 {
  text-align: center;
  margin-bottom: 24px;
}

.form-group {
  margin-bottom: 16px;
}

label {
  display: block;
  font-weight: bold;
  margin-bottom: 4px;
}

input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
}

button {
  width: 100%;
  background-color: #40e0d0;
  /* Verde Tiffany */
  color: white;
  border: none;
  padding: 10px;
  font-weight: bold;
  cursor: pointer;
  border-radius: 4px;
}

.login-link {
  margin-top: 16px;
  text-align: center;
}
</style>