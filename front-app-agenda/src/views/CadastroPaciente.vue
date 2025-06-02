<template>
    <div class="container">
      <h1>Cadastro de Paciente</h1>
      <form @submit.prevent="cadastrar">
        <div class="form-group">
          <label for="nome">Nome</label>
          <input type="text" id="nome" v-model="nome" required />
        </div>

        <div class="form-group">
          <label for="cpf">Cpf</label>
          <input type="text" id="cpf" v-model="nome" required />
        </div>
  
        <div class="form-group">
          <label for="email">Email</label>
          <input type="email" id="email" v-model="email" required />
        </div>
  
        <div class="form-group">
          <label for="senha">Senha</label>
          <input type="password" id="senha" v-model="senha" required />
        </div>

        <button type="submit">Cadastrar</button>
  
        <p class="login-link">
          Já tem uma conta? <router-link to="/login">Entrar</router-link>
        </p>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  
  const nome = ref('')
  const cpf = ref('')
  const email = ref('')
  const senha = ref('')
  const confirmarSenha = ref('')
  
  const cadastrar = () => {
    if (senha.value !== confirmarSenha.value) {
      alert('As senhas não coincidem')
      return
    }
  
    // Aqui você poderá fazer o post com axios para a API de cadastro
    console.log({ nome: nome.value, cpf: cpf.value, email: email.value, senha: senha.value })
  }
  </script>
  
  <style scoped>
  .container {
    max-width: 400px;
    margin: 80px auto;
    padding: 20px;
    background: white;
    border-radius: 8px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
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
    margin-bottom: 4px;
  }
  input {
    width: 100%;
    padding: 8px;
    box-sizing: border-box;
  }
  button {
    width: 100%;
    background-color: #40e0d0; /* Verde Tiffany */
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
  