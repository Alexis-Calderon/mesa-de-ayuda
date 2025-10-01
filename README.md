# Mesa de Ayuda

Sistema de mesa de ayuda corporativo desarrollado con .NET 9 (backend) y Angular 17+ (frontend), utilizando SQLite como base de datos.

## Arquitectura

- **Backend**: API REST en .NET 9 con controladores, autenticación JWT y arquitectura limpia
- **Frontend**: Aplicación Angular con componentes, servicios y guards
- **Base de Datos**: SQLite con Entity Framework Core y migraciones
- **Autenticación**: JWT con roles (Administrador, Técnico, Cliente)
- **Contenedor**: Docker para fácil despliegue

## Características

### Roles de Usuario
- **Administrador**: Gestión completa de usuarios, tickets y configuraciones
- **Técnico**: Resolución de tickets asignados, actualización de estados y comentarios
- **Cliente**: Creación de tickets y seguimiento de sus propios casos

### Funcionalidades
- ✅ Autenticación JWT
- ✅ Gestión de usuarios (CRUD)
- ✅ Sistema de tickets con estados y prioridades
- ✅ Comentarios en tickets
- ✅ Autorización basada en roles
- ✅ API RESTful documentada
- ✅ Base de datos SQLite con migraciones
- ✅ Contenedor Docker

## Usuario Administrador por Defecto

Al iniciar la aplicación por primera vez, se crea automáticamente un usuario administrador con las siguientes credenciales:

- **RUT**: `11111111-1`
- **Contraseña**: `Admin123!`
- **Rol**: Administrador

**⚠️ Importante**: Cambia la contraseña después del primer inicio de sesión por motivos de seguridad.

## Requisitos Previos

- Docker y Docker Compose
- .NET 9 SDK (para desarrollo local)
- Node.js 18+ y Angular CLI (para desarrollo frontend)

## Ejecución con Docker

### Opción 1: Usando Docker Compose (Recomendado)

```bash
# Construir e iniciar todos los servicios (API + Frontend)
docker-compose up --build

# O usando la nueva sintaxis de Docker Compose v2
docker compose up --build

# La aplicación completa estará disponible en http://localhost:8080
# API interna disponible en http://localhost:8080/api/
```

### Servicios Incluidos:
- **Frontend (Angular)**: `http://localhost:8080` - Interfaz de usuario
- **Backend (API .NET)**: `http://localhost:8080/api/` - API REST (accesible vía proxy)
- **Base de Datos**: SQLite persistente en volumen Docker

### Opción 2: Construir imagen manualmente

```bash
# Navegar al directorio de la API
cd backend/src/MesaDeAyuda.Api

# Construir la imagen
docker build -t mesa-ayuda-api .

# Ejecutar el contenedor
docker run -p 8080:8080 -v mesa-ayuda-data:/app/App_Data mesa-ayuda-api
```

## Desarrollo Local

### Backend

```bash
# Navegar al directorio backend
cd backend/src/MesaDeAyuda.Api

# Restaurar dependencias
dotnet restore

# Ejecutar migraciones
dotnet ef database update

# Iniciar la API
dotnet run
```

### Frontend

```bash
# Navegar al directorio frontend
cd client

# Instalar dependencias
npm install

# Iniciar aplicación
ng serve
```

## Endpoints de la API

### Autenticación
- `POST /api/auth/login` - Iniciar sesión

### Usuarios (Solo Administrador)
- `GET /api/usuarios` - Listar usuarios
- `POST /api/usuarios` - Crear usuario
- `PUT /api/usuarios/{rut}` - Actualizar usuario
- `DELETE /api/usuarios/{rut}` - Eliminar usuario

### Tickets
- `GET /api/tickets` - Listar tickets (filtrado por rol)
- `POST /api/tickets` - Crear ticket (solo Cliente)
- `PUT /api/tickets/{id}` - Actualizar ticket
- `PUT /api/tickets/{id}/asignar` - Asignar técnico (Admin)
- `PUT /api/tickets/{id}/resolver` - Resolver ticket

### Comentarios
- `GET /api/tickets/{ticketId}/comentarios` - Listar comentarios
- `POST /api/tickets/{ticketId}/comentarios` - Agregar comentario

## Variables de Entorno

| Variable | Descripción | Valor por defecto |
|----------|-------------|-------------------|
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | Production |
| `ConnectionStrings__DefaultConnection` | Cadena de conexión SQLite | Data Source=/app/App_Data/mesa_de_ayuda.db |
| `Jwt__Key` | Clave secreta JWT | YourSuperSecretKeyHere12345678901234567890 |
| `Jwt__Issuer` | Emisor del token | MesaDeAyuda.Api |
| `Jwt__Audience` | Audiencia del token | MesaDeAyuda.Client |
| `Jwt__ExpiryInMinutes` | Expiración del token | 60 |

## Estructura del Proyecto

```
mesa-de-ayuda/
├── backend/
│   ├── src/
│   │   ├── MesaDeAyuda.Api/          # Proyecto API
│   │   ├── MesaDeAyuda.Data/         # Capa de datos
│   │   └── MesaDeAyuda.Domain/       # Entidades y enums
│   └── tests/                        # Pruebas unitarias
├── client/                           # Frontend Angular
├── docker-compose.yml                # Configuración Docker
└── arquitectura.md                   # Documentación detallada
```

## Tecnologías Utilizadas

- **Backend**: .NET 9, ASP.NET Core, Entity Framework Core, SQLite
- **Frontend**: Angular 17+, TypeScript, RxJS
- **Autenticación**: JWT Bearer Tokens
- **Base de Datos**: SQLite con migraciones EF Core
- **Contenedor**: Docker, Docker Compose
- **Documentación**: Swagger/OpenAPI

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agrega nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.