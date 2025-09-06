# Consware Backend API

API backend desarrollada con **.NET 9** para gestión de usuarios y solicitudes de viaje, con autenticación JWT y roles.

## Tecnologías

- .NET 9
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI
- Docker

## Estructura del proyecto

backend/
│
├─ Controllers/
│ ├─ UserController.cs
│ └─ TravelRequestController.cs
│
├─ Dtos/
│ ├─ User/
│ │ ├─ UserCreateDto.cs
│ │ ├─ UserLoginDto.cs
│ │ └─ UpdateUserDto.cs
│ └─ TravelRequest/
│ ├─ TravelRequestCreateDto.cs
│ └─ TravelRequestUpdateDto.cs
│
├─ Model/
│ ├─ User.cs
│ └─ TravelRequest.cs
│
├─ Repository/
│ ├─ IUser.cs
│ └─ ITravelRequest.cs
│
├─ Service/
│ ├─ UserS.cs
│ └─ TravelRequestS.cs
│
├─ Utils/
│ ├─ ApiResponse.cs
│ └─ TravelRequestStatus.cs
│
├─ Data/
│ └─ DataContext.cs
│
├─ backend.csproj
└─ Program.cs

## Base URL

http://localhost:5000/api


Todos los endpoints que requieren autenticación deben incluir el header:

Authorization: Bearer {token}


---

## Endpoints de Usuario

### 1. Registrar usuario

**POST** `/user/register`

**Request Body:**

json
{
  "name": "Martín",
  "email": "martin@example.com",
  "password": "Password123!",
  "role": "Aprobador"
}
response : 
{
  "data": {
    "id": 1,
    "name": "Martín",
    "role": "Aprobador",
    "createdAtUtc": "2025-09-05T15:00:00Z",
    "updatedAtUtc": "2025-09-05T15:00:00Z"
  },
  "error": null,
  "status": 201
}

2. Login de usuario

POST /user/login

Request Body:

{
  "email": "martin@example.com",
  "password": "Password123!"
}
Response 200 OK:

{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "name": "Martín",
      "role": "Aprobador",
      "createdAtUtc": "2025-09-05T15:00:00Z",
      "updatedAtUtc": "2025-09-05T15:00:00Z"
    }
  },
  "error": null,
  "status": 200
}

3. Actualizar contraseña

PUT /user/update-password

Headers:

Authorization: Bearer {token}


Request Body:

{
  "email": "martin@example.com",
  "password": "NewPassword123!"
}


Response 200 OK:

{
  "data": "Contraseña actualizada correctamente",
  "error": null,
  "status": 200
}

4. Obtener todos los usuarios (solo Aprobador)

GET /user/all

Headers:

Authorization: Bearer {token}


Response 200 OK:

{
  "data": [
    {
      "id": 1,
      "name": "Martín",
      "role": "Aprobador",
      "createdAtUtc": "2025-09-05T15:00:00Z",
      "updatedAtUtc": "2025-09-05T15:00:00Z"
    },
    {
      "id": 2,
      "name": "Ana",
      "role": "Usuario",
      "createdAtUtc": "2025-09-05T15:05:00Z",
      "updatedAtUtc": "2025-09-05T15:05:00Z"
    }
  ],
  "error": null,
  "status": 200
}

Endpoints de Solicitudes de Viaje
1. Crear solicitud de viaje

POST /travelrequest/create

Headers:

Authorization: Bearer {token}


Request Body:

{
  "userId": 2,
  "originCity": "Bogotá",
  "destinationCity": "Medellín",
  "departureDate": "2025-09-10",
  "returnDate": "2025-09-15",
  "justification": "Viaje de negocios"
}


Response 201 Created:

{
  "data": {
    "id": 1,
    "userId": 2,
    "originCity": "Bogotá",
    "destinationCity": "Medellín",
    "departureDate": "2025-09-10",
    "returnDate": "2025-09-15",
    "justification": "Viaje de negocios",
    "status": 1,
    "createdAtUtc": "2025-09-05T15:10:00Z",
    "updatedAtUtc": "2025-09-05T15:10:00Z"
  },
  "error": null,
  "status": 201
}

2. Obtener todas las solicitudes (solo Aprobador)

GET /travelrequest/all

Headers:

Authorization: Bearer {token}


Response 200 OK:

{
  "data": [
    {
      "id": 1,
      "userId": 2,
      "originCity": "Bogotá",
      "destinationCity": "Medellín",
      "departureDate": "2025-09-10",
      "returnDate": "2025-09-15",
      "justification": "Viaje de negocios",
      "status": 1,
      "createdAtUtc": "2025-09-05T15:10:00Z",
      "updatedAtUtc": "2025-09-05T15:10:00Z"
    }
  ],
  "error": null,
  "status": 200
}

3. Actualizar estado de solicitud (solo Aprobador)

PUT /travelrequest/update-status

Headers:

Authorization: Bearer {token}


Request Body:

{
  "id": 1,
  "status": 2
}


status puede ser:

1 = pending

2 = approved

3 = rejected

Response 200 OK:

{
  "data": "Estado actualizado a approved en 2025-09-05T15:20:00Z",
  "error": null,
  "status": 200
}

Docker
Construir imagen
docker build -t backend-api .

Ejecutar contenedor en desarrollo
docker run -d -p 5000:80 -e DOTNET_ENVIRONMENT=Development --name backend-container backend-api

Detener contenedor
docker stop backend-container

Eliminar contenedor
docker rm backend-container

Swagger UI

Accede a: http://localhost:5000/swagger/index.html

Desde Swagger puedes probar todos los endpoints protegidos enviando el JWT en Authorize.

Notas

Las fechas se manejan en hora Bogotá, Colombia.

Contraseñas siempre se guardan hasheadas.

Roles (Aprobador, Usuario) controlan permisos de endpoints.

ApiResponse<T> unifica todas las respuestas de la API.


