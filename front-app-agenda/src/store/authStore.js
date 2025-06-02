import { defineStore } from 'pinia'
import axios from 'axios'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    usuario: null,
    tipoUsuario: null,
      token: null
  }),
  actions: {
    async login({ email, senha, tipo }) {
      try {
        const response = await axios.post('http://localhost:5074/api/main/login', {
          email,
          senha,
          tipoUsuario: tipo  // <-- aqui é o que o backend espera
        })

        this.usuario = response.data.usuario  // ou o que retornar seu backend
        this.tipoUsuario = tipo
        this.token = response.data.token
      } catch (error) {
        console.error('Erro no login:', error)
        throw new Error('Falha ao fazer login. Verifique suas credenciais.')
      }
    }
  }
})
