# Plataforma de Eventos — MVP

Plataforma de eventos online con arquitectura de microservicios, event-driven, Clean Architecture + DDD.

## Stack Tecnológico

- **Backend**: .NET 9, EF Core, MediatR (CQRS), FluentValidation, MassTransit + RabbitMQ, Redis
- **Frontend**: React 19, TypeScript, Vite, Tailwind CSS 4
- **BD**: SQL Server 2022
- **Cache**: Redis 7
- **Mensajería**: RabbitMQ 3
- **Contenedores**: Docker + Docker Compose

## Estructura del Proyecto

```
plataforma-eventos/
├── docs/
│   └── architecture.md          # Diagrama y documentación de arquitectura
├── scripts/
│   └── seed-data.sql            # Script de datos iniciales (opcional)
├── src/
│   ├── backend/
│   │   ├── EventPlatform.sln
│   │   ├── EventService.API/             # API REST (.NET 9)
│   │   ├── EventService.Application/     # CQRS, DTOs, Validators
│   │   ├── EventService.Domain/          # Entidades DDD
│   │   ├── EventService.Infrastructure/  # EF Core, RabbitMQ, Redis
│   │   ├── NotificationService.API/      # Consumer de mensajes
│   │   ├── NotificationService.*/        # Capas del servicio
│   │   └── Shared.Contracts/             # Mensajes compartidos
│   └── frontend/
│       └── event-platform-web/           # React + TypeScript
├── docker-compose.yml
└── README.md
```

## Inicio Rápido (Docker Compose)

### Prerequisitos
- Docker Desktop (o Podman Desktop)
- Git

### Levantar todo el sistema

```bash
# Clonar el repositorio
git clone <url-del-repo>
cd plataforma-eventos

# Levantar todos los servicios
docker compose up --build -d
```

### URLs de acceso

| Servicio | URL |
|----------|-----|
| Frontend (React) | http://localhost:3000 |
| EventService API | http://localhost:5010/swagger |
| NotificationService API | http://localhost:5020/swagger |
| RabbitMQ Management | http://localhost:15672 (guest/guest) |

### Detener servicios

```bash
docker compose down
```

Para limpiar volúmenes (datos):
```bash
docker compose down -v
```

## Desarrollo Local (sin Docker)

### Prerequisitos
- .NET 9 SDK
- Node.js 20+
- SQL Server corriendo localmente (o LocalDB)
- RabbitMQ corriendo localmente
- Redis corriendo localmente

### Backend

```bash
cd src/backend

# Restaurar paquetes
dotnet restore

# Ejecutar migraciones (EventService)
dotnet ef database update --project EventService.Infrastructure --startup-project EventService.API

# Ejecutar migraciones (NotificationService)
dotnet ef database update --project NotificationService.Infrastructure --startup-project NotificationService.API

# Ejecutar EventService
dotnet run --project EventService.API

# En otra terminal, ejecutar NotificationService
dotnet run --project NotificationService.API
```

> **Nota**: Para desarrollo local, actualizar los connection strings en `appsettings.Development.json` apuntando a `localhost` en lugar de los nombres de servicio Docker.
>
> Ejemplo connection string local: `Server=localhost;Database=EventDb;User Id=sa;Password=Str0ngP@ssw0rd!;TrustServerCertificate=True`

### Datos iniciales (Seed)

Opcionalmente, ejecutar el script de seed data con SSMS o sqlcmd:

```bash
sqlcmd -S localhost -U sa -P "Str0ngP@ssw0rd!" -i scripts/seed-data.sql
```

### Frontend

```bash
cd src/frontend/event-platform-web

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

El frontend estará disponible en `http://localhost:3000`.

## Migraciones de Base de Datos

Las migraciones de EF Core se aplican automáticamente al iniciar cada API (en `Program.cs`). Para aplicarlas manualmente:

```bash
cd src/backend

# EventService
dotnet ef database update --project EventService.Infrastructure --startup-project EventService.API

# NotificationService
dotnet ef database update --project NotificationService.Infrastructure --startup-project NotificationService.API
```

### Agregar nueva migración

```bash
dotnet ef migrations add <NombreMigracion> --project EventService.Infrastructure --startup-project EventService.API --output-dir Persistence/Migrations
```

## Endpoints de la API

### EventService (Puerto 5010)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| POST | /api/events | JWT | Crear evento con zonas |
| GET | /api/events | No | Listar eventos (con cache) |
| GET | /api/events/{id} | No | Detalle de un evento |

### Ejemplo de request (POST /api/events)

```json
{
  "name": "Concierto de Rock 2026",
  "date": "2026-12-15T20:00:00Z",
  "location": "Arena CDMX",
  "zones": [
    { "name": "VIP", "price": 2500.00, "capacity": 500 },
    { "name": "General", "price": 800.00, "capacity": 5000 },
    { "name": "Preferente", "price": 1500.00, "capacity": 1000 }
  ]
}
```

### JWT Token para Demo

Para probar en Swagger o Postman, usar el siguiente header:
```
Authorization: Bearer <token>
```

El token se puede generar con la clave secreta: `S3cur3K3yF0rD3m0Purp0s3sM1n32Chars!!`, issuer `EventPlatform`, audience `EventPlatformClient`.

## Arquitectura

Ver documentación completa en [docs/architecture.md](docs/architecture.md).

### Patrones implementados
- Clean Architecture + DDD
- CQRS con MediatR
- Event-Driven Architecture (RabbitMQ/MassTransit)
- Idempotent Consumer
- Cache-Aside Pattern (Redis)
- Database per Service
