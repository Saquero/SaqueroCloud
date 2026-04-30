# SaqueroCloud – SaaS Admin Platform

SaqueroCloud is a full stack SaaS-style admin platform built with **ASP.NET Core (.NET 8)** and **React**.

The project simulates a real subscription management system where an admin can manage users, subscription plans and active subscriptions from a modern dashboard connected to a secure REST API.

---

## Preview

### Dashboard
![Dashboard](./assets/dashboard.png)

### Users
![Users](./assets/users.png)

### Plans
![Plans](./assets/plans.png)

### Subscriptions
![Subscriptions](./assets/subscriptions.png)

---

## Features

- JWT authentication
- Role-based access control
- User management
- Subscription plan management
- Assign subscriptions to users
- Cancel active subscriptions
- Filter subscriptions by plan
- Track expiring subscriptions
- Server-side pagination
- Swagger API documentation
- REST Client testing file
- Modern React admin dashboard

---

## Tech Stack

### Backend
- C#
- ASP.NET Core (.NET 8)
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger / OpenAPI
- Middleware
- Repository / Service pattern

### Frontend
- React
- Vite
- JavaScript
- CSS
- Axios
- Lucide React

### Tools
- PowerShell
- Git
- VS Code
- REST Client
- Swagger

---

## Project Structure

```txt
SaqueroCloud/
├── SaqueroCloud.API/
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── Data/
│   ├── Middleware/
│   └── Program.cs
│
├── saquerocloud-frontend/
│   ├── src/
│   ├── public/
│   └── package.json
│
├── assets/
├── DECISIONS.md
├── requests.http
└── README.md
Backend Setup

From the backend folder:

cd SaqueroCloud.API
dotnet restore
dotnet run --urls="http://127.0.0.1:5000"

API:

http://127.0.0.1:5000

Swagger:

http://127.0.0.1:5000
Frontend Setup

From the frontend folder:

cd saquerocloud-frontend
npm install
npm run dev

Frontend:

http://localhost:5173

The port may change if Vite detects another app already running.

Default Credentials
Email: Saquero@pruebas.com
Password: Admin1234!
Main API Endpoints
Auth
POST /api/Auth/login
POST /api/Auth/register
Users
GET /api/Users?page=1&pageSize=10
GET /api/Users/{id}
PUT /api/Users/{id}
DELETE /api/Users/{id}
Subscription Plans
GET /api/subscription-plans
POST /api/subscription-plans
PUT /api/subscription-plans/{id}
DELETE /api/subscription-plans/{id}
Subscriptions
GET /api/Subscriptions/active
GET /api/Subscriptions/active?planId=1
GET /api/Subscriptions/expiring-soon?days=30
POST /api/Subscriptions/assign/{userId}
PATCH /api/Subscriptions/{subscriptionId}/cancel
Testing

The project includes:

requests.http

This file allows testing the API directly from VS Code using the REST Client extension.

Technical Decisions

Some technical decisions are documented in:

DECISIONS.md

Examples:

SQLite for local development
Layered architecture
DTO usage
JWT authentication
Separation between frontend and backend
Why this project matters

This project is not just a CRUD demo.

It includes real backend concepts commonly used in professional applications:

Authentication and authorization
Business rules
Relational data modelling
API documentation
Error handling
Frontend/backend integration
Admin dashboard workflows
Future Improvements
Deploy frontend and backend online
Add automated tests
Add Serilog structured logging
Add refresh tokens
Add user editing from the dashboard
Add production database support with PostgreSQL
Add Docker support
License

MIT © 2026 Manu Saquero

Contact

Created by Manu Saquero

GitHub: https://github.com/Saquero

LinkedIn: https://linkedin.com/in/manusaquero

Email: Contactar