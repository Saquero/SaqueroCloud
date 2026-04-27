# SaqueroCloud API

Proyecto personal desarrollado con ASP.NET Core (.NET 8) para practicar backend y diseño de APIs REST.

## ¿Qué hace este proyecto?

Esta API permite gestionar usuarios y suscripciones tipo SaaS de forma sencilla.

Incluye:

- Registro y login con JWT
- Sistema de roles (Admin / User)
- Gestión de usuarios
- Gestión de planes de suscripción
- Asignación de suscripciones a usuarios

## Tecnologías utilizadas

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger

## Cómo ejecutarlo

```bash
dotnet run

Acceder en:

http://localhost:5000

Usuario de prueba

Email: admin@saquerocloud.com

Password: Admin1234!

Estructura del proyecto
SaqueroCloud.API/
├── Controllers/
├── Services/
├── Repositories/
├── Models/
│   ├── Entities/
│   └── DTOs/
├── Data/
├── Middleware/
├── Program.cs
Qué estoy practicando con este proyecto
Arquitectura por capas
Diseño de APIs REST
Autenticación con JWT
Entity Framework Core
Notas

Proyecto en evolución. Mi idea es seguir añadiendo funcionalidades para mejorar la lógica de negocio y practicar casos más reales.