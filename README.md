# VideoProcessorX.AuthService

## 📌 Visão Geral
O **VideoProcessorX.AuthService** é um serviço de autenticação robusto para o processamento de vídeos, desenvolvido com **.NET 8**, utilizando **JWT para autenticação**, **Entity Framework Core para persistência de dados**, e **RabbitMQ para eventos assíncronos**.

## 🚀 Tecnologias Utilizadas
- **.NET 8** - Plataforma de desenvolvimento
- **Entity Framework Core** - ORM para manipulação de dados
- **ASP.NET Core Web API** - Backend do serviço
- **JWT (JSON Web Token)** - Autenticação segura
- **RabbitMQ** - Mensageria para eventos assíncronos
- **BCrypt.Net** - Hashing de senhas
- **Docker** - Containerização do serviço
- **Kubernetes** - Orquestração de containers (planejado)
- **Terraform** - Infraestrutura como código (planejado)
- **Azure Kubernetes Service (AKS)** - Implantação em nuvem (planejado)

## 📁 Estrutura do Projeto

```
AuthService/
│── docker-compose.yml
│── Dockerfile
│── src/
│   ├── AuthService.Application/
│   │   ├── Common/  # Classes genéricas e padrões
│   │   ├── DTOs/    # Data Transfer Objects (DTOs)
│   │   ├── Services/  # Serviços de autenticação
│   ├── AuthService.Domain/
│   │   ├── Entities/  # Modelos de domínio (Usuário, Vídeo, etc.)
│   │   ├── Interfaces/  # Interfaces de repositórios e serviços
│   ├── AuthService.Infraestructure/
│   │   ├── Data/  # DbContext (EF Core)
│   │   ├── Messaging/  # Publicação de eventos com RabbitMQ
│   │   ├── Persistence/  # Repositórios concretos
│   │   ├── Security/  # Implementação de JWT
│   ├── AuthService.Presentation/
│   │   ├── Controllers/  # Controladores da API REST
│   │   ├── Program.cs  # Configuração inicial do serviço
│── tests/
│   ├── AuthService.IntegrationTests/
│   ├── AuthService.UnitTests/
│── README.md
```

## ⚙️ Funcionalidades
✔ **Registro de usuários** com hash seguro de senha (BCrypt)  
✔ **Autenticação de usuários** com JWT  
✔ **Publicação de eventos de usuário criado** no RabbitMQ  
✔ **Persistência de dados** com EF Core  
✔ **Testes unitários e integração** com xUnit  
✔ **Containerização com Docker**  

## 🔧 Configuração e Execução

### 1️⃣ Pré-requisitos
Certifique-se de ter instalado:
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/)
- [RabbitMQ](https://www.rabbitmq.com/download.html) (caso queira rodar localmente)

### 2️⃣ Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/VideoProcessorX.AuthService.git
cd VideoProcessorX.AuthService
```

### 3️⃣ Configurar Variáveis de Ambiente
Crie um arquivo **appsettings.json** no diretório `AuthService.Presentation` com o seguinte conteúdo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthServiceDB;User Id=sa;Password=YourPassword;"
  },
  "Jwt": {
    "Key": "SuperSecretKeyForJwt",
    "Issuer": "VideoProcessorX",
    "Audience": "VideoProcessorXClients"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

### 4️⃣ Rodar a Aplicação

#### 🔹 Localmente (Sem Docker)
```bash
dotnet build
dotnet run --project src/AuthService.Presentation
```

#### 🔹 Com Docker
```bash
docker build -t videoprocessorx-authservice .
docker run -p 8080:8080 -e "ASPNETCORE_ENVIRONMENT=Development" videoprocessorx-authservice
```

#### 🔹 Com Docker-Compose (Banco de dados + RabbitMQ)
```bash
docker-compose up -d
```

## 📌 Endpoints da API

| Método | Rota              | Descrição                          | Autenticação |
|--------|-------------------|----------------------------------|--------------|
| POST   | `/api/auth/register` | Cria um novo usuário            | ❌ |
| POST   | `/api/auth/login`    | Gera um token JWT               | ❌ |
| GET    | `/api/users`         | Lista usuários (futuro)         | ✅ |


## 📜 Licença
Este projeto está sob a licença **MIT**.

---

Feito com ❤️ por Roberto Albano

