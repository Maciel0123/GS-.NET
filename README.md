## 📌 WScore API — Future of Work | Global Solution (.NET)

A WScore API é uma solução desenvolvida para monitoramento de bem-estar no ambiente de trabalho, permitindo que usuários registrem diariamente seu estado emocional, nível de energia, sono, foco e carga de trabalho.
A API calcula automaticamente um WScore, gera feedback inteligente, e organiza os registros com boas práticas REST, versionamento, observabilidade e testes automatizados.

## 🚀 Tecnologias Utilizadas

.NET 9 / ASP.NET Core

Entity Framework Core (Oracle + InMemory para testes)

API Versioning (Asp.Versioning)

Swagger / OpenAPI com suporte a versões

HATEOAS

Serilog

OpenTelemetry (Logs + Tracing)

Health Checks

xUnit (Testes de Integração)

## 📂 Arquitetura
📦 WScore
 ├── WScoreApi             → Controllers, Swagger, Versionamento, Observabilidade
 ├── WScoreBusiness        → Services, Regras de Negócio, Cálculo do Score
 ├── WScoreDomain          → Entidades e Objetos de Domínio
 ├── WScoreData  → DbContext, Migrations, Persistência
 ├── WScoreTests           → Testes integrados com WebApplicationFactory


Padrão orientado a camadas, isolando responsabilidades e garantindo entregabilidade corporativa.

## 🔢 Versionamento da API

A API segue o padrão:

/api/v1/...


Com suporte configurado para múltiplas versões no futuro (Swagger já preparado para v1, v2…).

## 📘 Endpoints Principais
### Checkins
| Método | Rota                                | Descrição                                         |
| ------ | ----------------------------------- | ------------------------------------------------- |
| GET    | `/api/v1/checkins`                  | Lista todos com HATEOAS                           |
| GET    | `/api/v1/checkins/paginado`         | Lista paginado                                    |
| GET    | `/api/v1/checkins/{id}`             | Busca por ID                                      |
| GET    | `/api/v1/checkins/usuario/{userId}` | Lista checkins de um usuário (404 se não existir) |
| POST   | `/api/v1/checkins`                  | Cria checkin, calcula score e gera feedback       |
| PUT    | `/api/v1/checkins`                  | Atualiza checkin                                  |
| DELETE | `/api/v1/checkins/{id}`             | Remove checkin                                    |


### Users
| Método | Rota                     | Descrição        |
| ------ | ------------------------ | ---------------- |
| GET    | `/api/v1/users`          | Lista todos      |
| GET    | `/api/v1/users/paginado` | Lista paginado   |
| GET    | `/api/v1/users/{id}`     | Busca por ID     |
| POST   | `/api/v1/users`          | Cria usuário     |
| PUT    | `/api/v1/users`          | Atualiza usuário |
| DELETE | `/api/v1/users/{id}`     | Remove usuário   |


## 🧮 Cálculo do Score

O WScore é calculado com base nos seguintes atributos:

Humor

Energia

Foco

Carga de Trabalho

Sono (invertido → quanto MENOS sono, maior impacto positivo no score)

E inclui geração automática de feedback personalizado, ex:

“Sua carga de trabalho está muito alta, tente redistribuir atividades.”

“Você está com pouco sono, tente descansar mais hoje.”

## 🧠 Feedback Inteligente

O feedback é armazenado no banco e varia dependendo dos valores:

Sono baixo

Foco baixo

Humor baixo

Energia baixa

Carga de trabalho alta

O objetivo é orientar o bem-estar do usuário em tempo real.

## 🧪 Testes Automatizados (xUnit)

Inclui testes reais:

Criar usuário

Criar checkin

Testar paginação

Com CustomWebApplicationFactory, isolando banco e ambiente de teste.

## 🌡 Health Checks

Disponível em:

/health


Retorna 200 se a API estiver operacional.

## 🔍 Observabilidade Completa
✔ Serilog

Logs estruturados

Console sink

Contextualização automática

✔ OpenTelemetry

Tracing (rastreamento de requisições)

Logging (correlação no pipeline)

Console Exporter

Isso garante rastreabilidade ponta a ponta.

## 🗄 Banco de Dados (Oracle)

Entidade principal:

TB_CHECKINS
 - Id
 - DataCheckin
 - Humor
 - Sono
 - Energia
 - Foco
 - CargaTrabalho
 - Score
 - Feedback
 - UserId (FK)

## ▶️ Como Rodar o Projeto
1. Restaurar pacotes
```
dotnet restore
```

2. Rodar as migrations
```
cd WScoreData
dotnet ef database update
```

3. Executar a API
```
cd WScoreApi
dotnet run
```

A API iniciará em:
```
https://localhost:5221
```

4. Se quiser executar os testes:
```
dotnet test
```

## 📚 Swagger

Disponível em:
```
https://localhost:5221/swagger
```

Com suporte a versionamento:
```
/swagger/v1/swagger.json
```
## 🧑‍💻 Integrantes

- Henrique Maciel - RM556480
- Gabriela Moguinho Gonçalves — RM556143
- Mariana Christina Fernandes Rodrigues - RM554773