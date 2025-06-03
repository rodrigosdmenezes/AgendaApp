# Documentação do Projeto - AgendaApp

## Passo a Passo para Utilizar o Sistema - AgendaApp
Para Pacientes:
1. Cadastro
Acesse a página inicial do sistema.

Clique em "Cadastrar Paciente".

Preencha seus dados:

Nome completo,
CPF,
E-mail,
Senha,
Clique em "Cadastrar".

2. Login
Vá até a página de Login.
Informe o e-mail e a senha cadastrados.
Clique em "Entrar".

3. Agendamento de Consulta
Após o login, vá para a página de Agendar Consulta.
Selecione o médico desejado.
Selecione uma data e horário disponível.
Digite seu nome completo para confirmação.
Clique em "Confirmar Consulta".

4. Ver Minhas Consultas
Após agendar uma consulta, clique em "Ver minhas consultas".
A página exibirá:
Nome do médico
Data e horário da consulta

Para Médicos:
1. Cadastro
Acesse a página inicial do sistema.
Clique em "Cadastrar Médico".
Preencha os dados:
Nome completo,
CRM,
CPF,
E-mail,
Senha,
Clique em "Cadastrar".

2. Login
Vá até a página de Login.
Informe o e-mail e senha cadastrados.
Clique em "Entrar".

3. Disponibilizar Horários
Após o login, vá até a página Disponibilizar Horários.
Selecione uma data e insira um horário.
Clique em "Adicionar Horário".
Repita quantas vezes quiser.

4. Ver Consultas Agendadas
Acesse a página de Consultas Agendadas.
Nela serão listadas:
Nome dos pacientes
Datas e horários das consultas marcadas com você

# Documentação do Frontend:

## Tecnologias Utilizadas

* Vue 3
* Vue Router
* Pinia (gerenciamento de estado)
* Axios (requisições HTTP)
* Composition API

---

## Instalação e Execução Local

### Requisitos

* Node.js (versão 18 ou superior)
* Gerenciador de pacotes (npm ou yarn)

### Passos

```bash
cd agendaapp-frontend
npm install
npm run dev
```

