import { defineStore } from 'pinia'
import axios from 'axios'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    usuario: null,
    tipoUsuario: null
  }),
  actions: {
    async login({ email, senha, tipo }) {
      try {
        const response = await axios.post('http://localhost:5000/api/login', {
          email,
          senha,
          tipo
        })

        this.usuario = response.data.usuario
        this.tipoUsuario = tipo
      } catch (error) {
        console.error('Erro no login:', error)
        throw new Error('Falha ao fazer login. Verifique suas credenciais.')
      }
    }
  }
})
