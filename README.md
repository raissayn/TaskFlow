# TaskFlow API

API REST para gestão de projetos e tarefas, desenvolvida em .NET 8 com foco em organização, arquitetura limpa e boas práticas de desenvolvimento.

O objetivo do projeto é demonstrar uma aplicação realista de backend com autenticação, regras de negócio, persistência, validação e testes automatizados, seguindo padrões como Clean Architecture e CQRS.

---

## Objetivo do projeto

Este projeto foi pensado como uma solução backend para acompanhar tarefas de projetos em equipe, com fluxo simples e organizado:

- usuários podem se registrar e fazer login;
- cada usuário pode possuir projetos;
- cada projeto pode ter várias tarefas;
- tarefas podem ser filtradas por status e prioridade;
- a aplicação usa autenticação JWT para proteger os endpoints de negócio;
- a estrutura do código foi organizada para facilitar manutenção, evolução e testes.

Em outras palavras, o projeto serve como base para uma API de gerenciamento de tarefas com uma arquitetura que escala bem e é fácil de evoluir.

---

## Stack e tecnologias

- .NET 8
- ASP.NET Core Web API
- Clean Architecture
- CQRS com MediatR
- Entity Framework Core + SQLite
- FluentValidation
- AutoMapper
- JWT Bearer Authentication
- BCrypt para hash de senhas
- Serilog para logs
- Swagger / OpenAPI
- xUnit + FluentAssertions para testes
- Docker + docker-compose

---

## Arquitetura

A solução está dividida em camadas para manter o código organizado e desacoplado:

```text
TaskFlow/
├── src/
│   ├── TaskFlow.Domain
│   │   └── Entidades, enums e regras centrais do domínio
│   ├── TaskFlow.Application
│   │   └── Casos de uso, comandos, queries, DTOs, validadores e interfaces
│   ├── TaskFlow.Infrastructure
│   │   └── Implementações concretas: EF Core, JWT, BCrypt, serviços de infraestrutura
│   └── TaskFlow.Api
│       └── Controllers, configuração da API, middleware, Swagger e injeção de dependência
├── tests/
│   └── TaskFlow.Application.Tests
│       └── Testes unitários dos handlers e validadores
├── TaskFlow.sln
├── Dockerfile
├── docker-compose.yml
├── .gitignore
└── README.md
```

Principais decisões arquiteturais:

- Domain: núcleo do negócio, sem dependência de framework;
- Application: regras e casos de uso com comandos e queries;
- Infrastructure: detalhes técnicos como acesso a dados e autenticação;
- Api: camada de entrada HTTP, endpoints e documentação.

Esse desenho ajuda a manter o código testável, extensível e sem acoplamento desnecessário.

---

## Funcionalidades

- Registro de usuários
- Login com autenticação JWT
- Criação de projetos
- Listagem de projetos
- Criação de tarefas
- Listagem de tarefas com filtros
- Busca de tarefa por Id
- Atualização de tarefa
- Conclusão de tarefa
- Exclusão de tarefa
- Tratamento centralizado de exceções
- Validação automática de entrada
- Documentação interativa com Swagger

---

## Requisitos

Antes de rodar, certifique-se de ter instalado:

- .NET SDK 8.0
- Git
- (opcional) Docker

Para Linux, se o SDK não estiver no PATH, pode instalar localmente com:

```bash
cd ~
curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
bash /tmp/dotnet-install.sh --channel 8.0 --install-dir $HOME/.dotnet
export PATH="$HOME/.dotnet:$PATH"
```

---

## Como rodar localmente

Na raiz do projeto:

```bash
cd /home/raissa/Downloads/TaskFlow
export PATH="$HOME/.dotnet:$PATH"
dotnet restore
dotnet build
dotnet test
```

Para iniciar a API:

```bash
cd /home/raissa/Downloads/TaskFlow
export PATH="$HOME/.dotnet:$PATH"
dotnet run --project src/TaskFlow.Api/TaskFlow.Api.csproj --urls http://localhost:5000


- http://localhost:5000/swagger/index.html
- http://localhost:5000/swagger/v1/swagger.json
```
---

## Configuração da API

O arquivo de configuração principal está em:

- src/TaskFlow.Api/appsettings.json

Ele inclui:

- ConnectionStrings.DefaultConnection
- Jwt.Secret
- Jwt.Issuer
- Jwt.Audience
- Jwt.ExpiryMinutes

Importante: em produção, o valor de `Jwt:Secret` deve ser um segredo forte e aleatório, nunca um valor fixo genérico.

---

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

Todos os endpoints de negócio requerem autenticação JWT, exceto registro e login.

---

## Testes

Os testes estão na pasta:

- tests/TaskFlow.Application.Tests

Executando:

```bash
dotnet test
```

Resultado verificado durante a validação do projeto:

- 5 testes passados
- 0 falhos
- 0 ignorados

---

## Docker

Também existe suporte a containerização com Docker.

Para subir a aplicação via Docker:

```bash
docker compose up --build
```

O arquivo `docker-compose.yml` e o `Dockerfile` já estão preparados para execução da aplicação em container.

---

## Validação executada

Durante a validação do projeto, os comandos abaixo foram executados com sucesso:

```bash
export PATH="$HOME/.dotnet:$PATH"
cd /home/raissa/Downloads/TaskFlow
dotnet build --nologo
dotnet test --nologo --no-build
curl -sS -o /dev/null -w 'HTTP %{http_code}\n' http://localhost:5001/swagger/v1/swagger.json
```

Resultado observado:

- Build succeeded
- Testes: 5/5 aprovados
- HTTP 200 na rota Swagger

Isso confirma que a API está rodando e respondendo corretamente no ambiente validado.

---

## Observações e melhorias futuras

O projeto já está funcional, mas há pontos que podem ser melhorados em evoluções futuras:

- geração de migrations reais em vez de `EnsureCreated`
- autorização mais granular por usuário e projeto
- paginação e ordenação em listagens
- refresh tokens
- health checks
- testes de integração com `WebApplicationFactory`
- atualização da dependência AutoMapper para versão com correção de vulnerabilidade

---

## Licença

Este projeto usa a licença MIT. Consulte o arquivo LICENSE para mais detalhes.