A aplicação estará disponível em: [http://localhost:8081/]

---

## Estrutura de Diretórios

```
src/
|-- assets/         # Imagens, fontes, etc.
|-- components/     # Componentes reutilizáveis
|-- pages/          # Telas principais (Login, Cadastro, etc.)
|-- services/       # Requisições HTTP via Axios
|-- stores/         # Estados globais (Pinia)
|-- router/         # Configuração de rotas
|-- App.vue         # Componente raiz
|-- main.js         # Arquivo principal
```

---

## Configuração de Rotas (`router/index.js`)

```js
const routes = [
  { path: '/', component: LoginPage },
  { path: '/cadastro-paciente', component: CadastroPacientePage },
  { path: '/cadastro-medico', component: CadastroMedicoPage },
  { path: '/agendamento', component: AgendamentoConsultaPage },
  { path: '/minhas-consultas', component: MinhasConsultasPage },
  { path: '/consultas-medico', component: ConsultasMedicoPage },
]
```

---

## Serviço de Autenticação (`services/authService.js`)

```js
import axios from 'axios'

const API = 'http://localhost:5074/api/main/login'

export const login = (email, senha) => {
  return axios.post(`${API}/login`, { email, senha })
}

export const registrarPaciente = (dados) => {
  return axios.post(`${API}/register/paciente`, dados)
}

export const registrarMedico = (dados) => {
  return axios.post(`${API}/register/medico`, dados)
}
```

---
## Estado Global com Pinia (`stores/userStore.js`)

```js
import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
  state: () => ({
    token: localStorage.getItem('token') || '',
    usuario: null,
  }),
  actions: {
    setToken(token) {
      this.token = token
      localStorage.setItem('token', token)
    },
    setUsuario(usuario) {
      this.usuario = usuario
    },
    logout() {
      this.token = ''
      this.usuario = null
      localStorage.removeItem('token')
    }
  }
})
```

---

## Telas Disponíveis

### Login

* Formulário com campos de e-mail e senha.
* Armazena o token no Pinia e `localStorage`.

### Cadastro de Paciente

* Formulário com nome, CPF, e-mail e senha.
* Requisições para `register/paciente`

### Cadastro de Medico

* Formulário com nome, CPF, CRM, especialidade, e-mail e senha.
* Requisições para `register/medico`

### Agendamento de Consulta

* Seleciona o médico
* Seleciona data e hora
* Confirma nome do paciente
* Chamada para `criarConsulta`

### Minhas Consultas (Paciente)

* Lista de consultas agendadas com médico e data
* Chamada para `listarConsultasPaciente`

### Consultas Agendadas (Médico)

* Lista de pacientes que marcaram com o médico
* Chamada para `listarConsultasMedico`

---

## Observações

* Todas as rotas protegidas requerem token JWT.
* O token é armazenado localmente e enviado via header `Authorization`.
* A aplicação segue padrões da Composition API e boas práticas de separação de responsabilidades.

### Documentação do Frontend Backend:

## Tecnologias e Ferramentas Utilizadas

* .NET 8.0
* ASP.NET Core Web API
* Entity Framework Core 8.0
* PostgreSQL

## Pacotes NuGet Utilizados

| Pacote                                        | Versão | Descrição                                         |
| --------------------------------------------- | ------ | ------------------------------------------------- |
| Microsoft.EntityFrameworkCore                 | 8.0.0  | ORM para manipulação do banco de dados relacional |
| Microsoft.EntityFrameworkCore.Design          | 8.0.0  | Suporte a scaffolding e migrations do EF Core     |
| Microsoft.EntityFrameworkCore.Tools           | 8.0.0  | Comandos EF para CLI (`dotnet ef migrations`)     |
| Npgsql.EntityFrameworkCore.PostgreSQL         | 8.0.0  | Provider para PostgreSQL com EF Core              |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.0  | Middleware de autenticação JWT                    |
| System.IdentityModel.Tokens.Jwt               | 6.34.0 | Manipulação e validação de tokens JWT             |
| Microsoft.Extensions.DependencyInjection      | 8.0.0  | Injeção de dependência de serviços e interfaces   |

## Banco de Dados

### Configuração Local do PostgreSQL

1. Instale o PostgreSQL (versão recomendada: 15+).
2. Crie um banco de dados com o nome desejado, por exemplo: `agenda_app_db`.
3. Configure a string de conexão no arquivo `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=AgendaDb;Username=postgres;Password=123456"
}
```

## Como Rodar o Projeto Localmente

### Pré-requisitos

* .NET SDK 8.0
* PostgreSQL 15+
* Ferramenta como Postman ou Thunder Client

### Passos

```bash
git clone https://github.com/seuusuario/AgendaApp.git
cd AgendaApp
dotnet restore
dotnet ef database update
dotnet run
```

A API estará disponível por padrão em:

```
https://localhost:5001 ou http://localhost:5000
```

## Testar Autenticação com JWT (Postman)

1. Registro:

   * `POST /api/auth/register/paciente`
   * `POST /api/auth/register/medico`

2. Login:

   * `POST /api/auth/login`
   * Retorna um token JWT.

3. Adicionar o token nas requisições protegidas:

```
Authorization: Bearer {seu_token_aqui}
```

## Endpoints Implementados

### Autenticação

* POST /api/auth/register/paciente
* POST /api/auth/register/medico
* POST /api/auth/login

### Consultas

* POST /api/consulta/criar – Cadastrar consulta
* POST /api/consulta/cancelar – Cancelar consulta
* GET /api/consulta/listarconsultas – Listar todas as consultas

Todos os endpoints acima estão protegidos por autenticação JWT.

## Exemplos de Testes com Postman

### Registro de Paciente

* URL: POST /api/auth/register/paciente

```json
{
  "nome": "João da Silva",
  "email": "joao@email.com",
  "senha": "123456"
}
```

### Registro de Médico

* URL: POST /api/auth/register/medico

```json
{
  "nome": "Dra. Ana Souza",
  "email": "ana@medico.com",
  "senha": "123456"
}
```

### Login

* URL: POST /api/auth/login

```json
{
  "email": "joao@email.com",
  "senha": "123456"
}
```

Resposta esperada:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

### Criar Consulta

* URL: POST /api/consulta/criar
* Headers:

  * Authorization: Bearer {token\_aqui}
  * Content-Type: application/json
* Body:

```json
{
  "medicoId": "GUID-do-medico",
  "pacienteId": "GUID-do-paciente",
  "dataHora": "2025-06-10T14:00:00"
}
```

### Cancelar Consulta

* URL: POST /api/consulta/cancelar
* Headers:

  * Authorization: Bearer {token\_aqui}
  * Content-Type: application/json
* Body:

```json
{
  "consultaId": "GUID-da-consulta"
}
```

### Listar Consultas

* URL: GET /api/consulta/listarconsultas
* Headers:

  * Authorization: Bearer {token\_aqui}
