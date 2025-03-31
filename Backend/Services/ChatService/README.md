# MesajX ChatService

Chat microservice for the MesajX application.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/downloads)

## Getting Started

1. Clone the repository:
```bash
git clone <repo-url>
```

2. Navigate to the ChatService directory:
```bash
cd MesajX/Backend/Services/ChatService
```

3. Start the Docker containers:
```bash
docker-compose up --build
```

## Service URLs & Ports

- **ChatService API**: http://localhost:5000
- **PostgreSQL**:
  - Host: localhost
  - Port: 5432
  - Database: MesajXChatDb
  - Username: postgres
  - Password: admin123D!
- **Redis**:
  - Host: localhost
  - Port: 6379

## Architecture

The service is built using a layered architecture:
- MesajX.ChatService (API Layer)
- MesajX.ChatService.BusinessLayer
- MesajX.ChatService.DataAccessLayer
- MesajX.ChatService.DtoLayer
- MesajX.ChatService.EntityLayer

## Technologies

- .NET 8.0
- PostgreSQL
- Redis
- Docker
- Entity Framework Core
- AutoMapper
- FluentValidation
