# TaskFlow

API REST para gerenciamento de projetos e tarefas, desenvolvida em .NET 8 com arquitetura limpa, CQRS e autenticação JWT.

## Objetivo

O projeto tem como objetivo fornecer uma solução backend para controlar tarefas dentro de projetos, com organização por usuários, projetos e tarefas, seguindo boas práticas de design e desenvolvimento.

Principais funcionalidades:

- Cadastro e login de usuários
- Autenticação via JWT
- Criação e listagem de projetos
- Criação, atualização, consulta e exclusão de tarefas
- Filtros por status e prioridade
- Validação de entrada com FluentValidation
- Tratamento centralizado de exceções
- Swagger para documentação da API
- Testes automatizados para regras de negócio

## Arquitetura

A solução foi organizada em camadas para manter o projeto fácil de evoluir e testar:

- Domain: entidades e regras do domínio
- Application: casos de uso, commands, queries, DTOs e validação
- Infrastructure: acesso a dados, autenticação, hashing e integrações externas
- API: exposição dos endpoints HTTP

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- MediatR
- FluentValidation
- AutoMapper
- JWT
- BCrypt.Net-Next
- Serilog
- Swagger
- xUnit

## Requisitos

Antes de executar o projeto, confirme que os itens abaixo estão instalados:

- .NET SDK 8.0
- Git
- (opcional) Docker

## Como rodar o projeto

1. Abra o terminal na raiz do projeto
2. Instale o SDK .NET 8, caso ainda não esteja disponível
3. Execute os comandos abaixo:

```bash
export PATH="$HOME/.dotnet:$PATH"
dotnet restore
dotnet build
dotnet test
```

Para iniciar a API:

```bash
export PATH="$HOME/.dotnet:$PATH"
dotnet run --project src/TaskFlow.Api/TaskFlow.Api.csproj --urls http://localhost:5000
```

A API ficará disponível em:

- http://localhost:5000/swagger/index.html
- http://localhost:5000/swagger/v1/swagger.json

## Endpoints principais

### Autenticação

- POST /api/Auth/register
- POST /api/Auth/login

### Projetos

- GET /api/Projects
- POST /api/Projects

### Tarefas

- GET /api/Tasks
- GET /api/Tasks/{id}
- POST /api/Tasks
- PUT /api/Tasks/{id}
- DELETE /api/Tasks/{id}
- PATCH /api/Tasks/{id}/complete

## Testes

Os testes podem ser executados com:

```bash
dotnet test
```

## Observações

- O projeto usa SQLite para persistência local durante o desenvolvimento.
- Em produção, o segredo JWT deve ser substituído por um valor forte e seguro.
- A configuração da API fica em `src/TaskFlow.Api/appsettings.json`.

## Status

O projeto está funcional, com build concluída e testes aprovados em execução local validada.
