<template>
    <div class="cadastro-container">
      <h2>Cadastro de Médico</h2>
      <form @submit.prevent="cadastrar">
        <div>
          <label for="nome">Nome</label>
          <input v-model="form.nome" id="nome" type="text" required />
        </div>

        <div>
          <label for="crm">Crm</label>
          <input v-model="form.crm" id="crm" type="text" required />
        </div>
        
        <div>
          <label for="cpf">CPF</label>
          <input v-model="form.cpf" id="cpf" type="text" required />
        </div>
  
        <div>
          <label for="especialidade">Especialidade</label>
          <input v-model="form.especialidade" id="especialidade" type="text" required />
        </div>
  
        <div>
          <label for="email">Email</label>
          <input v-model="form.email" id="email" type="email" required />
        </div>
  
        <div>
          <label for="senha">Senha</label>
          <input v-model="form.senha" id="senha" type="password" required />
        </div>
  
        <button type="submit">Cadastrar</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { reactive } from 'vue'
  import axios from 'axios'
  
  const form = reactive({
    nome: '',
    cpf: '',
    especialidade: '',
    email: '',
    senha: ''
  })
  
  const cadastrar = async () => {
    try {
      await axios.post('http://localhost:5074/api/main/medicos', form)
      alert('Cadastro realizado com sucesso!')
      // Redirecionar para login se quiser
    } catch (error) {
      alert('Erro ao cadastrar médico')
      console.error(error)
    }
  }
  </script>
  
  <style scoped>
  .cadastro-container {
    max-width: 400px;
    margin: 0 auto;
    padding: 24px;
    background-color: white;
    border-radius: 12px;
    box-shadow: 0 0 8px rgba(0,0,0,0.1);
  }
  form div {
    margin-bottom: 12px;
  }
  label {
    display: block;
    font-weight: bold;
    margin-bottom: 4px;
  }
  input {
    width: 100%;
    padding: 8px;
    border-radius: 6px;
    border: 1px solid #ccc;
  }
  button {
    background-color: #40E0D0; /* Verde Tifanny */
    color: white;
    padding: 10px;
    width: 100%;
    border: none;
    border-radius: 6px;
    font-weight: bold;
    cursor: pointer;
  }
  button:hover {
    background-color: #2fc9bc;
  }
  </style>
  