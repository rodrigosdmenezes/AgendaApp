# Documentação do Projeto - AgendaApp

## Tecnologias e Ferramentas Utilizadas

### Backend:

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
